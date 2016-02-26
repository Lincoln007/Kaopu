using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Ztop.Todo.Model;

namespace Ztop.Todo.Common
{
    public static class ADController
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

        private static DirectoryEntry GetDirectoryObjects(string Name, string Password)
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
        private static SearchResult SearchOne(string Filter, DirectoryEntry Entry = null)
        {
            if (Entry == null)
            {
                Entry = GetDirectoryObjects();
            }
            using (var searcher = new DirectorySearcher(Entry))
            {
                searcher.Filter = Filter;
                searcher.SearchScope = SearchScope.Subtree;
                return searcher.FindOne();
            }
        }
        public static bool Login(string Name, string Password)
        {
            if (!string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Password))
            {
                var user = GetDirectoryObjects(Name, Password);
                var result = SearchOne("(&(objectCategory=person)(objectClass=user)(sAMAccountName=" + Name + "))", user);
                return result == null ? false : true;
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
        private static string GetProperty(DirectoryEntry Entry, string PropertyName)
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

        #endregion

        #region  用户
        private static DirectoryEntry GetUserObject(string sAMAccountName)
        {
            return Get("(&(objectCategory=person)(objectClass=user)(sAMAccountName=" + sAMAccountName + "))");
        }
        public static AUser GetUser(string sAMAccountName)
        {
            var user = GetUserObject(sAMAccountName);
            if (user == null)
            {
                return new AUser()
                {
                    Name = sAMAccountName,
                    Type = GroupType.Guest
                };
            }
            return new AUser()
            {
                Name = GetProperty(user, "name"),
                Account = GetProperty(user, "sAMAccountName"),
                Group = Extract(GetAllProperty(user, "memberOf"), "group").OrderBy(e => e).ToList()
            };
        }
        public static bool IsAdministrator(AUser auser)
        {
            if (auser.Group == null || auser.Group.Count == 0)
            {
                return false;
            }
            foreach(var item in AdminList)
            {
                if (auser.Group.Contains(item))
                {
                    return true;
                }
            }
            return false;
        }
        public static bool IsManager(AUser auser)
        {
            if (auser.Group == null || auser.Group.Count == 0)
            {
                return false;
            }
            foreach(var item in ManagerList)
            {
                if (auser.Group.Contains(item))
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region 组
        private static List<string> GetGroupListBysAMAccountName(string sAMAccountName)
        {
            var userEntry = GetUserObject(sAMAccountName);
            if (userEntry == null)
            {
                return null;
            }
            return Extract(GetAllProperty(userEntry, "memberOf"), "group");
        }
        private static DirectoryEntry GetGroupObject(string GroupName)
        {
            return Get("(&(objectCategory=group)(objectClass=group)(cn=" + GroupName + "))");
        }

        public static Group GetGroup(string GroupName)
        {
            var Group = GetGroupObject(GroupName);
            return new Group()
            {
                Name = GetProperty(Group, "name"),
                Descriptions = GetProperty(Group, "description")
            };
        }
        public static List<Group> GetGroupList(string sAMAccountName)
        {
            var GroupsNames = GetGroupListBysAMAccountName(sAMAccountName);
            var list = new List<Group>();
            if (GroupsNames != null)
            {
                foreach (var item in GroupsNames)
                {
                    list.Add(GetGroup(item));
                }
            }
            return list.OrderBy(e => e.Name).ToList();
        }
        #endregion

        #region 组织单元

        #endregion

        #region  组和用户之间操作

        #endregion

    }
}
