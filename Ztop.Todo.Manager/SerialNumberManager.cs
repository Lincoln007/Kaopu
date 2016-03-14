using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztop.Todo.Model;

namespace Ztop.Todo.Manager
{
    public class SerialNumberManager:ManagerBase
    {
        public SerialNumber GetNewModel()
        {
            using (var db = GetDbContext())
            {
                var serialNumber = new SerialNumber();
                var entry = db.SerialNumbers.OrderByDescending(e => e.ID).FirstOrDefault(e => e.Number == serialNumber.Number);
                int count = 0;
                if (entry != null)
                {
                    count = entry.NumberExt;
                }
                serialNumber.NumberExt = (count + 1);
                db.SerialNumbers.Add(serialNumber);
                db.SaveChanges();
                return serialNumber;
            }
        }
        public void Update(SerialNumber serialNumber)
        {
            using (var db = GetDbContext())
            {
                var entry = db.SerialNumbers.FirstOrDefault(e => e.ID == serialNumber.ID && e.Number == serialNumber.Number && e.NumberExt == serialNumber.NumberExt);
                if (entry != null)
                {
                    db.Entry(entry).CurrentValues.SetValues(serialNumber);
                    db.SaveChanges();
                }
            }
        }
    }
}
