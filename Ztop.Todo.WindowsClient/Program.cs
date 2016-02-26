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
            }
            if (string.IsNullOrEmpty(filePath))
            {
                RegistrationTable.Registrate();
            }   
            bool canCreateNew = false;
            Mutex m = new Mutex(true, "ZTOPTODO", out canCreateNew);
            if (canCreateNew)
            {

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                
                var login = new LoginForm(filePath);
                Application.Run(login);
            }
            else
            {
                TCPHelper.TCPSend(filePath);
            }
            
        }
    }
}
