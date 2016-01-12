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

namespace Ztop.Todo.WindowsClient
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            webControl1.Source = new Uri(ServerHelper.GetServerUrl());
            webControl1.DocumentReady += WebControl1_DocumentReady;

            timer1.Tick += Timer1_Tick;
            timer1.Interval = 1000 * 10;
            timer1.Start();
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
                form.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - form.Width - 5 , Screen.PrimaryScreen.WorkingArea.Height - form.Height - 5);
                form.Show();
            }
        }

        public static string AccessToken { get; private set; }

        private void WebControl1_DocumentReady(object sender, DocumentReadyEventArgs e)
        {
            if (string.IsNullOrEmpty(AccessToken))
            {
                var cookie = webControl1.ExecuteJavascriptWithResult("document.cookie;");
                if (cookie != null && cookie.IsString)
                {
                    var cookieStr = cookie.ToString();
                    if (!string.IsNullOrEmpty(cookieStr))
                    {
                        AccessToken = cookieStr.Substring(".user=".Length);
                    }
                }
            }
        }

        public void OpenTask(Task model)
        {
            webControl1.Source = new Uri(ServerHelper.GetTaskUrl(model));
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                this.WindowState = FormWindowState.Minimized;
                this.Hide();
                e.Cancel = true;
            }
        }

        private void notifyIcon1_Click(object sender, EventArgs e)
        {
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WebCore.DestroyUnwrappedViews();
            webControl1.Dispose();
            timer1.Stop();
            timer1.Dispose();
            Application.Exit();
        }
    }
}
