using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ztop.Todo.Common;

namespace Ztop.Todo.WindowsClient
{
    
    public class TODOListener
    {
        public bool IsLive { get; set; }
        private string Name { get; set; }
        private string Password { get; set; }
        private MainForm mainForm { get; set; }
        public TODOListener(string Name,string Password,MainForm form)
        {
            this.mainForm = form;
            this.Name = Name;
            this.Password = Password;
            IsLive = true;
        }

        public void Listen()
        {
            TcpListener tcpListener = new TcpListener(IPHelper.GetIPAddress(), Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["Port"]));
            tcpListener.Start();
            while (IsLive)
            {
                using (TcpClient tcpclient = tcpListener.AcceptTcpClient())
                {
                    NetworkStream ns = tcpclient.GetStream();
                    byte[] buffer = new byte[tcpclient.Available];
                    ns.Read(buffer, 0, buffer.Length);
                    string str = System.Text.Encoding.Unicode.GetString(buffer).Trim();
                    if (System.IO.File.Exists(str))
                    {
                        string uploadPath = string.Empty;
                        try
                        {
                            uploadPath = FTPHelper.UploadFile(str);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("上传失败，错误信息：" + ex.ToString());
                            continue;
                        }
                        MessageBox.Show("成功上传文件到服务器" + str);
                        if (!string.IsNullOrEmpty(uploadPath)&&this.mainForm!=null)
                        {
                            mainForm.ThreadFunction(uploadPath);
                            //MainListener("/task/edit?fileOne="+uploadPath+"&&Name="+Name+"&&Password="+Password);
                        }
                    }
                }
            }
        }


        [STAThread]
        public void MainListener(string UriPath)
        {
            Application.Run(new MainForm2(UriPath));
        }
    }
}
