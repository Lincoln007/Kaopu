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

        public List<iPadRegister> Get()
        {
            using (var db = GetDbContext())
            {
                return db.iPad_Registers.ToList();
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
                    var register_iPads = db.Register_iPads.Where(e => e.RID == entry.ID).ToList();
                    if (register_iPads.Count > 0)
                    {
                        entry.iPads = register_iPads.Select(e => db.iPads.Find(e.IID)).Where(e => e != null).ToList();
                    }
                  
                }
                return entry;
            }
        }
    }
}
