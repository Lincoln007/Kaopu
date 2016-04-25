using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztop.Todo.Model;

namespace Ztop.Todo.Manager
{
    public class VerifyManager:ManagerBase
    {
      
        /// <summary>
        /// 更新 当前数据库中存在 那就更新 不存在添加
        /// </summary>
        /// <param name="verify"></param>
        public void Update(Verify verify)
        {
            using (var db = GetDbContext())
            {
                var entry = db.Verifys.OrderByDescending(e=>e.ID).FirstOrDefault(e => e.Name == verify.Name && e.SID == verify.SID && e.Step == verify.Step);
                if (entry != null)
                {
                    verify.ID = entry.ID;
                    db.Entry(entry).CurrentValues.SetValues(verify);
                }
                else
                {
                    db.Verifys.Add(verify);
                }
                db.SaveChanges();
            }
        }


        public void Save(Verify verify)
        {
            using (var db = GetDbContext())
            {
                db.Verifys.Add(verify);
                db.SaveChanges();
            }
        }
        public List<int> GetSheetID(string name)
        {
            using (var db = GetDbContext())
            {
                return db.Verifys.Where(e => e.Name == name).GroupBy(e => e.SID).Select(g => g.Key).ToList();
            }
        }
        
    }
}
