using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztop.Todo.Model;

namespace Ztop.Todo.Manager
{
    public class iPad_ContractManager:ManagerBase
    {
        public int Save(iPadContract contract)
        {
            using (var db = GetDbContext())
            {
                var entry = db.iPad_Contracts.Find(contract.ID);
                if (entry == null)
                {
                    db.iPad_Contracts.Add(contract);
                }
                else
                {
                    db.Entry(entry).CurrentValues.SetValues(contract);
                }
                db.SaveChanges();
                return contract.ID;
            }
        }

        public List<iPadContract> Get()
        {
            using (var db = GetDbContext())
            {
                var list = db.iPad_Contracts.ToList();
                foreach(var item in list)
                {
                    item.iPads = db.Register_iPads.Where(e => e.RID == item.ID && e.Relation == Relation.Contract_iPad).ToList().Select(e => db.iPads.Find(e.IID)).ToList();
                }
                return list;
            }
        }

        public iPadContract Get(int id)
        {
            if (id == 0) return null;
            using (var db = GetDbContext())
            {
                var entry = db.iPad_Contracts.Find(id);
                if (entry != null)
                {
                    var Contract_iPads = db.Register_iPads.Where(e => e.RID == entry.ID && e.Relation == Relation.Contract_iPad).ToList();
                    foreach(var item in Contract_iPads)
                    {
                        item.iPad = db.iPads.Find(item.IID);
                    }
                    entry.Contract_iPads = Contract_iPads;
                }
                return entry;
            }
        }
    }
}
