using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztop.Todo.Model;

namespace Ztop.Todo.Common
{
    public static partial class ADController
    {
        private static List<string> GetGroupListBysAMAccountName(this string sAMAccountName)
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

        private static Group GetGroup(this DirectoryEntry Entry)
        {
            return new Group
            {
                Name = Entry.GetProperty("name"),
                Descriptions = Entry.GetProperty("description")
            };
        }

        private static Group GetGroup(this SearchResult Result)
        {
            return new Group
            {
                Name = Result.GetProperty("name"),
                Descriptions = Result.GetProperty("description")
            };
        }
        public static Group GetGroup(string GroupName)
        {
            var Group = GetGroupObject(GroupName);
            return Group.GetGroup();
        }
        public static List<Group> GetGroupList(string sAMAccountName)
        {
            var GroupsNames = sAMAccountName.GetGroupListBysAMAccountName();
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
        public static Dictionary<string,List<string>> Gain()
        {
            var dict = new Dictionary<string, List<string>>();
            var dicto = Gain("(&(objectCategory=organizationalUnit))");
            var dictg = Gain("(&(objectCategory=group))");
            foreach(var item in dicto.Keys)
            {
                if (dict.ContainsKey(item))
                {
                    foreach(var value in dicto[item])
                    {
                        dict[item].Add(value);
                    }
                }
                else
                {
                    dict.Add(item, dicto[item]);
                }
            }

            foreach(var item in dictg.Keys)
            {
                if (dict.ContainsKey(item))
                {
                    foreach(var value in dictg[item])
                    {
                        dict[item].Add(value);
                    }
                }
                else
                {
                    dict.Add(item, dictg[item]);
                }
            }
            return dict;
        }
        public static Dictionary<string,List<string>> Gain(string Filter)
        {
            var dict = new Dictionary<string, List<string>>();
            var collection = Filter.SearchAll();
            foreach(SearchResult result in collection)
            {
                var distingusihedName = result.GetProperty("distinguishedName");
                string[] values = distingusihedName.Split(',');
                var name = result.GetProperty("name");
                if (values.Count() == 3)
                {
                    if (name.IsChinese())
                    {
                        if (dict.ContainsKey(name))
                        {
                            continue;
                        }
                        else
                        {
                            dict.Add(name, new List<string>());
                        }
                    }
                }else if (values.Count() == 4)
                {
                    string parent = values[1].Replace("OU=", "");
                    if (parent.IsChinese())
                    {
                        if (dict.ContainsKey(parent))
                        {
                            dict[parent].Add(name);
                        }
                        else
                        {
                            dict.Add(parent, new List<string> { name });
                        }
                    }
                }
            }
            return dict;
        }

        public static Dictionary<string,List<Group>> GetGroupDict()
        {
            var dict = new Dictionary<string, List<Group>>();
            var collection = "(&(objectCategory=group)(objectClass=group))".SearchAll();
            foreach(SearchResult result in collection)
            {
                var distinguishedName = result.GetDistinguishedName();
                if (!distinguishedName.IsIgnore())
                {
                    var ou = distinguishedName.Extract(1, "OU=");
                    var temp = result.GetGroup();
                    if (dict.ContainsKey(ou))
                    {
                        dict[ou].Add(temp);
                    }
                    else
                    {
                        dict.Add(ou, new List<Group> { temp });
                    }
                }
            }
            return dict;
        }
        public static List<string> GetGroupList()
        {
            return "(&(objectCategory=group)(objectClass=group))".GetList();
        }

        public static Group GetTree()
        {
            var admin = GetDirectoryObject();
            return admin.GetTree();
        }
        public static Group GetTree(this DirectoryEntry Entry)
        {
            var name = Entry.GetProperty("name");
            if (string.IsNullOrEmpty(name) || IgnoresList.Contains(name))
            {
                return null;
            }
            DateTime time;
            var group = new Group
            {
                Name = name,
                Descriptions = Entry.GetProperty("description"),
                CreateTime = DateTime.TryParse(Entry.GetProperty("whenCreated"), out time) ? time : time
            };
            foreach(DirectoryEntry item in Entry.Children)
            {
                var temp = item.GetTree();
                if (temp != null)
                {
                    group.Children.Add(temp);
                }
            }
            return group;
        }

        private static TreeObject GetTreeObject(this DirectoryEntry Entry)
        {
            var tree = new TreeObject();
            tree.label = Entry.GetProperty("name");
            if (string.IsNullOrEmpty(tree.label) || IgnoresList.Contains(tree.label) || "内部用户" == tree.label)
            {
                return null;
            }
            var description = Entry.GetProperty("description");
            if (!string.IsNullOrEmpty(description))
            {
                tree.label += "--" + description;
            }
            foreach(DirectoryEntry item in Entry.Children)
            {
                var temp = item.GetTreeObject();
                if (temp != null)
                {
                    tree.children.Add(temp);
                }
            }
            return tree;
        }
        public static TreeObject GetTreeObject()
        {
            var admin = GetDirectoryObject();
            return admin.GetTreeObject();    
        }
    }
}
