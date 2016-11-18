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

        /// <summary>
        /// 作用：添加BillOne
        /// 作者：汪建龙
        /// 编写时间：2016年11月16日16:11:02
        /// </summary>
        /// <param name="billOne"></param>
        /// <returns></returns>
        public int Add(BillOne billOne)
        {
            using (var db = GetDbContext())
            {
                db.BillOnes.Add(billOne);
                db.SaveChanges();
                return billOne.ID;
            }
        }

        /// <summary>
        /// 作用：
        /// 作者：汪建龙
        /// 编写时间：2016年11月16日15:46:10
        /// </summary>
        /// <param name="hid"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public List<string> Input(int hid,List<BillOne> list)
        {
            var errors = new List<string>();
            var preSerialNumber = 1;
            BillOne preEntry = null;
            foreach(var item in list)
            {
                if (preEntry != null)
                {
                    if (!BillClass.CheckLogic(item, preEntry))
                    {
                        errors.Add(string.Format("第{0}个数据：序号前后不一致，应递增，账户余额收支前后不一致；", preSerialNumber));
                    }
                }
                item.HID = hid;
                if (Add(item)==0)
                {
                    errors.Add(string.Format("第{0}个数据：保存到数据库失败；", preSerialNumber));
                }
                preEntry = item;
                preSerialNumber++;
            }
            return errors;
        }

        /// <summary>
        /// 作用：获取Bill_Head 
        /// 作者：汪建龙
        /// 编写时间：2016年11月16日16:16:09
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Bill_Head GetHead(int id)
        {
            using (var db = GetDbContext())
            {
                var entry = db.Bill_Heads.Find(id);
                if (entry != null)
                {
                    entry.Ones = db.BillOnes.Where(e => e.HID == entry.ID).ToList();
                }
                return entry;
            }
        }

        /// <summary>
        /// 作用：获取BillOne
        /// 作者：汪建龙
        /// 编写时间？：2016年11月16日17:01:52
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BillOne GetBillOne(int id)
        {
            using (var db = GetDbContext())
            {
                return db.BillOnes.Find(id);
            }
        }
        /// <summary>
        /// 作用：归类银行账目
        /// 作者：汪建龙
        /// 编写时间：2016年11月17日10:24:40
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cost"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        public BillOne Classify(int id,Cost cost,Category? category)
        {
            using (var db = GetDbContext())
            {
                var entry = db.BillOnes.Find(id);
                if (entry != null)
                {
                    entry.Cost = cost;
                    entry.Category = category;
                    db.SaveChanges();
                    return entry;
                }
            }

            return null;
        }
    }
}
