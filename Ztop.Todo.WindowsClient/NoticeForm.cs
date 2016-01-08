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
        }

        private int _duration = 10;

        private void Timer1_Tick(object sender, EventArgs e)
        {
            _duration--;
            if(_duration == 0)
            {
                this.Close();
            }
        }

        public NoticeForm(Task task):this()
        {
            Task = task;
            btnTitle.Text = task.Title;
            labContent.Text = task.Content;
        }

        public Task Task { get; private set; }

        private void btnTitle_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form is MainForm)
                {
                    form.Show();
                    ((MainForm)form).OpenTask(Task);
                }
            }
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void labContent_Click(object sender, EventArgs e)
        {
            btnTitle_LinkClicked(null, null);
        }
    }
}
