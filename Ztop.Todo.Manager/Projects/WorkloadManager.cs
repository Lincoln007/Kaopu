using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztop.Todo.Model;

namespace Ztop.Todo.Manager
{
    public class WorkloadManager:ManagerBase
    {
        public void Update(List<WorkLoad> list)
        {
            foreach(var item in list)
            {
                var model = DB.WorkLoads.FirstOrDefault(e => e.ProjectId == item.ProjectId && e.UserId == item.UserId);
                if (model == null)
                {
                    DB.WorkLoads.Add(item);
                }
                else
                {
                    model.Percent = item.Percent;
                }
            }
            DB.SaveChanges();
        }

        public List<WorkLoad> Get(int projectId)
        {
            return DB.WorkLoads.Where(e => e.ProjectId == projectId).ToList();
        }
    }
}
