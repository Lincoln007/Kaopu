using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Ztop.Todo.Common;
using Ztop.Todo.Model;

namespace Ztop.Todo.WindowsClient
{
    static class Program
    {
        const int WM_COPYDATA = 0x004A;
        //private static Mutex _m;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            var filePath = string.Join(" ", args ?? new string[] { });
            if (WJLHasInstance())
            {
                
                if (!string.IsNullOrEmpty(filePath))
                {
                    filePath = System.IO.Path.GetFullPath(filePath);
                    if (!System.IO.File.Exists(filePath))
                    {
                        MessageBox.Show(string.Format("无法识别的文件路径，当前识别路径：{0}", filePath));
                        return;
                    }
                    #region  发送信息
                    Thread.Sleep(500);
                    SendMessage(filePath);
                    #endregion
                }
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new LoginForm(filePath));
            }
          
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = (Exception)e.ExceptionObject;
            LogHelper.WriteLog(ex);
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

        private static bool WJLHasInstance()
        {
            #region  方式一

            //bool isAppRunning = false;
            //System.Threading.Mutex mutex = new System.Threading.Mutex(true, System.Diagnostics.Process.GetCurrentProcess().ProcessName, out isAppRunning);
            //return !isAppRunning;

            #endregion

            #region  方式二
            bool canCreateNew = false;
            var _m = new Mutex(true, "ZTOP-TODO", out canCreateNew);
            return !canCreateNew;
            #endregion
        }

        private static void RegisterApplication()
        {
            var exePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AppDomain.CurrentDomain.FriendlyName);
            #region  注册鼠标右键
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
            #endregion

            #region  注册开机启动
            //using (RegistryKey localMachine = Registry.LocalMachine)
            //{
            //    RegistryKey reg = localMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
            //    if (reg == null)
            //    {
            //        reg = localMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
            //    }
            //    reg.SetValue("ZTOPTODO", exePath);
            //    reg.Close();
            //    localMachine.Close();
            //}
            #endregion
        }

        [DllImport("User32.dll",EntryPoint ="SendMessage")]
        private static extern int SendMessage(int hWnd, int Msg, int wParam, ref COPYDATASTRUCT IParam);
        [DllImport("User32.dll",EntryPoint ="FindWindow")]
        private static extern int FindWindow(string IpClassName, string IpWindowName);

        private static void SendMessage(string filePath)
        {
            int WINDOW_HANDLER = FindWindow(null, "智拓TODO");
            if (WINDOW_HANDLER != 0)
            {
                byte[] sarr = System.Text.Encoding.Default.GetBytes(filePath);
                int len = sarr.Length;
                COPYDATASTRUCT cds;
                cds.dwData = (IntPtr)100;
                cds.IpData = filePath;
                cds.cbData = len + 1;
                SendMessage(WINDOW_HANDLER, WM_COPYDATA, 0, ref cds);
            }
        }
    }
}
