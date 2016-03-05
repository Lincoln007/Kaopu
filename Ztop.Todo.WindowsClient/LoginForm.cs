using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;
using Ztop.Todo.ActiveDirectory;
using Ztop.Todo.Model;

namespace Ztop.Todo.WindowsClient
{
    public partial class LoginForm : Form
    {
        private string FileOne { get; set; }
        private Dictionary<string,UserInfo> Users { get; set; }
        private List<UserInfo> List { get; set; }
        private string RememberFile { get; set; }
        public LoginForm(string FilePath)
        {
            InitializeComponent();
            this.FileOne = FilePath;
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
            this.LoginButton.Text = "正在登陆";
            this.LoginButton.Enabled = false;
            if (!string.IsNullOrEmpty(comboBox1.Text) && !string.IsNullOrEmpty(PasswordText.Text))
            {
                if(ADController.TryLogin(comboBox1.Text, PasswordText.Text))
                {
                    var userinfo = new UserInfo { Name = comboBox1.Text, Password = PasswordText.Text };
                    if (!RememberChecked.Checked)
                    {
                        userinfo.Password = "";
                    }
                    Remember(userinfo);
                    var form = new MainForm(comboBox1.Text,PasswordText.Text,this.FileOne);
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
            this.LoginButton.Text = "登陆";
            this.LoginButton.Enabled = true;
        }

        private void Remember(UserInfo userInfo)
        {
            if (Users != null )
            {
                if (Users.ContainsKey(userInfo.Name))
                {
                    Users[userInfo.Name].Password = userInfo.Password;
                }
                else
                {
                    Users.Add(userInfo.Name, userInfo);
                }
                using (var fs=new FileStream(this.RememberFile, FileMode.OpenOrCreate))
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

        private void LoginForm_Load(object sender, EventArgs e)
        {
            var fs = new FileStream(this.RememberFile, FileMode.OpenOrCreate);
            if (fs.Length > 0)
            {
                var bf = new BinaryFormatter();
                Users = bf.Deserialize(fs) as Dictionary<string, UserInfo>;
                foreach(var user in Users.Values)
                {
                    comboBox1.Items.Add(user.Name);
                    List.Add(user);
                }
                if (comboBox1.Text != "")
                {
                    if (Users.ContainsKey(comboBox1.Text))
                    {
                        PasswordText.Text = Users[comboBox1.Text].Password;
                        RememberChecked.Checked = true;
                    }
                }
            }
            fs.Close();
            comboBox1.DataSource = List;
            comboBox1.DisplayMember = "Name";
        }

        private void Changed()
        {
            if (Users != null)
            {
                string Name = comboBox1.Text;
                if (Users.ContainsKey(Name))
                {
                    PasswordText.Text = Users[Name].Password;
                    RememberChecked.Checked = string.IsNullOrEmpty(PasswordText.Text) ? false : true;
                }
                else
                {
                    PasswordText.Text = "";
                    RememberChecked.Checked = false;
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Changed();
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            Changed();
        }
    }
}
