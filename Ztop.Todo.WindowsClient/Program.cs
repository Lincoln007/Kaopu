using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Ztop.Todo.Common;

namespace Ztop.Todo.WindowsClient
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            string filePath = string.Empty;
            if (args != null && args.Length > 0)
            {
                int count = args.Length;
                for(var i = 0; i < count; i++)
                {
                    if (string.IsNullOrEmpty(filePath))
                    {
                        filePath += args[i].ToString();
                    }
                    else
                    {
                        filePath += " " + args[i].ToString();
                    }
                    
                }
                //filePath = args[0].ToString();
            }   
            MessageBox.Show(filePath);
            bool canCreateNew = false;
            Mutex m = new Mutex(true, "ZTOPTODO", out canCreateNew);
            if (canCreateNew)
            {

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                var login = new LoginForm();
                Application.Run(login);
            }
            else
            {
                TcpClient tcpClient = new TcpClient(IPHelper.GetIPAddress().ToString(), Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["Port"]));
                NetworkStream ns = tcpClient.GetStream();
                //byte[] buffer = System.Text.Encoding.ASCII.GetBytes(filePath);
                byte[] buffer = System.Text.Encoding.Unicode.GetBytes(filePath);
                try
                {
                    lock (ns)
                    {
                        ns.Write(buffer, 0, buffer.Length);
                    }
                }catch(Exception ex)
                {
                    MessageBox.Show("发送文件路径失败："+ex.ToString());
                }
            }
            
        }
    }
}
