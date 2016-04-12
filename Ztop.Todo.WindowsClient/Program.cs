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
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            var filePath = string.Join(" ", args ?? new string[] { });
            if (HasInstance())
            {
                if (!string.IsNullOrEmpty(filePath))
                {
                    filePath = System.IO.Path.GetFullPath(filePath);
                    if (!System.IO.File.Exists(filePath))
                    {
                        MessageBox.Show(string.Format("无法识别的文件路径，当前识别路径：{0}", filePath));
                        return;
                    }
                    try
                    {
                        filePath = FTPHelper.UploadFile(filePath);
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(string.Format("上传文件失败！请确保该文件是否被占用，错误信息：{0}", ex.ToString()));
                        return;
                    }
                    
                    try
                    {
                       
                        TCPHelper.TCPSend(filePath);
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(string.Format("发送文件信息失败，错误信息：{0}",ex.ToString()));
                        return;
                    }
                    //MessageBox.Show(string.Format("成功发送文件路径：{0}", filePath));
                  
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
                catch(Exception ex)
                {
                    MessageBox.Show(string.Format("注册右键附件上传失败，请确保以管理员运行本程序，错误信息：{0}", ex.ToString()) );
                }
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
            bool canCreateNew = false;
            var m = new Mutex(true, "ZTOPTODO", out canCreateNew);
            return canCreateNew;
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
    }
}
