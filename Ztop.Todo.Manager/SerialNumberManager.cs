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
        public SerialNumber GetNewModel(string name)
        {
            using (var db = GetDbContext())
            {
                SerialNumber serialNumber = new SerialNumber()
                {
                    Name = name
                };

                var entry = db.SerialNumbers.Where(e => e.Name == serialNumber.Name && e.Number == serialNumber.Number && e.SID == 0).OrderByDescending(e => e.ID).FirstOrDefault();
                if (entry != null)
                {
                    return entry;
                }
                else
                {
                    entry = db.SerialNumbers.Where(e => e.Number == serialNumber.Number).OrderByDescending(e => e.ID).FirstOrDefault();
                    int count = 0;
                    if (entry != null)
                    {
                        count = entry.NumberExt;
                    }
                    serialNumber.NumberExt = count + 1;
                    db.SerialNumbers.Add(serialNumber);
                    db.SaveChanges();
                }
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
