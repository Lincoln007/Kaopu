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
                    if (contract.Leave >= contract_price[i])
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

        public void Update(List<BillContract> list)
        {
            using (var db = GetDbContext())
            {
                db.BillContracts.AddRange(list);
                db.SaveChanges();
            }
        }
    }
}
