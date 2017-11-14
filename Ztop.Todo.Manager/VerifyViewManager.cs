using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztop.Todo.Model;

namespace Ztop.Todo.Manager
{
    public class VerifyViewManager:ManagerBase
    {
        public List<VerifyView> Search(SheetVerifyParameter parameter,bool IsTransfer=false)
        {
            using (var db = GetDbContext())
            {
                var query = db.VerifyViews.Where(e => e.Deleted == false&&e.Step!=Step.Create).AsQueryable();
                
                if (parameter.Year.HasValue)
                {
                    query = query.Where(e => e.SheetTime.Year == parameter.Year.Value);
                }

                if (parameter.Month.HasValue)
                {
                    query = query.Where(e => e.SheetTime.Month == parameter.Month.Value);
                }
                if (parameter.Position.HasValue)
                {
                    query = query.Where(e => e.Position == parameter.Position.Value);
                }

                if (!string.IsNullOrEmpty(parameter.Checker))
                {
                    query = query.Where(e => e.Name == parameter.Checker);
                }
                if (parameter.StartTime.HasValue)
                {
                    query = query.Where(e => e.Time >= parameter.StartTime.Value);
                }
                if (parameter.EndTime.HasValue)
                {
                    query = query.Where(e => e.Time <= parameter.EndTime.Value);
                }

                if (!string.IsNullOrEmpty(parameter.Coding))
                {
                    query = query.Where(e => e.PrintNumber.Contains(parameter.Coding));
                }
                //if (!string.IsNullOrEmpty(parameter.Coding))
                //{
                //    query = query.Where(e => e.Coding.ToLower().Contains(parameter.Coding.ToLower()));
                //}
                if (parameter.MinMoney.HasValue)
                {
                    query = query.Where(e => e.Money >= parameter.MinMoney.Value);
                }
                if (parameter.MaxMoney.HasValue)
                {
                    query = query.Where(e => e.Money <= parameter.MaxMoney.Value);
                }
                if (!string.IsNullOrEmpty(parameter.Creater))
                {
                    query = query.Where(e => e.SheetName.ToLower().Contains(parameter.Creater.ToLower()));
                }
                if (!string.IsNullOrEmpty(parameter.Time))
                {
                    var currentTime = DateTime.Now;
                    switch (parameter.Time)
                    {
                        case "一周内":
                            currentTime = currentTime.AddDays(-7);
                            break;
                        case "一个月内":
                            currentTime = currentTime.AddMonths(-1);
                            break;
                        case "半年内":
                            currentTime = currentTime.AddMonths(-6);
                            break;
                        case "一年内":
                            currentTime = currentTime.AddYears(-1);
                            break;
                    }
                    query = query.Where(e => e.SheetTime >= currentTime);
                }
                if (IsTransfer == true)
                {
                    query = query.Where(e => e.Type == SheetType.Transfer);
                }
                else
                {
                    query = query.Where(e => e.Type != SheetType.Transfer);
                  
                }
                if (parameter.SheetType.HasValue)
                {
                    query = query.Where(e => e.Type == parameter.SheetType.Value);
                }
                if (parameter.RID.HasValue)
                {
                    query = query.Where(e => e.RID.HasValue && e.RID.Value == parameter.RID.Value);
                }
                if (parameter.SRID.HasValue)
                {
                    query = query.Where(e => e.SRID.HasValue && e.SRID.Value == parameter.SRID.Value);
                }
                if (!string.IsNullOrEmpty(parameter.Content))
                {
                    query = query.Where(e => (!string.IsNullOrEmpty(e.Remarks) && e.Remarks.Contains(parameter.Content)) || (!string.IsNullOrEmpty(e.Place) && e.Place.Contains(parameter.Content)));
                }
                //var list = query.ToList();
                //query = list.GroupBy(e=>e.PrintNumber).Select(e=>e.First()).AsQueryable();
                if (!string.IsNullOrEmpty(parameter.CheckKey))
                {
                    query = query.Where(e => e.CheckNumber.Contains(parameter.CheckKey));
                }
                var list = query.ToList();
                query = list.OrderBy(e=>e.ID).GroupBy(e => e.PrintNumber).Select(e => e.First()).AsQueryable(); 
                switch (parameter.Order)
                {
                    case Order.Time:
                        query = query.OrderByDescending(e => e.Time);
                        break;
                    case Order.Money:
                        query = query.OrderByDescending(e => e.Money);
                        break;
                    case Order.PrintNumber:
                        query = query.OrderByDescending(e => e.PrintNumber);
                        break;
                    case Order.CheckNumber:
                        query = query.OrderByDescending(e => e.CheckNumber);
                        break;
                }
                query = query.SetPage(parameter.Page);
                return query.ToList();
            }
        }
    }
}
