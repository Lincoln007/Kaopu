using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztop.Todo.Model;

namespace Ztop.Todo.Manager
{
    public class Report_ManagerManager:ManagerBase
    {
        public List<ReportManagerView> Get()
        {
            using (var db = GetDbContext())
            {
                return db.Report_Manager_Views.Where(e => e.Delete == false).ToList();
            }
        }
    }
}
