using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztop.Todo.Model;

namespace Ztop.Todo.Manager
{
    public class Bill_OneManager:ManagerBase
    {

        /// <summary>
        /// 作用：获取银行对账头信息
        /// 作者：汪建龙
        /// 编写时间：2016年11月13日15:27:20
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        public Bill_Head GetBillHead(int year,int month,Company company)
        {
            using (var db = GetDbContext())
            {
                return db.Bill_Heads.FirstOrDefault(e => e.Year == year && e.Month == month && e.Company == company);
            }
        }

        /// <summary>
        /// 作用：添加银行对账头记录
        /// 作者：汪建龙
        /// 编写时间：2016年11月13日15:39:13
        /// </summary>
        /// <param name="head"></param>
        /// <returns></returns>
        public int Add(Bill_Head head)
        {
            using (var db = GetDbContext())
            {
                var entry = db.Bill_Heads.FirstOrDefault(e => e.Year == head.Year && e.Month == head.Month && e.Company == head.Company);
                if (entry == null)
                {
                    db.Bill_Heads.Add(head);
                    db.SaveChanges();
                    return head.ID;
                }
                else
                {
                    return entry.ID;
                }
            }
        }

        public void Input(int year,int month,Company company,List<BillOne> list)
        {
            var head = GetBillHead(year, month, company);
            var hid = head != null ? head.ID : Add(new Bill_Head { Year = year, Month = month, Company = company });
            var errors = new List<string>();
            var preSerialNumber = 1;
            foreach(var item in list)
            {


            }
        }
    }
}
