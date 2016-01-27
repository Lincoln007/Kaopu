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
                    var form = new MainForm(NameText.Text,PasswordText.Text);
                    form.login = this;
                    form.Show();
                    this.Hide();
                    //if (Form != null)
                    //{
                    //    Form.Enabled = true;
                    //    Form.Name = NameText.Text;
                    //    Form.Password = PasswordText.Text;
                    //    MessageBox.Show("成功登录");
                    //    this.Close();
                    //}
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
    }
}
