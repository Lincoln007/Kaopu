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
        public List<Substancs> GetSubstances(int[] rid,int[] srid,string[] details,double[] prices,bool[] payWays)
        {
            //var details = context.Request.Form["Detail"].Split(',');
            //var prices = context.Request.Form["Price"].Split(',');
            if (rid == null || rid.Count() != 10||details.Count()!=10||prices.Count()!=10)
            {
                return null;
            }
            var list = new List<Substancs>();
            //double temp = .0;
            var j = 0;
            var k = 0;
            for(var i = 0; i < 10; i++)
            {
                if (rid[i] == 0)
                {
                    break;
                }
                var rt = Core.Report_TypeManager.Get(rid[i]);
                if (rt == null||Math.Round(prices[i]-0)<0.001)
                {
                    continue;
                }
                var entry = new Substancs()
                {
                    RID=rid[i],
                    Details = details[i],
                    Price=prices[i]

                    //Price = double.TryParse(prices[i], out temp) ? temp : .0
                };
                if (rt.Children != null&&srid!=null&&srid.Length>j&&rt.Children.FirstOrDefault(e=>e.ID==srid[j])!=null)
                {
                    entry.SRID = srid[j++];
                }
                if (rt.IsEnterprise&&payWays!=null&&payWays.Length>k)
                {
                    entry.EnterprisePay = payWays[k++];
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
        public List<Traffic> GetTraffic(HttpContextBase context,string[] busTypes,Driver[] drivers,double[] carpettys)
        {
            if (busTypes == null)
            {
                return null;
            }
            var a = .0;
           // var b = 0;
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
                        traffic.Driver = drivers[0];
                        traffic.CarPetty = carpettys[0];
                        traffic.Plate = plates[0];
                        traffic.KiloMeters = double.TryParse(context.Request.Form["KiloMeters"].Split(',')[0], out a) ? a: .0;
                        break;
                    case BusType.Personal:
                        traffic.Toll = double.TryParse(tolls[1], out a) ? a : .0;
                        traffic.Driver = drivers[1];
                        traffic.CarPetty = carpettys[1];
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
