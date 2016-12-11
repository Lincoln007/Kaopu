using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztop.Todo.Model;

namespace Ztop.Todo.Manager
{
    public class iPadManager:ManagerBase
    {
        public bool Exist(iPad ipad)
        {
            using (var db = GetDbContext())
            {
                var entry = db.iPads.Where(e => e.Name == ipad.Name || e.SerialNumber == ipad.SerialNumber).FirstOrDefault();
                return entry != null;
            }
        }
        public int Add(iPad ipad,bool edit)
        {
            if (string.IsNullOrEmpty(ipad.Name)||string.IsNullOrEmpty(ipad.SerialNumber))
            {
                throw new ArgumentException("平板名称和序列号不能为空，请核对！");
            }
            if (edit)
            {
                using (var db = GetDbContext())
                {
                    var entry = db.iPads.Find(ipad.ID);
                    if (entry != null)
                    {
                        db.Entry(entry).CurrentValues.SetValues(ipad);
                        db.SaveChanges();
                    }
                }
                return ipad.ID;
            }
            else
            {
                if (Exist(ipad))
                {
                    throw new ArgumentException(string.Format("已经存在名称为{0}或者序列号为：{1}的平板记录", ipad.Name, ipad.SerialNumber));
                }
                using (var db = GetDbContext())
                {
                    db.iPads.Add(ipad);
                    db.SaveChanges();
                    return ipad.ID;
                }
            }

        }

        public List<iPad> Get()
        {
            using (var db = GetDbContext())
            {
                var list = db.iPads.OrderByDescending(e => e.Time).ToList();
                foreach(var item in list)
                {
                    item.Account = db.iPad_Accounts.Find(item.AID);
                    item.Relations = db.Register_iPads.Where(e => e.IID == item.ID).ToList();
                }
                return list;
            }
        }
        /// <summary>
        /// 作用：获取指定类型的平板
        /// 作者：汪建龙
        /// 编写时间：2016年12月9日10:48:09
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<iPad> Get(iPadType type)
        {
            using (var db = GetDbContext())
            {
                var list = db.iPads.Where(e => e.Type == type).OrderByDescending(e => e.ID).ToList();
                foreach(var item in list)
                {
                    item.Account = db.iPad_Accounts.Find(item.AID);
                    item.Relations = db.Register_iPads.Where(e => e.IID == item.ID).ToList();
                }
                return list;
            }
        }
        public iPad Get(int id)
        {
            if (id == 0) return null;
            using (var db = GetDbContext())
            {
                return db.iPads.Find(id);
            }
        }

        public void Delete(int id)
        {
            if (id == 0) return;
            using (var db = GetDbContext())
            {
                var entry = db.iPads.Find(id);
                db.iPads.Remove(entry);
                db.SaveChanges();
            }
        }

        public bool CheckUse(int[] ids)
        {
            if (ids == null) return false;
            using (var db = GetDbContext())
            {
                foreach(var id in ids)
                {
                    var entry = db.iPads.Find(id);
                    if (entry == null||entry.Statue!=iPadStatue.Vacant)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public bool Update(int[] ids,iPadStatue statue)
        {
            if (ids == null) return false;
            using (var db = GetDbContext())
            {
                foreach(var id in ids)
                {
                    var entry = db.iPads.Find(id);
                    if (entry == null)
                    {
                        return false;
                    }
                    entry.Statue = statue;
                    db.SaveChanges();
                }
            }

            return true;
        }
    }
}
