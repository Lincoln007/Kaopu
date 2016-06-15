using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztop.Todo.Model;

namespace Ztop.Todo.Manager
{
    public class InvoiceManager:ManagerBase
    {
        public int Save(Invoice invoice)
        {
            if (invoice == null)
            {
                throw new Exception("invoice 为 null");
            }
            using (var db = GetDbContext())
            {
                db.Invoices.Add(invoice);
                db.SaveChanges();
                return invoice.ID;
            }
        }
        public List<Invoice> GetByCID(int cid)
        {
            if (cid == 0)
            {
                return null;
            }
            using (var db = GetDbContext())
            {
                return db.Invoices.Where(e => e.CID == cid).ToList();
            }
        }
        public Invoice Get(int id)
        {
            using (var db = GetDbContext())
            {
                return db.Invoices.Find(id);
            }
        }


        public bool Improve(int id,DateTime fillTime,string number,string remark,InvoiceState state)
        {
            using (var db = GetDbContext())
            {
                var invoice = db.Invoices.Find(id);
                if (invoice == null)
                {
                    return false;
                }
                invoice.FillTime = fillTime;
                invoice.Number = number;
                invoice.Remark = remark;
                invoice.State = state;
                db.SaveChanges();
                return true;
            }
        }
    }
}
