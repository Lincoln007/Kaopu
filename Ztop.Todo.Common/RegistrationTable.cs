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
        public static void AddRegistration(string ExePath)
        {
            using (RegistryKey classesroot = Registry.ClassesRoot)
            {
                using (RegistryKey star = classesroot.OpenSubKey("*"))
                {
                    using (RegistryKey Shell = star.OpenSubKey("Shell", true))
                    {
                        using (RegistryKey TODO = Shell.CreateSubKey("TODO"))
                        {

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
