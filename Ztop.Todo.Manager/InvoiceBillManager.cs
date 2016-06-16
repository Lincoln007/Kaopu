using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztop.Todo.Model;

namespace Ztop.Todo.Manager
{
    public class InvoiceBillManager:ManagerBase
    {
        public List<InvoiceBill> Get(int[] iid,double[] price,int bid)
        {
            var count = iid.Count();
            var list = new List<InvoiceBill>();
            for(var i = 0; i < count; i++)
            {
                list.Add(new InvoiceBill()
                {
                    Price = price[i],
                    IID = iid[i],
                    BID = bid
                });
            }
            return list;
        }
        public void Save(List<InvoiceBill> list)
        {
            using (var db = GetDbContext())
            {
                db.InvoiceBills.AddRange(list);
                db.SaveChanges();
            }
        }
    }
}
