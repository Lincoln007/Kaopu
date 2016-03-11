using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Ztop.Todo.Model;

namespace Ztop.Todo.Manager
{
    public class SubstancsManager:ManagerBase
    {
        public List<Substancs> Get(HttpContextBase context)
        {
            var categorys = context.Request.Form["Category"].ToString().Split(',');
            var details = context.Request.Form["Detail"].Split(',');
            var prices = context.Request.Form["Price"].Split(',');
            if (categorys.Count() != 10 || details.Count() != 10 || prices.Count() != 10)
            {
                return null;
            }
            var list = new List<Substancs>();
            double temp = .0;
            for(var i = 0; i < 10; i++)
            {
                if (string.IsNullOrEmpty(categorys[i]) || string.IsNullOrEmpty(details[i]) || string.IsNullOrEmpty(prices[i]))
                {
                    break;
                }

                list.Add(new Substancs
                {
                    Category = (Category)Enum.Parse(typeof(Category),categorys[i]),
                    Details = details[i],
                    Price = double.TryParse(prices[i], out temp) ? temp : .0
                });
            }
            return list;
        }
    }
}
