using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Ztop.Todo.ActiveDirectory;
using Ztop.Todo.Model;

namespace Ztop.Todo.WindowsClient
{
    public partial class LoginForm : Form
    {
        private string _uploadFile { get; set; }
        private Dictionary<string, UserInfo> Users { get; set; }
        private List<UserInfo> List { get; set; }
        private string RememberFile { get; set; }
        public LoginForm(string uploadFile)
        {
            InitializeComponent();
            _uploadFile = uploadFile;
            this.RememberFile = System.Configuration.ConfigurationManager.AppSettings["REME"];
            this.Users = new Dictionary<string, UserInfo>();
            this.List = new List<UserInfo>();
        }
        public LoginForm()
        {
            InitializeComponent();
        }
        private void LoginButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUsername.Text))
            {
                txtUsername.Focus();
                return;
            }
#if DEBUG
#else
            if (string.IsNullOrEmpty(txtPassword.Text))
            {
                txtPassword.Focus();
                return;
            }
#endif

            this.btnLogin.Text = "正在登陆";
            this.btnLogin.Enabled = false;

            new Thread(() =>
            {
                var token = LoginHelper.Login(txtUsername.Text, txtPassword.Text);
                btnLogin.BeginInvoke(new Action(() =>
                {
                    if (string.IsNullOrEmpty(token))
                    {
                        btnLogin.Enabled = true;
                        btnLogin.Text = "登录";
                        MessageBox.Show("用户名或者密码不正确");
                        return;
                    }

                    if (cbxAutoLogin.Checked)
                    {
                        LoginHelper.Remeber(token);
                    }
                    MShow();
                    btnLogin.Text = "登陆";
                    btnLogin.Enabled = true;
                }));
            }).Start();
        }
        private void Remember(UserInfo userInfo)
        {
            if (Users != null)
            {
                if (Users.ContainsKey(userInfo.Name))
                {
                    Users[userInfo.Name].Password = userInfo.Password;
                }
                else
                {
                    Users.Add(userInfo.Name, userInfo);
                }
                using (var fs = new FileStream(this.RememberFile, FileMode.OpenOrCreate))
                {
                    var bf = new BinaryFormatter();
                    bf.Serialize(fs, Users);
                    fs.Close();
                }
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
        private void MShow()
        {
            var mainForm = new MainForm(_uploadFile);
            mainForm.Show(this);
            this.Hide();
        }
        private void Init()
        {
            var token = LoginHelper.GetToken();
            if (!string.IsNullOrEmpty(token))
            {
                MShow();
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }

        private void LoginForm_Activated(object sender, EventArgs e)
        {
            Init();
        }
    }
}
