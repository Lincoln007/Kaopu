using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Awesomium;
using Awesomium.Core;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;
using Ztop.Todo.Common;

namespace Ztop.Todo.WindowsClient
{
    public partial class MainForm : Form
    {
        private Thread thread { get; set; }
        private bool IsLive { get; set; }
        private LoginForm _loginForm { get; set; }
        public MainForm()
        {
            InitializeComponent();
            if (!WebCore.IsInitialized)
            {
                WebCore.Initialize(WebConfig.Default, true);
                WebCore.ResourceInterceptor = new ResourceInterceptor();
            }
            
          
        }

        public MainForm(string uploadFile) : this()
        {
            Control.CheckForIllegalCrossThreadCalls = false;

            if (!string.IsNullOrEmpty(uploadFile))
            {
                var savePath = string.Empty;
                try
                {
                    savePath = FTPHelper.UploadFile(uploadFile);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("上传文件到服务器失败，错误信息：" + ex.ToString());
                }
                webControl1.Source = new Uri(ServerHelper.GetServerUrl() + "/task/edit?file=" + savePath);
            }
            else
            {
                webControl1.Source = new Uri(ServerHelper.GetServerUrl());
            }


            IsLive = true;
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            var task = ServerHelper.GetNewTask();
            if (task != null)
            {
                foreach (Form nf in Application.OpenForms)
                {
                    //如果已经有一个提醒和当前查询的新任务同一个任务，就不再提醒了
                    if (nf is NoticeForm)
                    {
                        if (((NoticeForm)nf).Task.ID == task.ID)
                        {
                            return;
                        }
                    }
                }

                var form = new NoticeForm(task);
                form.StartPosition = FormStartPosition.Manual;
                form.WindowState = FormWindowState.Normal;
                form.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - form.Width - 5, Screen.PrimaryScreen.WorkingArea.Height - form.Height - 5);
                form.Show();
            }
        }

        public void OpenTask(string UriPath)
        {
            webControl1.Source = new Uri(ServerHelper.GetServerUrl() + UriPath);
        }


        public void OpenTask(Task model)
        {
            webControl1.Source = new Uri(ServerHelper.GetTaskUrl(model));
            OpenWindow();
        }

        private void OpenWindow()
        {
            this.Show();
            this.Activate();
            Size = _size;
            WindowState = _state;
        }

        private void CloseWindow()
        {
            _state = WindowState;
            _size = Size;
            this.WindowState = FormWindowState.Minimized;
            //this.Hide();
        }


        private FormWindowState _state;
        private Size _size;

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                CloseWindow();
            }
            
        }

        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            var e1 = e as MouseEventArgs;
            if (e1.Button != MouseButtons.Right)
            {
                if (WindowState == FormWindowState.Minimized)
                {
                    OpenWindow();
                }
                else
                {
                    CloseWindow();
                }
            }
        }

        private void Stop()
        {
            webControl1.Dispose();
            WebCore.DestroyUnwrappedViews();

            timer1.Stop();
            timer1.Dispose();
            if (this.thread != null && this.thread.IsAlive)
            {
                TCPHelper.TCPSend(System.Configuration.ConfigurationManager.AppSettings["TCPSTOP"]);
                this.thread.Join();
            }
            

        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stop();
            this.Close();
            _loginForm.Close();
        }

        private void Start()
        {
            var listener = new TODOListener(this);
            this.thread = new Thread(listener.Listen);
            this.thread.IsBackground = false;
            this.thread.Start();

            timer1.Tick += Timer1_Tick;
            timer1.Interval = 1000 * 10;
            timer1.Start();
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            Start();
            _loginForm = this.Owner as LoginForm;
        }

        private delegate void FlushClient(string FileOne);
        public void ThreadFunction(string FileOne)
        {
            if (this.webControl1.InvokeRequired)
            {
                FlushClient fs = new FlushClient(ThreadFunction);
                this.Invoke(fs, new[] { FileOne });
            }
            else
            {
                OpenTask("/task/edit?FileOne=" + FileOne);
            }
        }


        private void 注销ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            Stop();
            LoginHelper.Logout();
            
            this.Close();
            this.Dispose();
            _loginForm.Show();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {

        }
    }
}
