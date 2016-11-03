using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztop.Todo.Model;
using Ztop.Todo.Common;
using System.Web;
using Ztop.Todo.ActiveDirectory;

namespace Ztop.Todo.Manager
{
    public class AuthorizeManager:ManagerBase
    {
        public Authorize Get(int id)
        {
            using (var db = GetDbContext())
            {
                var entry = db.Authorizes.Find(id);
                if (entry != null)
                {
                    var gid = entry.GID.Split(';');
                    entry.Groups = new List<ADGroup>();
                    var a = 0;
                    foreach(var item in gid)
                    {
                        if(int.TryParse(item,out a))
                        {
                            var group = db.AD_Groups.Find(a);
                            if (group != null)
                            {
                                entry.Groups.Add(group);
                            }
                        }
               
                        
                    }
                }
                return entry;
            }
        }
        public Authorize Get(HttpContextBase context,int id=0)
        {
            if (id == 0)
            {
                return new Authorize
                {
                    GroupName = context.Request.Form["GroupName"],
                    Manager = context.Request.Form["Manager"]
                };
            }
            var authorize = Get(id);
            if (authorize == null)
            {
                throw new ArgumentException("未找到权限表");
            }
            authorize.GroupName = context.Request.Form["GroupName"];
            return authorize;
        }
        public void Add(Authorize authorize)
        {
            if (string.IsNullOrEmpty(authorize.GroupName) || string.IsNullOrEmpty(authorize.Manager))
            {
                return;
            }
            var groups = authorize.GroupName.Split(',');
            var gid = new List<int>();
            var user = Core.UserManager.UserGet(authorize.Manager);
            if (user == null)
            {
                throw new ArgumentException("未找到相关管理者用户信息，请核对管理者名称!");
            }
            foreach(var groupname in groups)
            {
                var group = Core.AD_groupManager.Get(groupname);
                if (group != null)
                {
                    gid.Add(group.ID);
                }
            }
            authorize.GID = string.Join(";", gid.ToArray());
            using (var db = GetDbContext())
            {
                var entry = db.Authorizes.Where(e => e.Manager == authorize.Manager).FirstOrDefault();
                if (entry != null)
                {
                    throw new ArgumentException(string.Format("已经授权{0}为管理者，如要增加该管理者的权限，请在权限表中增加{0}的权限",authorize.Manager));
                }
                db.Authorizes.Add(authorize);
                db.SaveChanges();
            }
        }
        public void Edit(Authorize authorize)
        {
            var groups = authorize.GroupName.Split(',');
            var gid = new List<int>();
            foreach (var groupname in groups)
            {
                var group = Core.AD_groupManager.Get(groupname);
                if (group != null)
                {
                    gid.Add(group.ID);
                }
            }
            authorize.GID = string.Join(";", gid.ToArray());
            using (var db = GetDbContext())
            {
                var entry = db.Authorizes.Find(authorize.ID);
                if (entry != null)
                {
                    db.Entry(entry).CurrentValues.SetValues(authorize);
                    db.SaveChanges();
                }
            }
        }
        public List<Authorize> GetList()
        {
            using (var db = GetDbContext())
            {
                var list = db.Authorizes.ToList();
                foreach(var item in list)
                {
                    if (!string.IsNullOrEmpty(item.GID))
                    {
                        var gid = item.GID.Split(';');
                        item.Groups = new List<ADGroup>();
                        var a = 0;
                        foreach(var entry in gid)
                        {
                            if(int.TryParse(entry,out a))
                            {
                                var group = db.AD_Groups.Find(a);
                                if (group != null)
                                {
                                    if (group.OID > 0)
                                    {
                                        group.Parent = db.AD_Groups.Find(group.OID);
                                    }
                                    item.Groups.Add(group);
                                }
                            }
                        
                        }
                    }
                }
                return list;
            }
        }
        /// <summary>
        /// 作用：通过名称 获取该用户管理的权限组
        /// 作者：汪建龙
        /// 编写时间：2016年11月2日09:50:36
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public List<string> GetList(string Name)
        {
            var authorize = GetList().FirstOrDefault(e => e.Manager == Name);
            if (authorize == null||authorize.Groups==null)
            {
                return new List<string>();
            }
            return authorize.Groups.Select(e => e.Name).ToList();
            //var array = authorize.GroupName.Split(',');
            //var list = new List<string>();
            //foreach (var item in array)
            //{
            //    list.Add(item);
            //}
            //return list.OrderBy(e => e).ToList();
        }
        /// <summary>
        /// 作用：通过管理者名称获取授权列表
        /// 作者：汪建龙
        /// 编写时间：2016年11月1日17:49:39
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Authorize Get(string name)
        {
            return GetList().FirstOrDefault(e => e.Manager == name);
        }
        public List<string> GetAllManager()
        {
            return GetList().Select(e => e.Manager).ToList();
        }
        public void Screen(string[] Origin,string sAMAccountName,out List<string> None,out List<string> Have)
        {
            None = new List<string>();
            Have = new List<string>();
            foreach(var item in Origin)
            {
                if (ADController.IsMember(item, sAMAccountName))
                {
                    Have.Add(item);
                }
                else
                {
                    None.Add(item);
                }
            }
        }

        /// <summary>
        /// 作用：删除授权管理记录
        /// 作者：汪建龙
        /// 编写时间：2016年11月3日11:12:19
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            using (var db = GetDbContext())
            {
                var entry = db.Authorizes.Find(id);
                if (entry == null)
                {
                    throw new ArgumentException("未找到相关授权管理记录！");
                }
                db.Authorizes.Remove(entry);
                db.SaveChanges();
            }
        }
    }
}
