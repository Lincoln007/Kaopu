using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztop.Todo.Common
{
    public static class RegistrationTable
    {
        public static void Registrate()
        {
            string EXEPATH = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, System.Configuration.ConfigurationManager.AppSettings["EXENAME"]);
            EXEPATH.AddRegistration();
        }
        public static void AddRegistration(this string ExePath)
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
                                command.SetValue(null, string.Format("{0} %1", ExePath));
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
