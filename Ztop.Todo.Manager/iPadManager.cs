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
                return db.iPads.OrderByDescending(e => e.Time).ToList();
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
    }
}
