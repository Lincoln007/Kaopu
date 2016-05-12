using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Ztop.Todo.Model;

namespace Ztop.Todo.Manager
{
    public class BillManager:ManagerBase
    {
        public List<Bill> GetBills(int bid,string[] coding,DateTime[] time,Budget[] budget,double[] money,string[] account,string[] summary)
        {
            if (coding == null)
            {
                throw new ArgumentException("参数错误");
            }
            var count = coding.Count();
            
            var list = new List<Bill>();
            for(var i = 0; i < count; i++)
            {
                list.Add(new Bill()
                {
                    Coding = coding[i],
                    Time = time[i],
                    Money = money[i],
                    Account = account[i],
                    Budget = budget[i],
                    Summary = summary[i],
                    BID = bid
                });
            }
            return list;
        }

        public Bank GetBank(int year,int month,Company company)
        {
            using (var db = GetDbContext())
            {
                var entry = db.Banks.FirstOrDefault(e => e.Year == year && e.Company == company && e.Month == month);
                if (entry == null)
                {
                    entry = new Bank() { Year = year, Month = month, Company = company };
                    db.Banks.Add(entry);
                    db.SaveChanges();
                }
                return entry;
            }
        }

        public Bank GetBank(int id)
        {
            if (id == 0)
            {
                return null;
            }
            using (var db = GetDbContext())
            {
                var entry = db.Banks.Find(id);
                if (entry != null)
                {
                    entry.Bills = db.Bills.Where(e => e.BID == entry.ID).ToList();
                }
                return entry;
            }
        }
        public void UpDateBills(List<Bill> list,int bid)
        {
            using (var db = GetDbContext())
            {
                var older = db.Bills.Where(e => e.BID == bid).ToList();
                if (older != null)
                {
                    db.Bills.RemoveRange(older);
                    db.SaveChanges();
                }
                db.Bills.AddRange(list);
                db.SaveChanges();
            }
        }
        public List<Bank> GetBanks(Company company)
        {
            using (var db = GetDbContext())
            {
                return db.Banks.Where(e => e.Company == company).ToList();
            }
        }

        
    }
}
