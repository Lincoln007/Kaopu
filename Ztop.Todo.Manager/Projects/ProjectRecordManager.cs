using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztop.Todo.Model;

namespace Ztop.Todo.Manager
{
    public class ProjectRecordManager:ManagerBase
    {
        public int Save(ProjectRecord record)
        {
            DB.Project_Records.Add(record);
            DB.SaveChanges();
            return record.ID;
        }

        public List<ProjectRecord> Get(int projectId)
        {
            return DB.Project_Records.Where(e => e.ProjectId == projectId).OrderBy(e => e.Time).ToList();
        }
    }
}
