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
    }
}
