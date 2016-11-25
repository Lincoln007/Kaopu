using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztop.Todo.Model;

namespace Ztop.Todo.Manager
{
    public class AD_GroupManager:ManagerBase
    {
        /// <summary>
        /// 作用：获取所有域服务器的组织单元和组
        /// 作者：汪建龙
        /// 编写时间：2016-10-31
        /// </summary>
        /// <returns></returns>
        public List<ADGroup> Get()
        {
            using (var db = GetDbContext())
            {
                return db.AD_Groups.ToList();
            }
        }
        /// <summary>
        /// 作用：通过ID获取Ad_group对象
        /// 作者：汪建龙
        /// 编写时间：2016年11月24日09:21:31
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ADGroup Get(int id)
        {
            using (var db = GetDbContext())
            {
                var entry= db.AD_Groups.Find(id);
                if (entry != null)
                {
                    entry.Parent = db.AD_Groups.FirstOrDefault(e => e.OID == entry.OID);
                }
                return entry;
            }
        }

        /// <summary>
        /// 作用：通过名称搜索  可追加类型
        /// 作者：汪建龙
        /// 编写时间：2016年11月1日09:35:54
        /// 修改：汪建龙
        /// 修改时间：2016年11月3日14:45:00
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ADGroup Get(string name,ADType? type=null)
        {
            using (var db = GetDbContext())
            {
                var query = db.AD_Groups.Where(e => e.Name.ToUpper() == name.ToUpper()).AsQueryable();
                if (type.HasValue)
                {
                    query = query.Where(e => e.Type == type.Value);
                }
                return query.FirstOrDefault();
            }
        }

        /// <summary>
        /// 作用：获取域服务器的组织单元
        /// 作者：汪建龙
        /// 编写时间：2016-10-31
        /// </summary>
        /// <returns></returns>
        public List<ADGroup> GetOrganication()
        {
            return Get().Where(e => e.Type == ADType.Organication).ToList();
        }

        /// <summary>
        /// 作用：添加记录
        /// 作者：汪建龙
        /// 编写时间：2016年10月31日16:26:22
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public ADGroup Save(ADGroup group)
        {
            using (var db = GetDbContext())
            {
                db.AD_Groups.Add(group);
                db.SaveChanges();
                return group;
            }
        }

        /// <summary>
        /// 作用：批量增加记录
        /// 作者：汪建龙
        /// 编写时间：2016-10-31
        /// </summary>
        /// <param name="list"></param>
        public void Save(List<ADGroup> list)
        {
            using (var db = GetDbContext())
            {
                db.AD_Groups.AddRange(list);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// 作用：通过OID(上级组ID)获取列表
        /// 作者：汪建龙
        /// 编写时间：2016年10月31日16:23:59
        /// </summary>
        /// <param name="oid"></param>
        /// <returns></returns>
        public List<ADGroup> GetByOID(int oid)
        {
            return Get().Where(e => e.OID == oid).ToList();
        }

        /// <summary>
        /// 作用：更新记录
        /// 作者：汪建龙
        /// 编写时间：2016年10月31日16:33:35
        /// </summary>
        /// <param name="group"></param>
        public void Update(ADGroup group)
        {
            using (var db = GetDbContext())
            {
                var current = db.AD_Groups.Find(group.ID);
                if (current != null)
                {
                    db.Entry(current).CurrentValues.SetValues(group);
                    db.SaveChanges();
                }
            }
        }

        /// <summary>
        /// 作用：删除记录  批量
        /// 作者：汪建龙
        /// 编写时间：2016年10月31日16:37:42
        /// </summary>
        /// <param name="list"></param>
        public void Remove(List<ADGroup> list)
        {
            using (var db = GetDbContext())
            {
                var remove = new List<ADGroup>();
                foreach(var item in list)
                {
                    var current = db.AD_Groups.Find(item.ID);
                    if (current != null)
                    {
                        remove.Add(current);
                    }
                }
                db.AD_Groups.RemoveRange(remove);
                db.SaveChanges();
            }
        }
        /// <summary>
        /// 作用：通过组织单元名称获得该组织单元下的组
        /// 作者：汪建龙
        /// 编写时间：2016年11月1日14:45:36
        /// </summary>
        /// <param name="organicationName"></param>
        /// <returns></returns>
        public List<ADGroup> GetGroupByOrganication(string organicationName)
        {
            var organication = Get(organicationName);
            if (organicationName == null)
            {
                return null;
            }
            return GetByOID(organication.ID);
        }
        /// <summary>
        /// 作用：获取所有子组
        /// 作者：汪建龙
        /// 编写时间：2016年11月2日09:43:25
        /// </summary>
        /// <returns></returns>
        public List<ADGroup> GetAllGroup()
        {
            return Get().Where(e => e.Type == ADType.Group).ToList();
        }

        /// <summary>
        /// 作用：通过ID列表，获取AD_group列表
        /// 作者：汪建龙
        /// 编写时间：2016年11月24日09:22:39
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public List<ADGroup> Get(List<int> ids)
        {
            var list = new List<ADGroup>();
            foreach(var item in ids)
            {
                list.Add(Get(item));
            }
            return list;
        }

        
    }
}
