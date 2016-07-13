using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztop.Todo.Model;

namespace Ztop.Todo.Manager
{
    public class BillContractManager:ManagerBase
    {
        public List<BillContract> Get(int bill_id,int[] contract_id,double[] contract_price)
        {
            var list = new List<BillContract>();
            for(var i = 0; i < contract_id.Length; i++)
            {
                var contract = Core.ContractManager.Get(contract_id[i]);
                if (contract != null)
                {
                    if (contract_price[i]>0 && contract.Leave >= contract_price[i])
                    {
                        list.Add(new BillContract()
                        {
                            BillID = bill_id,
                            ContractID = contract.ID,
                            Price = contract_price[i]
                        });
                    }
                }
            }
            return list;
        }

        public void AddRange(List<BillContract> list)
        {
            using (var db = GetDbContext())
            {
                db.BillContracts.AddRange(list);
                db.SaveChanges();
            }
        }
        public List<BillContract> GetByBillID(int id)
        {
            using (var db = GetDbContext())
            {
                var list = db.BillContracts.Where(e => e.BillID == id).ToList();
                foreach(var billcontract in list)
                {
                    billcontract.Contract = db.Contracts.FirstOrDefault(e => e.ID == billcontract.ContractID);
                }
                return list;
            }
        }
        public List<BillContract> GetByContractID(int id)
        {
            using (var db = GetDbContext())
            {
                var list = db.BillContracts.Where(e => e.ContractID == id).ToList();
                foreach(var billcontract in list)
                {
                    billcontract.Bill = db.Bills.FirstOrDefault(e => e.ID == billcontract.BillID);
                }
                return list;
            }
        }
    }
}
