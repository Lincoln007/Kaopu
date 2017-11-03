using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztop.Todo.Model;

namespace Ztop.Todo.Manager
{
    public class Power_itemManager:ManagerBase
    {
      
        public List<PowerItem> Get(int userId)
        {
            return DB.Items.Where(e => e.UserId == userId).ToList();
        }

        public PowerItem Get(int userId,int PowerId)
        {
            return DB.Items.FirstOrDefault(e => e.UserId == userId && e.PowerId == PowerId);
        }
        
    }
}
