using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Awesomium;

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
            timer1.Interval = 1000;
            timer1.Start();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            var task = ServerHelper.GetNewTask();
            if (task != null)
            {
                var form = new NoticeForm(task);
                form.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - Width - 5, Screen.PrimaryScreen.WorkingArea.Height - Height - 5);
                form.Show();
            }
        }

        public static string AccessToken { get; private set; }

        private void WebControl1_DocumentReady(object sender, Awesomium.Core.DocumentReadyEventArgs e)
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
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }
    }
}
