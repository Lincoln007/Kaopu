using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztop.Todo.Model;

namespace Ztop.Todo.Manager
{
    public class ProjectSituationManager:ManagerBase
    {
        public ProjectSituation Get(int id)
        {
            return DB.ProjectSituation.Find(id);
        }

        public int Add(ProjectSituation situation)
        {
            DB.ProjectSituation.Add(situation);
            DB.SaveChanges();
            return situation.ID;
        }

        public bool Edit(ProjectSituation situation)
        {
            var model = DB.ProjectSituation.Find(situation.ID);
            if (model == null)
            {
                return false;
            }
            DB.Entry(situation).CurrentValues.SetValues(situation);
            DB.SaveChanges();
            return true;
        }


        public bool Delete(int id)
        {
            var model = DB.ProjectSituation.Find(id);
            if (model == null)
            {
                return false;
            }
            model.Delete = true;
            DB.SaveChanges();
            return true;
        }
    }
}
