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
                    foreach (var item in gid)
                    {
                        if (int.TryParse(item, out a))
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
        /// <summary>
        /// 作用：更新授权管理表
        /// 作者：汪建龙
        /// 编写时间：2016年11月20日12:07:56
        /// </summary>
        /// <param name="List"></param>
        /// <param name="uid"></param>
        public void UpdateFasts(List<Authorize_Fast> List,int uid)
        {
            using (var db = GetDbContext())
            {
                var old = db.Authorize_Fasts.Where(e => e.UID == uid).ToList();
                var add = new List<Authorize_Fast>();
                foreach (var item in List)
                {
                    var entry = old.FirstOrDefault(e => e.GID == item.GID);
                    if (entry != null)//已经存在
                    {
                        old.Remove(entry);
                    }
                    else//不存在
                    {
                        add.Add(item);
                    }
                }
                if (old.Count > 0)
                {
                    db.Authorize_Fasts.RemoveRange(old);
                    db.SaveChanges();
                }

                if (add.Count > 0)
                {
                    db.Authorize_Fasts.AddRange(add);
                    db.SaveChanges();
                }

            }
        }
        /// <summary>
        /// 作用：新建权限管理表
        /// 作者：汪建龙
        /// 编写时间：2016年11月20日10:36:24
        /// </summary>
        /// <param name="authorize"></param>
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
            var fasts = new List<Authorize_Fast>();
            foreach(var groupname in groups)
            {
                var group = Core.AD_groupManager.Get(groupname);
                if (group != null)
                {
                    fasts.Add(new Authorize_Fast { UID = user.ID, GID = group.ID });
                    gid.Add(group.ID);
                }
            }
            authorize.GID = string.Join(";", gid.ToArray());
            UpdateFasts(fasts, user.ID);
        }
        /// <summary>
        /// 作用：编辑权限表
        /// 作者：汪建龙
        /// 编写时间：2016年11月20日10:36:49
        /// </summary>
        /// <param name="authorize"></param>
        public void Edit(string groupName,int uid)
        {
            var groups = groupName.Split(',');
            var gid = new List<int>();
            var fasts = new List<Authorize_Fast>();
            foreach (var groupname in groups)
            {
                var group = Core.AD_groupManager.Get(groupname);
                if (group != null)
                {
                    fasts.Add(new Authorize_Fast { GID = group.ID, UID = uid });
                    gid.Add(group.ID);
                }
            }
            //authorize.GID = string.Join(";", gid.ToArray());
            UpdateFasts(fasts, uid);
        }
        /// <summary>
        /// 作用：通过用户名获取管理权限组列表
        /// 作者：汪建龙
        /// 编写时间：2016年11月20日12:19:14
        /// 修改时间：2016年11月24日09:38:35
        /// </summary>
        /// <param name="realname"></param>
        /// <returns></returns>
        public List<FastGroupUserView> GetFGUV(string realname)
        {
            using (var db = GetDbContext())
            {
                var result= db.FastGroupUserViews.Where(e => e.RealName.ToUpper() == realname.ToUpper()).ToList();
                foreach(var item in result)
                {
                    item.ADGroup = db.AD_Groups.FirstOrDefault(e => e.ID == item.GID);
                    item.Parent = db.AD_Groups.FirstOrDefault(e => e.ID == item.OID);
                    item.User = db.Users.FirstOrDefault(e => e.ID == item.UID);
                }
                return result;
            }
        }

        public List<FastGroupUserView> RichFGUV(List<FastGroupUserView> list)
        {
            if (list == null || list.Count == 0)
            {
                return list;
            }
            using (var db = GetDbContext())
            {
                foreach(var item in list)
                {
                    item.ADGroup = db.AD_Groups.FirstOrDefault(e => e.ID == item.GID);
                    item.Parent = db.AD_Groups.FirstOrDefault(e => e.ID == item.OID);
                    item.User = db.Users.FirstOrDefault(e => e.ID == item.UID);
                }
                return list;
            }
        }

        public List<FastGroupUserView> GetFGUV2(string realName)
        {
            using (var db = GetDbContext())
            {
                var result = db.FastGroupUserViews.Where(e => e.RealName.ToUpper() == realName.ToUpper()).ToList();
                return result;
            }
        }
        /// <summary>
        /// 作用：通过用户ID获取列表
        /// 作者：汪建龙
        /// 编写时间：2016年11月20日13:35:34
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public List<FastGroupUserView> GetFGUV(int uid)
        {
            using (var db = GetDbContext())
            {
                return db.FastGroupUserViews.Where(e => e.UID == uid).ToList();
            }
        }
        /// <summary>
        /// 作用：获取所有权限管理者以及管理者管理的权限组
        /// 作者：汪建龙
        /// 编写时间：2016年11月20日13:43:14
        /// </summary>
        /// <returns></returns>
        public Dictionary<string,List<string>> GetAuthorizeFasts()
        {
            using (var db = GetDbContext())
            {
                var dict= db.FastGroupUserViews.GroupBy(e => e.RealName).ToDictionary(e => e.Key, e => e.Select(k => k.Name).ToList());
                return dict;
            }
        }


        public List<Authorize> GetList()
        {
            using (var db = GetDbContext())
            {
                var list = db.Authorizes.ToList();
                foreach (var item in list)
                {
                    if (!string.IsNullOrEmpty(item.GID))
                    {
                        var gid = item.GID.Split(';');
                        item.Groups = new List<ADGroup>();
                        var a = 0;
                        foreach (var entry in gid)
                        {
                            if (int.TryParse(entry, out a))
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
        /// 作用：通过真实名 获取该用户管理的权限组
        /// 作者：汪建龙
        /// 编写时间：2016年11月2日09:50:36
        /// 修改时间：2016年11月20日13:29:34
        /// </summary>
        /// <param name="realname">真实名称</param>
        /// <returns></returns>
        public List<string> GetList(string realname)
        {
            var fasts = GetFGUV(realname);
            return fasts.Select(e => e.Name).ToList();
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
        /// <summary>
        /// 作用：获取所有权限管理者
        /// 作者：汪建龙
        /// 编写时间：2016年11月20日13:42:25
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllManager()
        {
            return GetAuthorizeFasts().Select(e => e.Key).ToList();
            //return GetList().Select(e => e.Manager).ToList();
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
        /// <summary>
        /// 作用：通过真实用户名删除授权管理表
        /// 作者：汪建龙
        /// 编写时间：2016年11月20日12:45:03
        /// </summary>
        /// <param name="realname"></param>
        public void Delete(string realname)
        {
            var user = Core.UserManager.UserGet(realname);
            if (user == null)
            {
                throw new ArgumentException("未找到相关用户信息");
            }
            using (var db = GetDbContext())
            {
                var removes = db.Authorize_Fasts.Where(e => e.UID == user.ID).ToList();
                if (removes.Count > 0)
                {
                    db.Authorize_Fasts.RemoveRange(removes);
                    db.SaveChanges();
                }
            }
        }
    }
}
