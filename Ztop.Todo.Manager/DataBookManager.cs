using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztop.Todo.Model;

namespace Ztop.Todo.Manager
{
    public class DataBookManager:ManagerBase
    {
        public List<DataBook> GetListByGroupName(string GroupName)
        {
            using (var db = GetDbContext())
            {
                return db.DataBooks.Where(e => e.GroupName == GroupName).ToList();
            }
        }
        public List<DataBook> Get(List<string> GroupNames,CheckStatus status)
        {
            var list = new List<DataBook>();
            foreach(var item in GroupNames)
            {
                var glist = GetListByGroupName(item).Where(e => e.Status == status).ToList();
                if (glist != null)
                {
                    foreach(var entry in glist)
                    {
                        list.Add(entry);
                    }
                }
            }
            return list;
        }
    }
}
