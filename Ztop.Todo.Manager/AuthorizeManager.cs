using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztop.Todo.Model;

namespace Ztop.Todo.Manager
{
    public class AuthorizeManager:ManagerBase
    {
        public List<Authorize> GetList()
        {
            using (var db = GetDbContext())
            {
                return db.Authorizes.ToList();
            }
        }
        public List<string> GetList(string Name)
        {
            var authorize = GetList().FirstOrDefault(e => e.Manager == Name);
            if (authorize == null || string.IsNullOrEmpty(authorize.GroupName))
            {
                return new List<string>();
            }
            var array = authorize.GroupName.Split(',');
            var list = new List<string>();
            foreach (var item in array)
            {
                list.Add(item);
            }
            return list.OrderBy(e => e).ToList();
        }
    }
}
