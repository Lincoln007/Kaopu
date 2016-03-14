using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztop.Todo.Model;

namespace Ztop.Todo.Manager
{
    public class SheetManager:ManagerBase
    {

        public Sheet GetModel(int id)
        {
            if (id == 0) return null;
            using (var db = GetDbContext())
            {
                return db.Sheets.FirstOrDefault(e => e.ID == id);
            }
        }
        public Sheet GetAllModel(int id)
        {
            if (id == 0) return null;
            using (var db = GetDbContext())
            {
                var model = db.Sheets.FirstOrDefault(e => e.ID == id);
                if (model != null)
                {
                    model.SerialNumber = db.SerialNumbers.FirstOrDefault(e => e.SID == model.ID);
                    model.Substances = db.Substances.Where(e => e.SID == id).OrderBy(e=>e.ID).ToList();
                    model.Verifys = db.Verifys.Where(e => e.SID == id).OrderBy(e => e.ID).ToList();
                }
                return model;
            }
        }
        /// <summary>
        /// 用于保存报销单
        /// </summary>
        /// <param name="sheet"></param>
        public void Save(Sheet sheet)
        {
            if (sheet == null) return;
            using (var db = GetDbContext())
            {
                if (sheet.ID == 0)
                {
                    db.Sheets.Add(sheet);
                }
                else
                {
                    var entry = db.Sheets.FirstOrDefault(e => e.ID == sheet.ID);
                    if (entry != null)
                    {
                        db.Entry(entry).CurrentValues.SetValues(sheet);
                    }
                }
                db.SaveChanges();
                if (sheet.Substances != null)
                {
                    db.Substances.AddRange(sheet.Substances.OrderBy(e => e.ID).Select(e => new Substancs
                    {
                        Category = e.Category,
                        Details = e.Details,
                        Price = e.Price,
                        SID = sheet.ID
                    }));
                }
                
                db.SaveChanges();

            }
        }

        private SerialNumber GetSerialNumber(int sid)
        {
            if (sid == 0) return null;
            using (var db = GetDbContext())
            {
                return db.SerialNumbers.FirstOrDefault(e => e.SID == sid);
            }
        }
        private List<Sheet> GetSheets()
        {
            using (var db = GetDbContext())
            {
                return db.Sheets.ToList();
            }
        }

        public List<Sheet> GetSheets(SheetQueryParameter parameter)
        {
            var list = GetSheets();
            foreach(var sheet in list)
            {
                sheet.SerialNumber = GetSerialNumber(sheet.ID);
            }
            var query = list.AsQueryable();
            if (!string.IsNullOrEmpty(parameter.Name))
            {
                query = query.Where(e => e.Name == parameter.Name);
            }
            if (parameter.Deleted.HasValue)
            {
                query = query.Where(e => e.Deleted == parameter.Deleted.Value);
            }
            return query.ToList();
        }
        public void Delete(int id)
        {
            using (var db = GetDbContext())
            {
                var entry = db.Sheets.FirstOrDefault(e => e.ID == id);
                if (entry != null)
                {
                    entry.Deleted = true;
                    db.SaveChanges();
                }
            }
        }

        
    }
}
