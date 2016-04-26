using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Ztop.Todo.WindowsClient
{
    public partial class NoticeForm : Form
    {
        public NoticeForm()
        {
            InitializeComponent();
            timer1.Tick += Timer1_Tick;
            timer1.Interval = 1000;
            //timer1.Start(); 不需要自动关闭
        }

        private int _duration = 30;

        private void Timer1_Tick(object sender, EventArgs e)
        {

        }

        public NoticeForm(Notification notification):this()
        {
            Notification = notification;
            txtContent.Text = notification.Description;
        }

        public Notification Notification { get; private set; }

        private void NavigationToTaskDetailPage()
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form is MainForm)
                {
                    form.Show();
                    ((MainForm)form).OpenTask(Notification);
                }
            }
        }

        private void btnTitle_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            NavigationToTaskDetailPage();
            this.Close();
        }


        private void labContent_Click(object sender, EventArgs e)
        {
            btnTitle_LinkClicked(null, null);
        }

        protected override void OnClosed(EventArgs e)
        {
            timer1.Stop();
            timer1.Dispose();
            base.OnClosed(e);
        }

        private void NoticeForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(e.CloseReason == CloseReason.UserClosing)
            {

            }
        }
    }
}
