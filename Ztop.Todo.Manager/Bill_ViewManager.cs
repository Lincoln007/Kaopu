using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztop.Todo.Model;

namespace Ztop.Todo.Manager
{
    public class Bill_ViewManager:ManagerBase
    {
        /// <summary>
        /// 作用：查询
        /// 作者：汪建龙
        /// 编写时间：2017年1月11日10:06:02
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public List<BillView> Search(BillParamter parameter)
        {
            using (var db = GetDbContext())
            {
                var query = db.Bill_views.AsQueryable();
                if (parameter.Company.HasValue)
                {
                    query = query.Where(e => e.Company == parameter.Company.Value);
                }
                if (parameter.MinMoney.HasValue)
                {
                    query = query.Where(e => e.Money >= parameter.MinMoney.Value);
                }

                if (parameter.MaxMoney.HasValue)
                {
                    query = query.Where(e => e.Money <= parameter.MaxMoney.Value);
                }
                if (parameter.StartTime.HasValue)
                {
                    query = query.Where(e => e.Time >= parameter.StartTime.Value);
                }
                if (parameter.EndTime.HasValue)
                {
                    query = query.Where(e => e.Time <= parameter.EndTime.Value);
                }
                if (!string.IsNullOrEmpty(parameter.OtherSide))
                {
                    query = query.Where(e => e.Account.Contains(parameter.OtherSide));
                }
                if (parameter.Association.HasValue)
                {
                    query = query.Where(e => e.Association == parameter.Association.Value);
                }
                if (!string.IsNullOrEmpty(parameter.Remark))
                {
                    query = query.Where(e => e.Remark.Contains(parameter.Remark));
                }
                if (parameter.Cost.HasValue)
                {
                    query = query.Where(e => e.Cost == parameter.Cost.Value);
                }
                else
                {
                    query = query.Where(e => e.Cost != Cost.RealIncome);
                }

                query = query.OrderBy(e => e.ID).SetPage(parameter.Page);
                return query.ToList();
            }
        }

        /// <summary>
        /// 作用：通过ID获取BillView对象
        /// 作者：汪建龙
        /// 编写时间：2017年1月11日10:07:06
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BillView Get(int id)
        {
            using (var db = GetDbContext())
            {
                return db.Bill_views.Find(id);
            }
        }
    }
}
