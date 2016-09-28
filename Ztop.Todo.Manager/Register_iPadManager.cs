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
       public void Add(int[] ipads,int registerId)
        {
            using(var db = GetDbContext())
            {
                var old = db.Register_iPads.Where(e => e.RID == registerId).ToList();
                if (old != null && old.Count > 0)
                {
                    db.Register_iPads.RemoveRange(old);
                    db.SaveChanges();
                }
                db.Register_iPads.AddRange(ipads.Select(e => new Register_iPad { IID = e, RID = registerId }));
                db.SaveChanges();
            }
        }
    }
}
