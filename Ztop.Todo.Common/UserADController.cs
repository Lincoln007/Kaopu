﻿using System;
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
        private static DirectoryEntry GetUserObject(this string sAMAccountName)
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
            foreach (var item in AdminList)
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
            foreach (var item in ManagerList)
            {
                if (auser.Group.Contains(item))
                {
                    return true;
                }
            }
            return false;
        }
        private static bool IsActive(this DirectoryEntry Entry)
        {
            int iUserAccountControl = Convert.ToInt32(GetProperty(Entry, "userAccountControl"));
            int iUserAccountControl_Disabled = Convert.ToInt32(ADAccountOptions.UF_ACCOUNTDISABLE);
            int iFlagExists = iUserAccountControl & iUserAccountControl_Disabled;
            return iFlagExists > 0 ? false : true;
        }
        public static void ActiveAccount(this string sAMAccountName)
        {
            if (string.IsNullOrEmpty(sAMAccountName))
            {
                return;
            }
            var user = sAMAccountName.GetUserObject();
            user.Properties["userAccountControl"][0] = ADAccountOptions.UF_NORMAL_ACCOUNT;
            user.CommitChanges();
            user.Close();
        }

        public static void DisableAccount(this string sAMAccountName)
        {
            if (string.IsNullOrEmpty(sAMAccountName))
            {
                return;
            }
            var user = sAMAccountName.GetUserObject();
            user.Properties["userAccountControl"][0] = ADAccountOptions.UF_NORMAL_ACCOUNT | ADAccountOptions.UF_DONT_EXPIRE_PASSWD | ADAccountOptions.UF_ACCOUNTDISABLE;
            user.CommitChanges();
            user.Close();
        }
        private static List<AUser> GetUserList(DirectoryEntry Parent, string Key = null)
        {
            var list = new List<AUser>();
            foreach (DirectoryEntry child in Parent.Children)
            {
                var name = GetProperty(child, "name");
                var account = GetProperty(child, "sAMAccountName");
                var flag = child.IsActive();
                if (string.IsNullOrEmpty(Key))
                {
                    list.Add(new AUser
                    {
                        Name = name,
                        Account = account,
                        Group = Extract(GetAllProperty(child, "memberOf"), "group").OrderBy(e => e).ToList(),
                        IsActive = flag
                    });
                }
                else
                {
                    if (name.Contains(Key) || account.Contains(Key))
                    {
                        list.Add(new AUser
                        {
                            Name = name,
                            Account = account,
                            Group = Extract(GetAllProperty(child, "memberOf"), "group").OrderBy(e => e).ToList(),
                            IsActive = flag
                        });
                    }
                }
            }
            return list;
        }
        public static List<string> GetUserList()
        {
            return "(&(objectCategory=person)(objectClass=user))".GetList();
        }
        public static Dictionary<string, List<AUser>> GetUserDict(bool? IsActive, string key = null)
        {
            var dict = new Dictionary<string, List<AUser>>();
            var childrens = GetChildren(System.Configuration.ConfigurationManager.AppSettings["PEOPLE"]);
            foreach (DirectoryEntry child in childrens)
            {
                var name = GetProperty(child, "name");
                if (string.IsNullOrEmpty(name) || dict.ContainsKey(name))
                {
                    continue;
                }
                var query = GetUserList(child, key).AsQueryable();
                if (IsActive.HasValue)
                {
                    query = query.Where(e => e.IsActive == IsActive.Value);
                }
                dict.Add(name, query.ToList());

            }
            return dict;
        }

        public static void CreateUser(string Name, string sAMAccountName, string Organization, string FirstPassword)
        {
            var organizationEntry = Organization.GetOrganizationObject();
            if (organizationEntry == null)
            {
                throw new ArgumentException("未找到创建用户的组织单元");
            }
            var user = organizationEntry.Children.Add("CN=" + Name, "user");
            organizationEntry.Close();
            user.Properties["sAMAccountName"].Value = sAMAccountName;
            user.CommitChanges();
            user.Invoke("SetPassword", new object[] { FirstPassword });
            user.CommitChanges();
            user.Close();
            sAMAccountName.ActiveAccount();
        }
    }
}
