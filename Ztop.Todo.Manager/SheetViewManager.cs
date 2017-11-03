using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztop.Todo.Model;

namespace Ztop.Todo.Manager
{
    public class SheetViewManager : ManagerBase
    {
        public List<SheetView> Search(SheetParameter parameter)
        {
            using (var db = GetDbContext())
            {
                var query = db.SheetViews.Where(e => e.CheckTime.HasValue).AsQueryable();
                if (parameter.Type.HasValue)
                {
                    query = query.Where(e => e.Type == parameter.Type.Value);
                }
                if (parameter.Deleted.HasValue)
                {
                    query = query.Where(e => e.Deleted == parameter.Deleted.Value);
                }
                if (parameter.Cost.HasValue)
                {
                    query = query.Where(e => e.Cost == parameter.Cost.Value);
                }
                if (!string.IsNullOrEmpty(parameter.Name))
                {
                    query = query.Where(e => e.Name == parameter.Name);
                }
                if (parameter.StartTime.HasValue)
                {
                    query = query.Where(e => e.CheckTime >= parameter.StartTime.Value);
                }
                if (parameter.EndTime.HasValue)
                {
                    query = query.Where(e => e.CheckTime <= parameter.EndTime.Value);
                }
                if (parameter.minMoney.HasValue)
                {
                    query = query.Where(e => e.Money >= parameter.minMoney.Value);
                }
                if (parameter.maxMoney.HasValue)
                {
                    query = query.Where(e => e.Money <= parameter.maxMoney.Value);
                }
                //if (parameter.RID.HasValue)
                //{
                //    query = query.Where(e => e.RID == parameter.RID.Value);
                //}
                if (!string.IsNullOrEmpty(parameter.Content))
                {
                    query = query.Where(e => (!string.IsNullOrEmpty(e.Place) && e.Place.ToLower().Contains(parameter.Content.ToLower()) || (!string.IsNullOrEmpty(e.Reason) && e.Reason.ToLower().Contains(parameter.Content.ToLower()))));
                }
                var list = query.ToList();
                query = list.GroupBy(e => e.PrintNumber).Select(e => e.First()).AsQueryable();
                switch (parameter.Order)
                {
                    case Order.CheckNumber:
                        query = query.OrderByDescending(e => e.CheckNumber);
                        break;
                    case Order.Money:
                        query = query.OrderByDescending(e => e.Money);
                        break;
                    case Order.PrintNumber:
                        query = query.OrderByDescending(e => e.PrintNumber);
                        break;
                    case Order.Time:
                        query = query.OrderByDescending(e => e.Time);
                        break;
                }
                query = query.SetPage(parameter.Page);
                return query.ToList();

            }
        }
        public Dictionary<string, List<SheetView>> Download(SheetParameter parameter)
        {
            using (var db = GetDbContext())
            {
                var query = db.SheetViews.Where(e => e.CheckTime.HasValue).AsQueryable();
                if (parameter.Type.HasValue)
                {
                    query = query.Where(e => e.Type == parameter.Type.Value);
                }
                if (parameter.Deleted.HasValue)
                {
                    query = query.Where(e => e.Deleted == parameter.Deleted.Value);
                }
                if (parameter.Cost.HasValue)
                {
                    query = query.Where(e => e.Cost == parameter.Cost.Value);
                }
                if (!string.IsNullOrEmpty(parameter.Name))
                {
                    query = query.Where(e => e.Name == parameter.Name);
                }
                if (parameter.StartTime.HasValue)
                {
                    query = query.Where(e => e.CheckTime >= parameter.StartTime.Value);
                }
                if (parameter.EndTime.HasValue)
                {
                    query = query.Where(e => e.CheckTime <= parameter.EndTime.Value);
                }
                if (parameter.minMoney.HasValue)
                {
                    query = query.Where(e => e.Money >= parameter.minMoney.Value);
                }
                if (parameter.maxMoney.HasValue)
                {
                    query = query.Where(e => e.Money <= parameter.maxMoney.Value);
                }
                //if (parameter.RID.HasValue)
                //{
                //    query = query.Where(e => e.RID == parameter.RID.Value);
                //}
                if (!string.IsNullOrEmpty(parameter.Content))
                {
                    query = query.Where(e => (!string.IsNullOrEmpty(e.Place) && e.Place.ToLower().Contains(parameter.Content.ToLower()) || (!string.IsNullOrEmpty(e.Reason) && e.Reason.ToLower().Contains(parameter.Content.ToLower()))));
                }
                return query.ToList().GroupBy(e => e.PrintNumber).ToDictionary(e => e.Key, e => e.ToList());
            }
        }
    }
}
