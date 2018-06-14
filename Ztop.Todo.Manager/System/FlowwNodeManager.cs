using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztop.Todo.Model;

namespace Ztop.Todo.Manager
{
    public class FlowwNodeManager:ManagerBase
    {
        public FlowwNode Get(int id)
        {
            var model = DB.Floww_Nodes.Find(id);
            if (model != null)
            {
                if (model.UserIds != null)
                {
                    model.Users = Core.UserManager.GetUsers(model.UserIds);
                }
            }

            return model;
        }
    }
}
