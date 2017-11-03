using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztop.Todo.Model;

namespace Ztop.Todo.Manager
{
    public class PowerManager:ManagerBase
    {

        public int Add(Power power)
        {
            DB.Powers.Add(power);
            DB.SaveChanges();
            return power.ID;
        }

        public bool Edit(Power power)
        {
            var entry = DB.Powers.Find(power.ID);
            if (entry != null)
            {

                DB.Items.RemoveRange(entry.Items);
                DB.Entry(entry).CurrentValues.SetValues(power);
                entry.Items = power.Items;
                DB.SaveChanges();
                return true;
            }
            return false;
        }

        public Power Get(int id)
        {
            return DB.Powers.Find(id);
        }

        public Dictionary<string, List<PowerBase>> GetByUserID(int userId)
        {

            return DB.Items.Where(e => e.UserId == userId && e.Power.PowerEnum == PowerEnum.Link)
                .Select(e => new PowerBase
                {
                    Name = e.Power.Name,
                    Url = e.Power.Url,
                    Order = e.Power.Order,
                    Glyphicon = e.Power.Glyphicon,
                    SystemClass = e.Power.System.Class,
                    SystemName = e.Power.System.Name
                }).GroupBy(e => e.SystemName).ToDictionary(e => e.Key, e => e.ToList());
        }

        public bool Delete(int id)
        {
            var entry = DB.Powers.Find(id);
            if (entry == null)
            {
                return false;
            }
            DB.Powers.Remove(entry);
            DB.SaveChanges();

            return true;
        }
    }
}
