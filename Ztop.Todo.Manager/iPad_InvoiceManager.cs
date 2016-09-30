using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztop.Todo.Model;

namespace Ztop.Todo.Manager
{
    public class iPad_InvoiceManager:ManagerBase
    {
        public int Save(iPadInvoice invoice)
        {
            using (var db = GetDbContext())
            {
                var entry = db.iPad_Invoices.Find(invoice.ID);
                if (entry == null)
                {
                    db.iPad_Invoices.Add(invoice);
                }
                else
                {
                    db.Entry(entry).CurrentValues.SetValues(invoice);
                }
                db.SaveChanges();
                return invoice.ID;
            }
        } 
        public List<iPadInvoice> Get()
        {
            using (var db = GetDbContext())
            {
                var list = db.iPad_Invoices.ToList();
                foreach(var item in list)
                {
                    item.Relations = db.Register_iPads.Where(e => e.RID == item.ID && e.Relation == Relation.Invoice_iPad).ToList();
                }
                return list;
            }
        }
    }
}
