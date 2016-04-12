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
        private MainForm mainForm { get; set; }
        public TODOListener(MainForm form)
        {
            this.mainForm = form;
            IsLive = true;
        }

        public void Listen()
        {
            TcpListener tcpListener = new TcpListener(IPHelper.GetIPAddress(), Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["Port"]));
            tcpListener.Start();
            while (IsLive)
            {
                string str = TCPHelper.TCPRecevier(tcpListener);
                if (str == System.Configuration.ConfigurationManager.AppSettings["TCPSTOP"])
                {
                    tcpListener.Stop();
                    tcpListener.Server.Close();
                    IsLive = false;
                    continue;
                }
                if (!string.IsNullOrEmpty(str))
                {
                    if (this.mainForm != null)
                    {
                        mainForm.ThreadFunction(str);
                    }
                }
            }

            
        }
    }
}
