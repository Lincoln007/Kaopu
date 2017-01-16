using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Ztop.Todo.Model;

namespace Ztop.Todo.Manager
{
    public class ContractManager:ManagerBase
    {
       
        private bool Exist(Contract contract)
        {
            using (var db = GetDbContext())
            {
                var entry = db.Contracts.FirstOrDefault(e => e.Coding == contract.Coding);
                return entry != null;
            }
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="contract"></param>
        /// <returns></returns>
        public int Save(Contract contract)
        {
            if (Exist(contract)&&contract.ID>0)
            {
                Edit(contract);
                return contract.ID;
            }

            using (var db = GetDbContext())
            {
                db.Contracts.Add(contract);
                db.SaveChanges();
                return contract.ID;
            }

        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="contract"></param>
        /// <returns></returns>
        public int Edit(Contract contract)
        {
            using (var db = GetDbContext())
            {
                var entry = db.Contracts.Find(contract.ID);
                if (entry != null)
                {
                    /*部分字段不能修改编辑*/
                    entry.Name = contract.Name;
                    entry.Company = contract.Company;
                    entry.ZtopCompany = contract.ZtopCompany;
                    entry.StartTime = contract.StartTime;
                    entry.EndTime = contract.EndTime;
                    entry.Money = contract.Money;
                    entry.PerformanceBond = contract.PerformanceBond;
                    entry.PayWay = contract.PayWay;
                    entry.Number = contract.Number;
                    db.SaveChanges();
                }
                return contract.ID;
            }
        }

        public bool Delete(int id)
        {
            using (var db = GetDbContext())
            {
                var entry = db.Contracts.Find(id);
                if (entry == null)
                {
                    return false;
                }
                entry.Deleted = true;
                db.SaveChanges();
                /*
                 * 将合同设置为删除的时候，同时也要设置发票为删除状态
                 * 
                 */
                var invoices = db.Invoices.Where(e => e.CID == entry.ID).ToList();
                foreach(var item in invoices)
                {
                    item.Deleted = true;
                }
                db.SaveChanges();
                return true;
            }
        }

        public bool Archive(int id)
        {
            if (id == 0)
            {
                return false;
            }
            using (var db = GetDbContext())
            {
                var entry = db.Contracts.Find(id);
                if (entry == null)
                {
                    return false;
                }
                entry.Archived = true;
                db.SaveChanges();
                return true;
            }
        }
        public Contract Get(int id)
        {
            using (var db = GetDbContext())
            {
                return db.Contracts.Find(id);
            }
        }

        public bool Retake(int id,string filePath)
        {
            if (id == 0) return false;
            using (var db = GetDbContext())
            {
                var entry = db.Contracts.Find(id);
                if (entry == null)
                {
                    return false;
                }
                entry.FilePath = filePath;
                entry.EndTime = DateTime.Now;
                db.SaveChanges();
            }
            return true;
        }
        public List<Contract> Get()
        {
            using (var db = GetDbContext())
            {
                return db.Contracts.Where(e=>e.Deleted==false).ToList();
            }
        }

        public List<Contract> Search(ContractParameter parameter)
        {
            using (var db = GetDbContext())
            {
                var query = db.Contracts.AsQueryable();
                if (!string.IsNullOrEmpty(parameter.UserName))
                {
                    query = query.Where(e => e.Creator == parameter.UserName);
                }
                if (!string.IsNullOrEmpty(parameter.Number))//登记编号
                {
                    query = query.Where(e => e.Number.Contains(parameter.Number));
                }
                if (parameter.Deleted.HasValue)//是否删除
                {
                    query = query.Where(e => e.Deleted == parameter.Deleted.Value);
                }
                if (!string.IsNullOrEmpty(parameter.Name))//项目合同 关键字
                {
                    query = query.Where(e => e.Name.Contains(parameter.Name));
                }
                if (!string.IsNullOrEmpty(parameter.OtherSide))//对方单位 关键字
                {
                    query = query.Where(e => e.Company.Contains(parameter.OtherSide));
                }
                if (parameter.StartTime.HasValue)//时间范围
                {
                    query = query.Where(e => e.StartTime >= parameter.StartTime.Value);
                }
                if (parameter.EndTime.HasValue)
                {
                    query = query.Where(e => e.EndTime <= parameter.EndTime.Value);
                }
                if (parameter.Status.HasValue)
                {
                    query = query.Where(e => e.Status == parameter.Status.Value);
                }
                if (parameter.Recevied.HasValue)
                {
                    query = query.Where(e => e.Recevied == parameter.Recevied.Value);
                }
                if (parameter.MinMoney.HasValue)//金额范围
                {
                    query = query.Where(e => e.Money >= parameter.MinMoney.Value);
                }
                if (parameter.MaxMoney.HasValue)
                {
                    query = query.Where(e => e.Money <= parameter.MaxMoney.Value);
                }
                if (!string.IsNullOrEmpty(parameter.Department))//合同部门
                {
                    query = query.Where(e => e.Department.Contains(parameter.Department));
                }
                if (parameter.Archived.HasValue)
                {
                    query = query.Where(e => e.Archived == parameter.Archived.Value);
                }
                if (parameter.ZtopCompany.HasValue)//所属组织
                {
                    query = query.Where(e => e.ZtopCompany == parameter.ZtopCompany.Value);
                }
                query = query.OrderByDescending(e=>e.Coding).SetPage(parameter.Page);
                return query.ToList();
            }
        }

        public List<Contract> Rebody(List<Contract> list)
        {
            if (list == null || list.Count == 0)
            {
                return list;
            }
            foreach (var item in list)
            {
                item.Invoices = Core.InvoiceManager.GetByCID(item.ID);
                item.BillContracts = Core.BillContractManager.GetByContractID(item.ID);
            }
            return list;
        }
        public bool UpdateState(int id)
        {
            using (var db = GetDbContext())
            {
                var contract = db.Contracts.FirstOrDefault(e=>e.ID==id);
                if (contract == null)
                {
                    return false;
                }
                var invoices = db.Invoices.Where(e=>e.CID==id&&e.State==InvoiceState.Have).ToList();
                if (invoices.Count == 0)
                {
                    contract.Status = ContractState.None;
                }
                else
                {
                    contract.Status = Math.Abs(contract.Money - invoices.Sum(e => e.Money)) < 0.01 ? ContractState.ALL : ContractState.Part;
                }
                db.SaveChanges();
            }
            return true;
        }
        //public bool UpdateRecevied(int id)
        //{
        //    using (var db = GetDbContext())
        //    {
        //        var contract = db.Contracts.FirstOrDefault(e => e.ID == id);
        //        if (contract == null)
        //        {
        //            return false;
        //        }
        //        var invoices = db.Invoices.Where(e => e.CID == id && e.State == InvoiceState.Have && e.BAID > 0).ToList();
        //        if (invoices.Count == 0)
        //        {
        //            contract.Recevied = Recevied.None;
        //        }
        //        else
        //        {
        //            contract.Recevied = Math.Abs(contract.Money - invoices.Sum(e => e.Money)) < 0.01 ? Recevied.ALL : Recevied.Part;
        //        }
        //        db.SaveChanges();
        //    }
        //    return true;
        //}

        public bool UpdateRecevied(int id)
        {
            using (var db = GetDbContext())
            {
                var contract = db.Contracts.Find(id);
                if (contract == null)
                {
                    return false;
                }
                var billContracts = db.BillContracts.Where(e => e.ContractID == id).ToList();
                if (billContracts.Count == 0)
                {
                    contract.Recevied = Recevied.None;
                }
                else
                {
                    contract.Leave = contract.Money - billContracts.Sum(e => e.Price);
                    if (Math.Abs(contract.Leave - 0) < 0.01)
                    {
                        contract.Archived = true;
                    }
                    contract.Recevied = contract.Leave < 0.01 ? Recevied.ALL : Recevied.Part;
                }
                db.SaveChanges();
            }
            return true;
        }

        public List<Contract> GetByIDList(List<int> idList)
        {
            var list = new List<Contract>();
            using (var db = GetDbContext())
            {
                foreach(var id in idList)
                {
                    var entry = db.Contracts.FirstOrDefault(e => e.ID == id);
                    if (entry != null)
                    {
                        list.Add(entry);
                    }
                }
            }
            return list;
        }
        /// <summary>
        /// 作用：通过登记编号获取合同列表
        /// 作者：汪建龙
        /// 编写时间：2017年1月5日15:39:31
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public List<Contract> GetByNumber(string number)
        {
            using (var db = GetDbContext())
            {
                return db.Contracts.Where(e => e.Number == number).ToList();
            }
        }

       
      

    }
}
