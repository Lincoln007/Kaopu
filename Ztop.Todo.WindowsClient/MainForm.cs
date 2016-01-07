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

            var server = System.Configuration.ConfigurationManager.AppSettings["Server"];
            if (string.IsNullOrEmpty(server))
            {
                throw new Exception("请在app.config里添加正确的Server配置");
            }
            webControl1.Source = new Uri(server);
        }

        protected override void OnLoad(EventArgs e)
        {
        }
    }
}
