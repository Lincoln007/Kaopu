using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztop.Todo.Model;

namespace Ztop.Todo.Manager
{
    public class iPad_RegisterManager:ManagerBase
    {
        public int Save(iPadRegister register)
        {
            using (var db = GetDbContext())
            {
                var entry = db.iPad_Registers.Find(register.ID);
                if (entry == null)
                {
                    db.iPad_Registers.Add(register);
                }
                else
                {
                    db.Entry(entry).CurrentValues.SetValues(register);
                }
                db.SaveChanges();
                return register.ID;
            }
        }

        /// <summary>
        /// 作用：获取所有借用记录
        /// 作者：汪建龙
        /// 编写时间：2016年12月11日10:55:23
        /// </summary>
        /// <returns></returns>
        public List<iPadRegister> Get()
        {
            using (var db = GetDbContext())
            {
                var list= db.iPad_Registers.ToList();
                
                foreach(var item in list)
                {
                    var relations = db.Register_iPads.Where(e => e.RID == item.ID && e.Relation == Relation.Register_iPad).ToList();
                    foreach(var entry in relations)
                    {
                        entry.iPad = db.iPads.Find(entry.IID);
                    }
                    item.Register_iPads = relations;
                }
                return list;
            }
        }

        public iPadRegister Get(int id)
        {
            if (id == 0) return null;
            using (var db = GetDbContext())
            {
                var entry= db.iPad_Registers.Find(id);
                if (entry != null)
                {
                    var register_iPads = db.Register_iPads.Where(e => e.RID == entry.ID&&e.Relation==Relation.Register_iPad).ToList();
                    foreach(var item in register_iPads)
                    {
                        item.iPad = db.iPads.Find(item.IID);
                    }
                    entry.Register_iPads = register_iPads;
                  
                }
                return entry;
            }
        }

        public bool Delete(int id)
        {
            if (id == 0) return false;
            using (var db = GetDbContext())
            {
                var entry = db.iPad_Registers.Find(id);
                if (entry != null)
                {
                    db.iPad_Registers.Remove(entry);
                    db.SaveChanges();
                }
            }

            return true;
        }
    }
}
