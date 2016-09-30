using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztop.Todo.Model;

namespace Ztop.Todo.Manager
{
    public class Register_iPadManager:ManagerBase
    {

        public void Add(List<Register_iPad> list,int rid,Relation relation)
        {
            using (var db = GetDbContext())
            {
                var old = db.Register_iPads.Where(e => e.RID == rid && e.Relation == relation).ToList();
                if (old != null && old.Count > 0)
                {
                    if (relation != Relation.Invoice_iPad)
                    {
                        //如果之前关联了平板为借用装填，现设置平板未空置状态
                        foreach (var item in old)
                        {
                            var ipad = db.iPads.Find(item.IID);
                            if (ipad != null)
                            {
                                ipad.Statue = iPadStatue.Vacant;
                                db.SaveChanges();
                            }
                        }
                    }

                    db.Register_iPads.RemoveRange(old);
                    db.SaveChanges();
                }
                db.Register_iPads.AddRange(list);
                db.SaveChanges();
            }
        }

        public void Append(List<Register_iPad> list)
        {
            using (var db = GetDbContext())
            {
                db.Register_iPads.AddRange(list);
                db.SaveChanges();
            }
        }

        public void Change(List<Register_iPad> list)
        {
            using (var db = GetDbContext())
            {
                foreach(var item in list)
                {
                    var entry = db.Register_iPads.Where(e => e.RID == item.RID && e.IID == item.IID && e.Relation == item.Relation).FirstOrDefault();
                    if (entry != null)
                    {
                        item.ID = entry.ID;
                        db.Entry(entry).CurrentValues.SetValues(item);
                        db.SaveChanges();
                    }
                }
            }
        }

        public List<Register_iPad> Get(int rid,Relation relation)
        {
            using (var db = GetDbContext())
            {
                var list= db.Register_iPads.Where(e => e.RID == rid && e.Relation == relation).ToList();
                foreach(var item in list)
                {
                    item.iPad = db.iPads.Find(item.IID);
                }
                return list;
            }
        }

        public List<Register_iPad> GetByiPadID(int iid)
        {
            using (var db = GetDbContext())
            {
                return db.Register_iPads.Where(e => e.IID == iid).ToList();
            }
        }
    }
}
