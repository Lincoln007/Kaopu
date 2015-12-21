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
                        Cache.HSet("users", user.ID.ToString(), user);
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

        public User GetUser(string username, string password)
        {
            if (string.IsNullOrEmpty(username))
            {
                return null;
            }
            var entity = GetList().FirstOrDefault(e => e.Username.ToLower() == username.ToLower());
            if (entity == null)
            {
                throw new ArgumentException("不存在该用户名");
            }
            if (entity.Password != password.MD5())
            {
                throw new ArgumentException("密码不正确");
            }
            return entity;
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
                    if(entity!=null)
                    {
                        if(!string.IsNullOrEmpty(user.Password))
                        {
                            entity.Password = user.Password.MD5();   
                        }
                        entity.RealName = user.RealName;
                        entity.LoginTimes++;
                        entity.LastLoginTime = DateTime.Now;
                    }
                }
                else
                {
                    var entity = db.Users.FirstOrDefault(e => e.Username.ToLower() == user.Username.ToLower());
                    if(entity !=null)
                    {
                        throw new ArgumentException("用户名已被使用");
                    }
                    db.Users.Add(user);
                }
                db.SaveChanges();
            }
            Cache.HSet(_cacheKey, user.ID.ToString(), user);
        }
    }
}
