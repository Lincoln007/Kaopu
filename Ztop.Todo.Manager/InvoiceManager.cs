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
                return db.Invoices.Where(e => e.CID == cid&&e.Deleted==false).ToList();
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
        public bool Change(int id,InvoiceState state)
        {
            using (var db = GetDbContext())
            {
                var invoice = db.Invoices.Find(id);
                if (invoice == null)
                {
                    return false;
                }

                invoice.State = state;
                db.SaveChanges();
                return true;
            }
        }

       public List<Invoice> Search()
        {
            using (var db = GetDbContext())
            {
                var list = db.Invoices.ToList();
                foreach(var invoice in list)
                {
                    invoice.Contract = db.Contracts.Find(invoice.CID);
                    invoice.InvoiceBills = db.InvoiceBills.Where(e => e.IID == invoice.ID).ToList();
                }
                return list;
            }
        }

        public bool Delete(int id)
        {
            if (id == 0)
            {
                return false;
            }

            using (var db = GetDbContext())
            {
                var invoice = db.Invoices.Find(id);
                if (invoice == null)
                {
                    return false;
                }
                if (invoice.State.HasValue)
                {
                    return false;
                }
                invoice.Deleted = true;
                db.SaveChanges();
                return true;
            }
        }
        public bool Edit(int id,ZtopCompany ztopcompany,string othersidecompany,double money,string content)
        {
            if (id == 0)
            {
                return false;
            }
            using (var db = GetDbContext())
            {
                var entry = db.Invoices.Find(id);
                if (entry == null||entry.State.HasValue)
                {
                    return false;
                }
                entry.ZtopCompany = ztopcompany;
                entry.OtherSideCompany = othersidecompany;
                entry.Money = money;
                entry.Content = content;
                db.SaveChanges();
                return true;
            }
        }

    }
}
