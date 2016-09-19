using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztop.Todo.Model;

namespace Ztop.Todo.Manager
{
    public class BankManager:ManagerBase
    {
        public Bank GetBank(int year,int month,Company company)
        {
            using (var db = GetDbContext())
            {
                return db.Banks.FirstOrDefault(e => e.Year == year && e.Month == month && e.Company == company);
            }
        }

        public void Save(Bank bank)
        {
            using (var db = GetDbContext())
            {
                var temp = db.Banks.FirstOrDefault(e => e.Year == bank.Year && e.Month == bank.Month && e.Company == bank.Company);

                if (temp == null)
                {
                    db.Banks.Add(bank);
                }
                else
                {
                    temp.Balance = bank.Balance;
                }
                db.SaveChanges();
            }
        }
    }
}
