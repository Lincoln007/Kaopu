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
    public partial class LoginForm : Form
    {
        private string FileOne { get; set; }
        public LoginForm(string FilePath)
        {
            InitializeComponent();
            this.FileOne = FilePath;
        }
        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(NameText.Text) && !string.IsNullOrEmpty(PasswordText.Text))
            {
                if(Ztop.Todo.Common.ADController.Login(NameText.Text, PasswordText.Text))
                {
                    var form = new MainForm(NameText.Text,PasswordText.Text,this.FileOne);
                    form.login = this;
                    form.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("用户名或者密码不正确");
                }
            }
            else
            {
                MessageBox.Show("请输入用户名或者密码");
            }
        }

        private void MCancelButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void PasswordText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.LoginButton_Click(sender, e);
            }
        }
    }
}
