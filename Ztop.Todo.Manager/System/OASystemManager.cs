using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztop.Todo.Model;

namespace Ztop.Todo.Manager
{
    public class OASystemManager:ManagerBase
    {
        public List<OASystem> Get()
        {
            var list = DB.OASystems.Where(e=>e.Deleted==false).OrderBy(e => e.ID).ToList();
            return list;
        }

        public List<OASystemBase> Get(OASystemClass oaSystemClass)
        {
            return DB.OASystems.Where(e => e.OASystemClass == oaSystemClass && e.Deleted == false).Select(e=>new OASystemBase {
                Name=e.Name,
                Class=e.Class,
                Order=e.Order
            }).OrderBy(e => e.Order).ToList();
        }

        public OASystem Get(int id)
        {
            return DB.OASystems.Find(id);
        }


        public int Save(OASystem system)
        {
            if (system.ID > 0)
            {
                var model = DB.OASystems.Find(system.ID);
                if (model != null)
                {
                    model.Name = system.Name;
                }
            }
            else
            {
                DB.OASystems.Add(system);
            }
           
            DB.SaveChanges();
            return system.ID;
        }
    }
}
