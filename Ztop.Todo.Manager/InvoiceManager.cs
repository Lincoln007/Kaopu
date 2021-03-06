﻿using System;
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
            if (string.IsNullOrEmpty(invoice.GroupName))
            {
                invoice.GroupName = Core.UserManager.GetGroupName(invoice.Applicant);
            }
            using (var db = GetDbContext())
            {
                db.Invoices.Add(invoice);
                db.SaveChanges();
                return invoice.ID;
            }
        }
        /// <summary>
        /// 作用：通过合同ID找到所有关联发票信息 cid为0  返回null;
        /// 作者：汪建龙
        /// 编写时间：2016年12月6日09:26:48
        /// </summary>
        /// <param name="cid"></param>
        /// <returns></returns>
        public List<Invoice> GetByCID(int cid)
        {
            if (cid == 0)
            {
                return null;
            }
            using (var db = GetDbContext())
            {
                var query = db.Invoices.Where(e => e.CID == cid && e.Deleted == false).AsQueryable();
                return query.Count() > 0 ? query.ToList() : new List<Invoice>();
            }
        }

        public Invoice Get(int id)
        {
            if (id == 0)
            {
                return null;
            }
            using (var db = GetDbContext())
            {
                return db.Invoices.FirstOrDefault(e => e.ID == id);
            }
        }
        public Invoice GetFullStance(int id)
        {
            if (id == 0)
            {
                return null;
            }
            using (var db = GetDbContext())
            {
                var invoice = db.Invoices.Find(id);
                if (invoice != null)
                {
                    invoice.Contract = db.Contracts.Find(invoice.CID);
                    if (invoice.Contract != null)
                    {
                        invoice.Contract.ContractFiles = db.ContractFiles.Where(e => e.ContractID == invoice.Contract.ID).ToList();
                    }
                }
                return invoice;
            }
        }
        public int Improve(int id,DateTime fillTime,string number,string remark,InvoiceState state)
        {
            using (var db = GetDbContext())
            {
                var invoice = db.Invoices.Find(id);
                if (invoice == null)
                {
                    return -1;
                }
                invoice.FillTime = fillTime;
                invoice.Number = number;
                invoice.Remark = remark;
                invoice.State = state;
                db.SaveChanges();
                return invoice.CID;
            }
        }
        public int Change(int id,InvoiceState state)
        {
            using (var db = GetDbContext())
            {
                var invoice = db.Invoices.Find(id);
                if (invoice == null)
                {
                    return -1;
                }

                invoice.State = state;
                db.SaveChanges();
                return invoice.CID;
            }
        }
        public List<Invoice> Search(InvoiceParameter parameter)
        {
            using (var db = GetDbContext())
            {
                var query = db.Invoices.Where(e=>e.Deleted==false).AsQueryable();
                if (!string.IsNullOrEmpty(parameter.Key))
                {
                    query = query.Where(e => e.GroupName.Contains(parameter.Key) || e.OtherSideCompany.Contains(parameter.Key));
                }
                if (!string.IsNullOrEmpty(parameter.Department))
                {
                    query = query.Where(e => e.GroupName == parameter.Department);
                }
                if (parameter.Status.HasValue)
                {
                    query = query.Where(e => e.State == parameter.Status.Value);
                }
                if (!string.IsNullOrEmpty(parameter.OtherSide))//对方单位
                {
                    query = query.Where(e => e.OtherSideCompany.Contains(parameter.OtherSide));
                }
                if (parameter.MinMoney.HasValue)
                {
                    query = query.Where(e => e.Money >= parameter.MinMoney.Value);
                }
                if (parameter.MaxMoney.HasValue)
                {
                    query = query.Where(e => e.Money <= parameter.MaxMoney.Value);
                }
                if (parameter.ZtopCompany.HasValue)
                {
                    query = query.Where(e => e.ZtopCompany == parameter.ZtopCompany.Value);
                }
                if (parameter.Recevied.HasValue)
                {
                    switch (parameter.Recevied.Value)
                    {
                        case Recevied.ALL:
                            query = query.Where(e => e.BAID > 0);
                            break;
                        case Recevied.None:
                            query = query.Where(e => e.BAID == 0);
                            break;
                    }
                    //query = query.Where(e => e.Recevied == parameter.Recevied.Value);
                }
                if (!string.IsNullOrEmpty(parameter.Time))
                {
                    DateTime compareTime = DateTime.Now;
                    switch (parameter.Time)
                    {
                        case "本周":
                            compareTime = compareTime.AddDays(7);
                            break;
                        case "本月":
                            compareTime = compareTime.AddMonths(1);
                            break;
                        case "本年":
                            compareTime = compareTime.AddYears(1);
                            break;
                    }
                    query = query.Where(e => (DateTime.Compare(compareTime, e.Time) > 0));
                }
               
                query = query.OrderByDescending(e => e.Time);
                query = query.SetPage(parameter.Page);
                if (parameter.Instance)
                {
                    foreach (var invoice in query)
                    {
                        invoice.Contract = db.Contracts.Find(invoice.CID);
                    }
                }
            
                return query.ToList();
            }
        }

        public List<Invoice> Search(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return new List<Invoice>();
            }
            using (var db = GetDbContext())
            {
                return db.Invoices.Where(e => e.GroupName.Contains(key) || e.OtherSideCompany.Contains(key)).ToList();
            }
        }
        /// <summary>
        /// 删除发票记录  只有当发票处于未开票的情况下才可以删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
                if (invoice.State!=InvoiceState.None)
                {
                    return false;
                }
                invoice.Deleted = true;
                db.SaveChanges();
                return true;
            }
        }
        /// <summary>
        /// 编辑发票  只有当发票处于未开票的情况下 才可以编辑
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ztopcompany"></param>
        /// <param name="othersidecompany"></param>
        /// <param name="money"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public bool Edit(int id,ZtopCompany ztopcompany,string othersidecompany,double money,string content)
        {
            if (id == 0)
            {
                return false;
            }
            using (var db = GetDbContext())
            {
                var entry = db.Invoices.Find(id);
                if (entry == null||entry.State!=InvoiceState.None)
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

        public bool Relate(int bid,string iidString)
        {
            if (string.IsNullOrEmpty(iidString))
            {
                return false;
            }
            var a = 0;
            var iids = iidString.Split(',');
            double relateSum = .0;
            var list = new List<int>();
            var cidList = new List<int>();
            using (var db = GetDbContext())
            {
                var billAccount = db.BillAccounts.FirstOrDefault(e => e.ID == bid);
                if (billAccount == null)
                {
                    return false;
                }
                if (billAccount.Leave <= 0)
                {
                    return false;
                }
                foreach (var item in iids)
                {
                    if (int.TryParse(item, out a))
                    {
                        var invoice = db.Invoices.FirstOrDefault(e => e.ID == a);
                        if (invoice != null)
                        {
                            if (invoice.BAID > 0)
                            {
                                continue;
                            }
                            else
                            {
                                list.Add(a);
                                cidList.Add(invoice.CID);
                                relateSum += invoice.Money;
                            }
                        }
                    }
                }
                if (billAccount.Leave >= relateSum)
                {
                    foreach(var index in list)
                    {
                        var invoice = db.Invoices.FirstOrDefault(e => e.ID == index);
                        if (invoice != null)
                        {
                            invoice.BAID = bid;
                            db.SaveChanges();
                        }
                    }
                    billAccount.Leave = billAccount.Leave - relateSum;
                    billAccount.Association = billAccount.Leave > 0 ? (billAccount.Money > billAccount.Leave ? Association.Part : Association.None) : Association.Full;
                    db.SaveChanges();
                }
                else
                {
                    return false;
                }
            }
            cidList = cidList.Distinct().ToList();
            foreach(var index in cidList)
            {
                Core.ContractManager.UpdateRecevied(index);
            }
            return true;
        }
    }
}
