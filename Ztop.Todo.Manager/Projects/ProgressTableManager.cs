using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztop.Todo.Model;

namespace Ztop.Todo.Manager
{
    public class ProgressTableManager:ManagerBase
    {
        public bool Update(List<ProgressTable> list)
        {
            foreach(var item in list)
            {
                var entry = DB.ProgressTables.FirstOrDefault(e => e.Year == item.Year && e.ProjectId==item.ProjectId);
                if (entry == null)
                {
                    DB.ProgressTables.Add(item);
                }
                else
                {
                    item.ID = entry.ID;
                    var old = DB.WorkLoads.Where(e => e.ProgressTableId == item.ID).ToList();
                    if (old != null)
                    {
                        DB.WorkLoads.RemoveRange(old);
                    }
                    foreach (var ee in item.WorkLoads)
                    {
                        ee.ProgressTableId = item.ID;
                        DB.WorkLoads.Add(ee);
                    }
                    DB.Entry(entry).CurrentValues.SetValues(item);
                }
            }
            DB.SaveChanges();
            return true;
        }
        public List<ProgressTable> Search(int year)
        {
            return DB.ProgressTables.Where(e => e.Year == year).ToList();
        }
    }
}
