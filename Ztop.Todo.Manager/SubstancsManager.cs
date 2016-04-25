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
        public List<Substancs> GetSubstances(HttpContextBase context)
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
        public List<Errand> GetErrands(HttpContextBase context)
        {
            int a = 0, b = 0;
            DateTime startTime, endTime;
            var list = new List<Errand>();
            for(var i = 0; i < 3; i++)
            {
                if(int.TryParse(context.Request.Form[string.Format("Peoples{0}",i)].ToString(),out a))
                {
                    if(DateTime.TryParse(context.Request.Form[string.Format("StartTime{0}", i)].ToString(), out startTime))
                    {
                        if(DateTime.TryParse(context.Request.Form[string.Format("EndTime{0}",i)].ToString(),out endTime))
                        {
                            list.Add(new Errand
                            {
                                StartTime = startTime,
                                EndTime = endTime,
                                Users = context.Request.Form[string.Format("Users{0}", i)].ToString(),
                                Peoples = a,
                                Days = int.TryParse(context.Request.Form[string.Format("Days{0}", i)].ToString(), out b) ? b : (endTime - startTime).Days + 1
                            });
                        }
                    }
                }
            }
            return list;
        }
        public List<Traffic> GetTraffic(HttpContextBase context,string[] busTypes)
        {
            if (busTypes == null)
            {
                return null;
            }
            var a = .0;
            var b = 0;
            var list = new List<Traffic>();
            var times = context.Request.Form["Times"].Split(',');
            var plates = context.Request.Form["Plate"].Split(',');
            var tolls = context.Request.Form["Toll"].Split(',');
            foreach(var type in busTypes)
            {
                var traffic = new Traffic()
                {
                    Type = (BusType)Enum.Parse(typeof(BusType), type),
                    Cost = double.TryParse(context.Request.Form[string.Format("Cost{0}", type)].ToString(), out a) ? a : .0,
                };
                switch (traffic.Type)
                {
                    case BusType.Company:
                        traffic.Toll = double.TryParse(tolls[0], out a) ? a : .0;
                        traffic.Plate = plates[0];
                        break;
                    case BusType.Personal:
                        traffic.Toll = double.TryParse(tolls[1], out a) ? a : .0;
                        traffic.Plate = plates[1];
                        break;
                    case BusType.Didi:
                        traffic.Times = int.TryParse(times[0], out b) ? b : 0;
                        break;
                    case BusType.Taxi:
                        traffic.Times = int.TryParse(times[1], out b) ? b : 0;
                        break;
                    default:break;
                }
                list.Add(traffic);
            }
            return list;
        }
    }
}
