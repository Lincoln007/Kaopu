using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztop.Todo.Model;

namespace Ztop.Todo.Manager
{
    public class Project_ProgressManager:ManagerBase
    {
        public int Save(ProjectProgress progress)
        {
            DB.Project_Progress.Add(progress);
            DB.SaveChanges();
            return progress.ID;
        }

        public ProjectProgress GetLast(int projectId)
        {
            return DB.Project_Progress.Where(e => e.ProjectID == projectId&&e.Delete==false).OrderByDescending(e => e.Time).FirstOrDefault();
        }

        public List<ProjectProgress> Get(int projectId)
        {
            return DB.Project_Progress.Where(e => e.ProjectID == projectId&&e.Delete==false).OrderBy(e => e.Time).ToList();
        }

        public bool Delete(int id)
        {
            var model = DB.Project_Progress.Find(id);
            if (model != null)
            {
                model.Delete = true;
                DB.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
