using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztop.Todo.Common
{
    public static class ADController
    {
        private static string ADServer { get; set; }
        private static string ADName { get; set; }
        private static string ADPassword { get; set; }

        static ADController()
        {
            ADServer = System.Configuration.ConfigurationManager.AppSettings["AServer"];
            ADName = System.Configuration.ConfigurationManager.AppSettings["Name"];
            ADPassword = System.Configuration.ConfigurationManager.AppSettings["Password"];
        }
        private static DirectoryEntry GetDirectoryObjects(string Name,string Password)
        {
            try
            {
                return new DirectoryEntry(ADServer, Name, Password, AuthenticationTypes.Secure);
            }
            catch
            {
                return null;
            }
        }
        private static DirectoryEntry GetDirectoryObjects()
        {
            return GetDirectoryObjects(ADName, ADPassword);
        }
        private static SearchResult SearchOne(string Filter,DirectoryEntry Entry = null)
        {
            if (Entry == null)
            {
                Entry = GetDirectoryObjects();   
            }
            using (var searcher=new DirectorySearcher(Entry))
            {
                searcher.Filter = Filter;
                searcher.SearchScope = SearchScope.Subtree;
                return searcher.FindOne();
            }
        }
        public static bool Login(string Name,string Password)
        {
            if (!string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Password))
            {
                var user = GetDirectoryObjects(Name, Password);
                var result = SearchOne("(&(objectCategory=person)(objectClass=user)(sAMAccountName=" + Name + "))", user);
                return result == null ? false : true;
            }
            return false;
        }
    }
}
