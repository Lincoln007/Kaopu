using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            if (Exist(contract))
            {
                throw new Exception(string.Format("当前系统存在合同编号：{0} 合同名称：{1}", contract.Coding, contract.Name));
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
                if (parameter.Archived.HasValue)
                {
                    query = query.Where(e => e.Archived == parameter.Archived.Value);
                }
                if (!string.IsNullOrEmpty(parameter.Name))
                {
                    query = query.Where(e => e.Name.Contains(parameter.Name));
                }
                if (!string.IsNullOrEmpty(parameter.OtherSide))
                {
                    query = query.Where(e => e.Company.Contains(parameter.OtherSide));
                }
                if (parameter.ZtopCompany.HasValue)
                {
                    query = query.Where(e => e.ZtopCompany == parameter.ZtopCompany.Value);
                }
                query = query.OrderByDescending(e=>e.Coding).SetPage(parameter.Page);
                return query.ToList();
            }
        }
    }
}
