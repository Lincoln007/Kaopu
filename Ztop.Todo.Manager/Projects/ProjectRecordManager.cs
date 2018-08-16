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


        public List<ProjectRecord> Search(ProjectRecordParameter parameter)
        {
            var query = DB.Project_Records.AsQueryable();

            if (parameter.ProjectId.HasValue)
            {
                query = query.Where(e => e.ProjectId == parameter.ProjectId.Value);
            }
            if (parameter.UserId.HasValue)
            {
                query = query.Where(e => e.UserId == parameter.UserId.Value);
            }
            query = query.OrderByDescending(e=>e.Time).SetPage(parameter.Page);
            return query.ToList();
        }
    }
}
