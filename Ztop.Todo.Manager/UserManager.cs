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
        private string _cacheKey = "users";

        private List<User> GetList()
        {
            if (!Cache.Exists(_cacheKey))
            {
                using (var db = GetDbContext())
                {
                    var list = db.Users.ToList();
                    foreach (var user in list)
                    {
                        Cache.HSet(_cacheKey, user.ID.ToString(), user);
                    }
                }
            }
            return Cache.HGetAll<User>(_cacheKey);
        }

        public User GetUser(int id)
        {
            if (!Cache.Exists(_cacheKey))
            {
                GetList();
            }
            return Cache.HGet<User>(_cacheKey, id.ToString());
        }

        public User GetUser(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return null;
            }
            return GetList().FirstOrDefault(e => e.Username == username);
        }

        /// <summary>
        /// 如果更新密码，请在调用方法前【不要】对password属性进行md5
        /// </summary>
        /// <param name="user"></param>
        public void Save(User user)
        {
            using (var db = GetDbContext())
            {
                if (user.ID > 0)
                {
                    var entity = db.Users.FirstOrDefault(e => e.ID == user.ID);
                    if (entity != null)
                    {
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
                    db.Users.Add(user);
                }
                db.SaveChanges();
            }
            Cache.HSet(_cacheKey, user.ID.ToString(), user);
        }

        public void UpdateLogin(User user)
        {
            using(var db = GetDbContext())
            {
                var entity = db.Users.FirstOrDefault(e => e.ID == user.ID);
                entity.LoginTimes++;
                entity.LastLoginTime = DateTime.Now;
                db.SaveChanges();
                Cache.HSet(_cacheKey, user.ID.ToString(), user);
            }
        }
    }
}
