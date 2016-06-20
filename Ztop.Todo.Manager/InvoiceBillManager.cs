using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
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
        public List<InvoiceBill> Get(HttpContextBase context,int bid)
        {
            var iidString = context.Request.Form["IID"].ToString().Split(',');
            int a = 0;
            double b = .0;
            var list = new List<InvoiceBill>();
            foreach(var item in iidString)
            {
                list.Add(new InvoiceBill()
                {
                    Price = double.TryParse(context.Request.Form[string.Format("{0}Price", item)].ToString(), out b) ? b : .0,
                    IID = int.TryParse(item, out a) ? a : 0,
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
