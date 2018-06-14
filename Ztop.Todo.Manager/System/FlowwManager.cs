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
    }
}
