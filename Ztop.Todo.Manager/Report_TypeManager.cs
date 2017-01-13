using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztop.Todo.Model;

namespace Ztop.Todo.Manager
{
    public class Report_TypeManager:ManagerBase
    {
        /// <summary>
        /// 作用：获取一级类
        /// 作者：汪建龙
        /// 编写时间：2017年1月12日17:28:05
        /// </summary>
        /// <returns></returns>
        public List<ReportType> Get()
        {
            using (var db = GetDbContext())
            {
                return db.ReportTypes.Where(e=>e.RID==0&&e.Delete==false).ToList();
            }
        }
        /// <summary>
        /// 作用：添加报销类型
        /// 作者：汪建龙
        /// 编写时间：2017年1月12日18:33:44
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public int Add(ReportType type)
        {
            using (var db = GetDbContext())
            {
                db.ReportTypes.Add(type);
                db.SaveChanges();
                return type.ID;
            }
        }
        /// <summary>
        /// 作用：通过ID获取
        /// 作者：汪建龙
        /// 编写时间：2017年1月12日19:02:24
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ReportType Get(int id)
        {
            if (id == 0)
            {
                return null;
            }
            using (var db = GetDbContext())
            {
                var entry= db.ReportTypes.Find(id);
                if (entry != null)
                {
                    entry.Children = db.ReportTypes.Where(e => e.RID == entry.ID).ToList();
                }
                return entry;
            }
        }
        /// <summary>
        /// 作用：获取二级类别
        /// 作者：汪建龙
        /// 编写时间：2017年1月12日19:11:20
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public List<ReportType> GetChildren(List<ReportType> list)
        {
            if (list == null || list.Count == 0)
            {
                return list;
            }
            using (var db = GetDbContext())
            {
                foreach(var item in list)
                {
                    item.Children = db.ReportTypes.Where(e => e.RID == item.ID&&e.Delete==false).ToList();
                }
                return list;
            }
        }
        /// <summary>
        /// 作用：编辑
        /// 作者：汪建龙
        /// 编写时间：2017年1月12日19:22:56
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool Edit(ReportType type)
        {
            if (type == null || type.ID == 0)
            {
                return false;
            }
            using (var db = GetDbContext())
            {
                var entry = db.ReportTypes.Find(type.ID);
                if (entry == null)
                {
                    return false;
                }

                entry.Name = type.Name;
                db.SaveChanges();
                return true;
            }
        }

        /// <summary>
        /// 作用：删除报销类
        /// 作者：汪建龙
        /// 编写时间：2017年1月12日20:26:29
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            if (id == 0)
            {
                return false;
            }

            using (var db = GetDbContext())
            {
                var entry = db.ReportTypes.Find(id);
                if (entry == null)
                {
                    return false;
                }
                entry.Delete = true;
                db.SaveChanges();
                var list = db.ReportTypes.Where(e => e.RID == entry.ID && e.Delete == false).ToList();
                if (list != null && list.Count > 0)
                {
                    foreach(var item in list)
                    {
                        item.Delete = true;
                    }
                    db.SaveChanges();
                }
                return true;
            }
        }
    }
}
