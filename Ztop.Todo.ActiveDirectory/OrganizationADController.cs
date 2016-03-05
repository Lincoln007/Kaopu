using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztop.Todo.ActiveDirectory
{
    public static partial class ADController
    {
        private static DirectoryEntry GetOrganizationObject(string OU)
        {
            return Get("(&(OU=" + OU + "))");
        }
        public static List<string> GetOrganizations(string OU)
        {
            var list = new List<string>();
            var childrens = GetChildren(OU);
            foreach (DirectoryEntry entry in childrens)
            {
                var name = entry.GetProperty("name");
                if (!string.IsNullOrEmpty(name))
                {
                    list.Add(name);
                }
            }
            return list;
        }
        public static List<string> GetAllOrganization()
        {
            return Gain("(&(objectCategory=organizationalUnit))").Select(e => e.Key).ToList();
        }
    }
}
