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
                var entry = db.Contracts.FirstOrDefault(e => e.Coding == contract.Coding && e.Name == contract.Name);
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
                //throw new Exception(string.Format("当前系统存在合同编号：{0} 合同名称：{1}", contract.Coding, contract.Name));
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
                    db.Entry(entry).CurrentValues.SetValues(contract);
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
                if (parameter.Status.HasValue||parameter.Recevied.HasValue)//发票开具情况
                {
                    foreach(var item in query)
                    {
                        //已经开具的发票 不包括退票  红票
                        var list = db.Invoices.Where(e => e.CID == item.ID&&e.State==InvoiceState.Have).ToList();
                        if (list == null)
                        {
                            item.Status = ContractState.None;
                        }
                        else
                        {
                            if (list.Count == 0)
                            {
                                item.Status = ContractState.None;
                            }
                            else
                            {
                                if (Math.Abs(item.Money - list.Sum(e => e.Money)) < 0.001)
                                {
                                    item.Status = ContractState.ALL;
                                }
                                else
                                {
                                    item.Status = ContractState.Part;
                                }
                            }
                        }
                    }
                    if (parameter.Status.HasValue)//过滤发票开具情况
                    {
                        query = query.Where(e => e.Status == parameter.Status.Value);
                    }
                    if (parameter.Recevied.HasValue)
                    {

                    }
                    
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
               // query = query.OrderByDescending(e=>e.Coding).SetPage(parameter.Page);
                return query.ToList();
            }
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

       
      

    }
}
