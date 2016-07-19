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

       
        public List<Sheet> GetSheetByVerify(string name)
        {
            using (var db = GetDbContext())
            {
                var list = new List<Sheet>();
                var query = db.Verifys.Where(e => e.Position == Position.Check && e.Name == name).AsQueryable();
                if (query.Count() > 0)
                {
                    var result = query.GroupBy(e => e.SID).Select(e => e.Key).ToList();
                    foreach(var sid in result)
                    {
                        var sheet = db.Sheets.Find(sid);
                        if (sheet != null)
                        {
                            //sheet.SerialNumber = db.SerialNumbers.FirstOrDefault(e => e.SID == sheet.ID);
                            list.Add(sheet);
                        }
                    }
                }
                return list;
            }
        }

        public List<Sheet> GetSheetByVerify(SheetVerifyParameter parameter)
        {
            var checks = GetSheetByVerify(parameter.Checker);
            var query = checks.AsQueryable();
            if (!string.IsNullOrEmpty(parameter.Coding))
            {
                query = query.Where(e => e.PrintNumber.Contains(parameter.Coding));
               // query = query.Where(e => e.SerialNumber.Coding.Contains(parameter.Coding));
            }

            if (parameter.MinMoney.HasValue&& parameter.MinMoney.Value > 0)
            {
                query = query.Where(e => e.Money >= parameter.MinMoney.Value);
            }
            if (parameter.MaxMoney.HasValue&& parameter.MaxMoney.Value > 0)
            {
                query = query.Where(e => e.Money <= parameter.MaxMoney.Value);
            }
            if (!string.IsNullOrEmpty(parameter.Creater))
            {
                query = query.Where(e => e.Name.Contains(parameter.Creater));
            }
            if (!string.IsNullOrEmpty(parameter.Time))
            {
                var currentTime = DateTime.Now;
                switch (parameter.Time)
                {
                    case "一周内":
                        currentTime = currentTime.AddDays(7);
                        break;
                    case "一个月内":
                        currentTime = currentTime.AddMonths(1);
                        break;
                    case "半年内":
                        currentTime = currentTime.AddMonths(6);
                        break;
                    case "一年内":
                        currentTime = currentTime.AddYears(1);
                        break;
                }
                query = query.Where(e => e.Time <= currentTime);
            }
            if (parameter.Order == Order.Time)
            {
                query = query.OrderByDescending(e => e.Time);
            }
            else
            {
                query = query.OrderByDescending(e => e.Money);
            }
            query = query.SetPage(parameter.Page);
            return query.ToList();
        }

        public Verify Get(int id)
        {
            using (var db = GetDbContext())
            {
                return db.Verifys.FirstOrDefault(e => e.ID == id);
            }
        }
        
    }
}
