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
        public void Save(Sheet sheet)
        {
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
                db.Substances.AddRange(sheet.Substances.OrderBy(e => e.ID).Select(e => new Substancs
                {
                    Category = e.Category,
                    Details = e.Details,
                    Price = e.Price,
                    SID = sheet.ID
                }));
                db.SaveChanges();

            }
        }

        public List<Sheet> GetSheets(SheetQueryParameter parameter)
        {
            using (var db = GetDbContext())
            {
                var query = db.Sheets.AsQueryable();
                if (!string.IsNullOrEmpty(parameter.Name))
                {
                    query = query.Where(e => e.Name == parameter.Name);
                }

                return query.ToList();
            }
        }
        public int GetNumberExt(string number)
        {
            using (var db = GetDbContext())
            {
                var entry = db.Sheets.OrderByDescending(e => e.ID).FirstOrDefault(e => e.Number == number);
                return entry == null ? 1 : ++entry.NumberExt;
            }
        }
    }
}
