using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztop.Todo.Model;
using Ztop.Todo.Common;

namespace Ztop.Todo.Manager
{
    public class UserManager : ManagerBase
    {
        private string _userCacheKey = "users";
        private string _groupCacheKey = "user_groups";

        public List<User> GetAllUsers()
        {
            if (!Cache.Exists(_userCacheKey))
            {
                using (var db = GetDbContext())
                {
                    var list = db.Users.ToList();
                    foreach (var user in list)
                    {
                        Cache.HSet(_userCacheKey, user.ID.ToString(), user);
                    }
                }
            }
            return Cache.HGetAll<User>(_userCacheKey);
        }

        public User GetUser(int id)
        {
            if (!Cache.Exists(_userCacheKey))
            {
                GetAllUsers();
            }
            return Cache.HGet<User>(_userCacheKey, id.ToString());
        }

        public User GetUser(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return null;
            }
            return GetAllUsers().FirstOrDefault(e => e.Username == username);
        }

        /// <summary>
        /// 如果更新密码，请在调用方法前【不要】对password属性进行md5
        /// </summary>
        /// <param name="user"></param>
        public void Save(User user)
        {
            using (var db = GetDbContext())
            {
                var group = GetOrSetUserGroup(user.GroupName) ?? new UserGroup();
                if (user.ID > 0)
                {
                    var entity = db.Users.FirstOrDefault(e => e.ID == user.ID);
                    if (entity != null)
                    {
                        entity.GroupID = group.ID;
                        entity.RealName = user.RealName;
                    }
                }
                else
                {
                    var entity = db.Users.FirstOrDefault(e => e.Username == user.Username);
                    if (entity != null)
                    {
                        throw new ArgumentException("用户名已被使用");
                    }
                    user.GroupID = group.ID;
                    db.Users.Add(user);
                }
                db.SaveChanges();
            }
            Cache.HSet(_userCacheKey, user.ID.ToString(), user);
        }

        private UserGroup GetOrSetUserGroup(string groupName)
        {
            if (string.IsNullOrEmpty(groupName)) return null;
            var group = GetUserGroup(groupName);
            if(group == null)
            {
                using (var db = GetDbContext())
                {
                    group = new UserGroup { Name = groupName };
                    db.UserGroups.Add(group);
                    db.SaveChanges();
                }
            }
            return group;
        }

        public List<UserGroup> GetUserGroups()
        {
            return Cache.GetOrSet(_groupCacheKey, () =>
            {
                using (var db = GetDbContext())
                {
                    return db.UserGroups.ToList();
                }
            });
        }

        public UserGroup GetUserGroup(int id)
        {
            return GetUserGroups().FirstOrDefault(e => e.ID == id);
        }

        public UserGroup GetUserGroup(string name)
        {
            return GetUserGroups().FirstOrDefault(e => e.Name == name);
        }

        public void UpdateLogin(User user)
        {
            using (var db = GetDbContext())
            {
                var entity = db.Users.FirstOrDefault(e => e.ID == user.ID);
                entity.LoginTimes++;
                entity.LastLoginTime = DateTime.Now;
                db.SaveChanges();
                Cache.HSet(_userCacheKey, user.ID.ToString(), user);
            }
        }
    }
}
