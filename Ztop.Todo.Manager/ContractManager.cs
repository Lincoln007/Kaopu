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
                return db.Contracts.ToList();
            }
        }
    }
}
