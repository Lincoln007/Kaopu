using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztop.Todo.Model;

namespace Ztop.Todo.Manager
{
    public  class UserGroupManager:ManagerBase
    {
        public UserGroup Get(int id)
        {
            return DB.UserGroups.Find(id);
        }

        public List<UserGroup> Get()
        {
            return DB.UserGroups.OrderByDescending(e => e.ID).ToList();
        }
        
    }
}
