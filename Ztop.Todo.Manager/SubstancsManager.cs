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
            var secondCategorys = context.Request.Form["SecondCategory"].ToString().Split(',');
            var details = context.Request.Form["Detail"].Split(',');
            var prices = context.Request.Form["Price"].Split(',');
            if (categorys.Count() != 10 || details.Count() != 10 || prices.Count() != 10)
            {
                return null;
            }
            var list = new List<Substancs>();
            double temp = .0;
            var j = 0;
            for(var i = 0; i < 10; i++)
            {
                if (string.IsNullOrEmpty(categorys[i]) || string.IsNullOrEmpty(details[i]) || string.IsNullOrEmpty(prices[i]))
                {
                    break;
                }
                var entry = new Substancs()
                {
                    Category = (Category)Enum.Parse(typeof(Category), categorys[i]),
                    Details = details[i],
                    Price = double.TryParse(prices[i], out temp) ? temp : .0
                };

                switch (entry.Category)
                {
                    case Category.FixedAsssets://固定资产
                    case Category.Equipment://耗材
                    case Category.Traffic://交通费
                    case Category.Express://邮电费
                    case Category.Print://印刷装订
                    case Category.Welfare://福利费
                    case Category.Bidding://招投标费
                        entry.SecondCategory = (SecondCategory)Enum.Parse(typeof(SecondCategory), secondCategorys[j++]);
                        break;
                    default:
                        break;
                }

                list.Add(entry);
            }
            return list;
        }
        public List<Errand> GetErrands(HttpContextBase context,string lines)
        {
            if (string.IsNullOrEmpty(lines))
            {
                return new List<Errand>();
            }
            int a = 0, b = 0;
            DateTime startTime, endTime;
            var list = new List<Errand>();
            var indexs = lines.Split(';');
            foreach(var index in indexs)
            {
                var peopleString = context.Request.Form[string.Format("Peoples{0}", index)].ToString();
                if (int.TryParse(peopleString,out a))
                {
                    if (int.TryParse(index, out a))
                    {
                        var startTimeString = context.Request.Form[string.Format("StartTime{0}", a)].ToString();
                        if (DateTime.TryParse(startTimeString, out startTime))
                        {
                            var endTimeString = context.Request.Form["EndTime"+index];
                            if (DateTime.TryParse(endTimeString, out endTime))
                            {
                                var users = context.Request.Form[string.Format("Users{0}", a)].ToString();
                                list.Add(new Errand
                                {
                                    StartTime = startTime,
                                    EndTime = endTime,
                                    Users = users,
                                    Peoples = users.Split(';').Count(),
                                    Days = int.TryParse(context.Request.Form[string.Format("Days{0}", index)].ToString(), out b) ? b : (endTime - startTime).Days + 1
                                });
                            }
                        }
                    }
                }
              
            }
            //for(var i = 0; i < 3; i++)
            //{
            //    if(int.TryParse(context.Request.Form[string.Format("Peoples{0}",i)].ToString(),out a))
            //    {
            //        if(DateTime.TryParse(context.Request.Form[string.Format("StartTime{0}", i)].ToString(), out startTime))
            //        {
            //            if(DateTime.TryParse(context.Request.Form[string.Format("EndTime{0}",i)].ToString(),out endTime))
            //            {
            //                list.Add(new Errand
            //                {
            //                    StartTime = startTime,
            //                    EndTime = endTime,
            //                    Users = context.Request.Form[string.Format("Users{0}", i)].ToString(),
            //                    Peoples = a,
            //                    Days = int.TryParse(context.Request.Form[string.Format("Days{0}", i)].ToString(), out b) ? b : (endTime - startTime).Days + 1
            //                });
            //            }
            //        }
            //    }
            //}
            return list;
        }
        public List<Traffic> GetTraffic(HttpContextBase context,string[] busTypes,Driver driver,double carpetty)
        {
            if (busTypes == null)
            {
                return null;
            }
            var a = .0;
            var b = 0;
            var list = new List<Traffic>();
            //var times = context.Request.Form["Times"].Split(',');
            var plates = context.Request.Form["Plate"].Split(',');
            var tolls = context.Request.Form["Toll"].Split(',');
            var petrol = context.Request.Form["Petrol"].Split(',');
            
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
                        traffic.Petrol = double.TryParse(petrol[0], out a) ? a : .0;
                        traffic.Driver = driver;
                        traffic.CarPetty = carpetty;
                        traffic.Plate = plates[0];
                        traffic.KiloMeters = double.TryParse(context.Request.Form["KiloMeters"].Split(',')[0], out a) ? a: .0;
                        break;
                    case BusType.Personal:
                        traffic.Toll = double.TryParse(tolls[1], out a) ? a : .0;
                        traffic.Plate = plates[1];
                        traffic.KiloMeters = double.TryParse(context.Request.Form["KiloMeters"].Split(',')[1], out a) ? a : .0;
                        break;
                    //case BusType.Didi:
                    //    traffic.Times = int.TryParse(times[0], out b) ? b : 0;
                    //    break;
                    //case BusType.Taxi:
                        //traffic.Times = int.TryParse(times[1], out b) ? b : 0;
                        //break;
                    default:break;
                }
                list.Add(traffic);
            }
            return list;
        }

        public List<Substancs> GetSubstances(List<Sheet> sheets)
        {
            var list = new List<Substancs>();
            using(var db = GetDbContext())
            {
                foreach (var item in sheets)
                {
                    var temp = db.Substances.Where(e => e.SID == item.ID).ToList();
                    if (temp != null && temp.Count != 0)
                    {
                        list.AddRange(temp);
                    }
                }
            }
   
            return list;
        }
    }
}
