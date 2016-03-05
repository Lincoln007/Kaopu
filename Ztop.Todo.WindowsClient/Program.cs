using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
            if (HasInstance())
            {
                if(args!=null && args.Length>0)
                {
                    var filePath = string.Join(" ", args ?? new string[] { });
                    TCPHelper.TCPSend(filePath);
                }
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                try
                {
                    RegisterApplication();
                }
                catch { }

                var token = LoginHelper.GetToken();
                if (!string.IsNullOrEmpty(token))
                {
                    Application.Run(new MainForm());
                }
                else
                {
                    Application.Run(new LoginForm());
                }
            }
        }

        private static bool HasInstance()
        {
            var hasStarted = 0;
            Process proc = Process.GetCurrentProcess();
            foreach (var p in Process.GetProcesses())
            {
                if (p.ProcessName == proc.ProcessName)
                {
                    hasStarted++;
                }
            }
            return hasStarted > 1;
        }

        private static void RegisterApplication()
        {
            using (RegistryKey classesroot = Registry.ClassesRoot)
            {
                using (RegistryKey star = classesroot.OpenSubKey("*"))
                {
                    using (RegistryKey Shell = star.OpenSubKey("Shell", true))
                    {
                        using (RegistryKey TODO = Shell.CreateSubKey("TODO"))
                        {
                            using (RegistryKey command = TODO.CreateSubKey("Command"))
                            {

                                var exePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AppDomain.CurrentDomain.FriendlyName);
                                command.SetValue(null, string.Format("{0} %1", exePath));
                                command.Close();
                            }
                            TODO.Close();
                        }
                        Shell.Close();
                    }
                    star.Close();
                }
                classesroot.Close();
            }
        }
    }
}
