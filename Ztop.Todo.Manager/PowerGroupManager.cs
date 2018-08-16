using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztop.Todo.Model;

namespace Ztop.Todo.Manager
{
    public class PowerGroupManager:ManagerBase
    {
        public List<PowerGroup> GetList()
        {
            return DB.PowerGroups.ToList();
        }

        public List<PowerGroup> GetList(int[] Ids)
        {
            var list = new List<PowerGroup>();
            foreach(var item in Ids)
            {
                var entry = DB.PowerGroups.Find(item);
                if (entry != null)
                {
                    list.Add(entry);
                }
            }
            return list;
        }

        public bool SaveGroups(int userId,int[] groupIds)
        {
            var model = DB.Users.Find(userId);
            if (model == null)
            {
                return false;
            }
            model.GroupIds = groupIds;
            DB.SaveChanges();
            return true;
        }
    }
}
