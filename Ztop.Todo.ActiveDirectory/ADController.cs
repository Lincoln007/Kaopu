using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Ztop.Todo.ActiveDirectory
{
    public static partial class ADController
    {
        private static string ADServer { get; set; }
        private static string ADName { get; set; }
        private static string ADPassword { get; set; }
        private static XmlDocument configXml { get; set; }
        private static List<string> IgnoresList { get; set; }
        private static List<string> ManagerList { get; set; }
        private static List<string> AdminList { get; set; }

        static ADController()
        {
            ADServer = System.Configuration.ConfigurationManager.AppSettings["AServer"];
            ADName = System.Configuration.ConfigurationManager.AppSettings["Name"];
            ADPassword = System.Configuration.ConfigurationManager.AppSettings["Password"];
            Init();
        }
        private static void Init()
        {
            configXml = new XmlDocument();
            configXml.Load(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, System.Configuration.ConfigurationManager.AppSettings["IGNORE"]));

            IgnoresList = InitConfig("/Composes/Compose");

            AdminList = InitConfig("/Composes/Administrators/Administrator");

            ManagerList = InitConfig("/Composes/Organizations/Organization");
            
        }

        private static List<string> InitConfig(string SelectString)
        {
            var list = new List<string>();
            var nodes = configXml.SelectNodes(SelectString);
            if (nodes != null)
            {
                for(var i = 0; i < nodes.Count; i++)
                {
                    list.Add(nodes[i].Attributes["Name"].Value);
                }
            }
            return list;
        }

        #region 通用

        private static DirectoryEntry GetDirectoryObject(string Name, string Password)
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
        private static DirectoryEntry GetDirectoryObject()
        {
            return GetDirectoryObject(ADName, ADPassword);
        }
        private static SearchResult SearchOne(string Filter, DirectoryEntry Entry = null)
        {
            if (Entry == null)
            {
                Entry = GetDirectoryObject();
            }
            using (var searcher = new DirectorySearcher(Entry))
            {
                searcher.Filter = Filter;
                searcher.SearchScope = SearchScope.Subtree;
                return searcher.FindOne();
            }
        }
        public static SearchResultCollection SearchAll(string Filter,DirectoryEntry Entry = null)
        {
            if (Entry == null)
            {
                Entry = GetDirectoryObject();
            }
            using (var searcher=new DirectorySearcher(Entry))
            {
                searcher.Filter = Filter;
                searcher.SearchScope = SearchScope.Subtree;
                return searcher.FindAll();
            }
        }
        public static bool TryLogin(string username, string password)
        {
#if DEBUG
            if(username == "liangjun")
            {
                return true;
            }
#endif
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return false;
            }
            try
            {
                var user = GetDirectoryObject(username, password);
                return SearchOne("(&(objectCategory=person)(objectClass=user)(sAMAccountName=" + username + "))", user) != null;
            }
            catch
            {
            }
            return false;
        }
        //通过筛选器获取DirectoryEntry
        private static DirectoryEntry Get(string Filter)
        {
            var searchResult = SearchOne(Filter);
            if (searchResult != null)
            {
                return searchResult.GetDirectoryEntry();
            }
            return null;
        }
        //获取DirectoryEntry中属性propertyName的值【0】
        private static string GetProperty(this DirectoryEntry Entry, string PropertyName)
        {
            if (Entry.Properties.Contains(PropertyName))
            {
                return Entry.Properties[PropertyName][0].ToString();
            }
            else
            {
                return string.Empty;
            }
        }
        public static string GetProperty(this SearchResult result,string PropertyName)
        {
            if (result.Properties.Contains(PropertyName))
            {
                return result.Properties[PropertyName][0].ToString();
            }
            else
            {
                return string.Empty;
            }

        }
        private static string GetDistinguishedName(this SearchResult result)
        {
            return result.GetProperty("distinguishedName");
        }
        private static string GetDistinguishedName(this DirectoryEntry Entry)
        {
            return Entry.GetProperty("distinguishedName");
        }
        private static bool IsIgnore(this string DistinguishedName)
        {
            var str = DistinguishedName.Split(',');
            for(var i = 1; i < str.Count(); i++)
            {
                if (IgnoresList.Contains(str[i].Replace("OU=", "").Replace("CN=", "")))
                {
                    return true;
                }
            }
            return false;
        }
        //属性值中提取group或者用户
        private static List<string> Extract(List<string> Origin, string Category)
        {
            var results = new List<string>();
            foreach (var item in Origin)
            {
                var entry = Get("(&(objectCategory=" + Category + ")(cn=" + item.Extract() + "))");
                if (entry == null)
                {
                    continue;
                }
                results.Add(item.Extract());
            }
            return results;
        }
        //获取DirectoryEntry中属性PropertyName所有的值
        private static List<string> GetAllProperty(DirectoryEntry Entry, string PropertyName)
        {
            var list = new List<string>();
            if (Entry.Properties.Contains(PropertyName))
            {

                foreach (var m in Entry.Properties[PropertyName])
                {
                    list.Add(m.ToString());
                }

            }
            return list;
        }
        private static DirectoryEntries GetChildren(string OU)
        {
            var directoryEntry = GetOrganizationObject(OU);
            if (directoryEntry == null)
            {
                throw new ArgumentException("无法获取DirectoryEntry对象");
            }
            return directoryEntry.Children;
        }
        private static List<string> GetList(string Filter)
        {
            var list = new List<string>();
            var collection = SearchAll(Filter);
            foreach(SearchResult result in collection)
            {
                if (!result.GetDistinguishedName().IsIgnore())
                {
                    list.Add(result.GetProperty("name"));
                }
            }
            return list;
        }

        #endregion

        #region  组和用户之间操作
        public static bool MoveUserToGroup(string sAMAccountName,string NewOrganization)
        {
            var userEntry = GetUserObject(sAMAccountName);
            var orgEntry = GetOrganizationObject(NewOrganization);
            if (userEntry == null || orgEntry == null)
            {
                return false;
            }
            orgEntry = new DirectoryEntry(orgEntry.Path, ADName, ADPassword, AuthenticationTypes.Secure);
            userEntry.MoveTo(orgEntry);
            userEntry.CommitChanges();
            return true;
        }
        public static void AddUserToGroup(string sAMAccountName,string GroupName)
        {
            var distinguishedName = GetUserObject(sAMAccountName).GetDistinguishedName();
            if (string.IsNullOrEmpty(distinguishedName))
            {
                throw new ArgumentException("未找到申请用户信息");
            }
            var group = GroupName.GetGroupObject();
            if (group == null)
            {
                throw new ArgumentException("未找到当前组");
            }
            group.Properties["member"].Add(distinguishedName);
            group.CommitChanges();
            group.Close();
        }

        #endregion

    }

    public enum ADAccountOptions
    {
        UF_TEMP_DUPLICATE_ACCOUNT = 0x0100,
        UF_NORMAL_ACCOUNT = 0x0200,
        UF_INTERDOMAIN_TRUST_ACCOUNT = 0x0800,
        UF_WORKSTATION_TRUST_ACCOUNT = 0x1000,
        UF_SERVER_TRUST_ACCOUNT = 0x2000,
        UF_DONT_EXPIRE_PASSWD = 0x10000,
        UF_SCRIPT = 0x0001,
        UF_ACCOUNTDISABLE = 0x0002,
        UF_HOMEDIR_REQUIRED = 0x0008,
        UF_LOCKOUT = 0x0010,
        UF_PASSWD_NOTREQD = 0x0020,
        UF_PASSWD_CANT_CHANGE = 0x0040,
        UF_ACCOUNT_LOCKOUT = 0X0010,
        UF_ENCRYPTED_TEXT_PASSWORD_ALLOWED = 0X0080,
        UF_EXPIRE_USER_PASSWORD = 0x800000,
    }
}
