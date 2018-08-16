using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztop.Todo.Model;

namespace Ztop.Todo.Manager
{
    public class FlowwManager:ManagerBase
    {
        public Floww Get(int id)
        {
            return DB.Flowws.Find(id);
        }

        public List<Floww> GetList()
        {
            var results= DB.Flowws.Where(e => e.Delete == false).ToList();
            foreach(var item in results)
            {
                item.Nodes2 = Core.FlowwNodeManager.Getlist(item.ID);
            }
            return results;
        }

        public int Add(Floww Flow)
        {
            DB.Flowws.Add(Flow);
            DB.SaveChanges();
            return Flow.ID;
        }

        public bool Edit(Floww Flow)
        {
            var model = DB.Flowws.Find(Flow.ID);
            if (model == null)
            {
                return false;
            }
            DB.Entry(model).CurrentValues.SetValues(Flow);
            DB.SaveChanges();

            return true;
        }

        public bool Delete(int id,bool flag=true)
        {
            var model = DB.Flowws.Find(id);
            if (model == null)
            {
                return false;
            }
            model.Delete = flag;
            DB.SaveChanges();
            return true;
        }
    }
}
