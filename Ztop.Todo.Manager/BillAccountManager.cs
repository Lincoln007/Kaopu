using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztop.Todo.Model;

namespace Ztop.Todo.Manager
{
    public class BillAccountManager:ManagerBase
    {
        public int Save(BillAccount billaccount)
        {
            using (var db = GetDbContext())
            {
                db.BillAccounts.Add(billaccount);
                db.SaveChanges();
                return billaccount.ID;
            }
        }
        

        public BillAccount Get(int id)
        {
            using (var db = GetDbContext())
            {
                return db.BillAccounts.Find(id);
            }
        }

        public List<BillAccount> Search(BillAccountParameter parameter)
        {
            using (var db = GetDbContext())
            {
                var query = db.BillAccounts.AsQueryable();
                if (parameter.StartTime.HasValue)
                {
                    query = query.Where(e => e.Time >= parameter.StartTime.Value);
                }
                if (parameter.EndTime.HasValue)
                {
                    query = query.Where(e => e.Time <= parameter.EndTime.Value);
                }
                if (parameter.MinMoney.HasValue)
                {
                    query = query.Where(e => e.Money >= parameter.MinMoney.Value);
                }
                if (parameter.MaxMoney.HasValue)
                {
                    query = query.Where(e => e.Money <= parameter.MaxMoney.Value);
                }
                if (!string.IsNullOrEmpty(parameter.OtherSide))
                {
                    query = query.Where(e => e.Account.Contains(parameter.OtherSide));
                }
                if (!string.IsNullOrEmpty(parameter.Remark))
                {
                    query = query.Where(e => e.Remark.Contains(parameter.Remark));
                }
                if (parameter.Association.HasValue)
                {
                    query = query.Where(e => e.Association == parameter.Association.Value);
                }
                query = query.OrderBy(e => e.Time).SetPage(parameter.Page);
                return query.ToList();
            }
        }
    }
}
