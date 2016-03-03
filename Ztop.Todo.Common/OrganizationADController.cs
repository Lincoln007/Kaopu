using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztop.Todo.Common
{
    public static partial class ADController
    {
        private static DirectoryEntry GetOrganizationObject(this string OU)
        {
            return Get("(&(OU=" + OU + "))");
        }
        public static List<string> GetOrganizations(this string OU)
        {
            var list = new List<string>();
            var childrens = OU.GetChildren();
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
