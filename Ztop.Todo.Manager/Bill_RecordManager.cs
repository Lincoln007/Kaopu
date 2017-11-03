using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztop.Todo.Common;
using Ztop.Todo.Model;

namespace Ztop.Todo.Manager
{
    public class Bill_RecordManager:ManagerBase
    {
        /// <summary>
        /// 作用：添加和编辑Bill_Record
        /// 作者：汪建龙
        /// 编写时间：2017年1月10日17:02:30
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public int Add(BillRecord record)
        {
            using (var db = GetDbContext())
            {
                var entry = db.Bill_Records.FirstOrDefault(e => e.SerialNumber == record.SerialNumber && e.HID == record.HID);
                if (entry != null)
                {
                    record.ID = entry.ID;
                    if (record.Budget == entry.Budget)//假如当前记录是收入或者支出统一时，一级类和二级类不做修改
                    {
                        record.Cost = entry.Cost;
                        record.RID = entry.RID;
                    }
                    record.Sync = entry.Sync;
                    db.Entry(entry).CurrentValues.SetValues(record);
                }
                else
                {
                    db.Bill_Records.Add(record);
                }

                db.SaveChanges();
                return record.ID;
            }
        }

        /// <summary>
        /// 作用：通过HID  得到列表
        /// 作者：汪建龙
        /// 编写时间：2017年1月10日17:15:01
        /// </summary>
        /// <param name="hid"></param>
        /// <returns></returns>
        public List<BillRecord> GetByHID(int hid)
        {
            using (var db = GetDbContext())
            {
                return db.Bill_Records.Where(e => e.HID == hid).OrderBy(e => e.SerialNumber).ToList();
            }
        }
        /// <summary>
        /// 作用：通过ID 得到
        /// 作者：汪建龙
        /// 编写时间：2017年1月10日17:19:17
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BillRecord Get(int id)
        {
            using (var db = GetDbContext())
            {
                return db.Bill_Records.Find(id);
            }
        }
        /// <summary>
        /// 作用：对账单进行备注
        /// 作者：汪建龙
        /// 编写时间：2017年1月10日17:21:55
        /// </summary>
        /// <param name="id"></param>
        /// <param name="remark2"></param>
        /// <returns></returns>
        public BillRecord Remark(int id,string remark2)
        {
            using (var db = GetDbContext())
            {
                var entry = db.Bill_Records.Find(id);
                if (entry != null)
                {
                    entry.Remark2 = remark2;
                    db.SaveChanges();
                    return entry;
                }
            }
            return null;
        }
        /// <summary>
        /// 作用：对银行账单进行归类
        /// 作者：汪建龙
        /// 编写时间：2017年1月10日17:26:53
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cost"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        public BillRecord Classify(int id,Cost cost,int? rid)
        {
            using (var db = GetDbContext())
            {
                var entry = db.Bill_Records.Find(id);
                if (entry != null)
                {
                    entry.Cost = cost;
                    // entry.Category = category;
                    entry.RID = rid;
                    db.SaveChanges();
                    return entry;
                }
            }
            return null;
        }
        public Dictionary<string, double> Collect(List<BillRecordView> list, Budget budget) 
        {
            list = list.Where(e => e.Budget.Value == budget).ToList();
            var dict = new Dictionary<string, double>();
            var none = list.Where(e => e.Cost == null).ToList();
            if (none.Count > 0)
            {
                dict.Add("无类别", none.Sum(e => e.Money));
            }
            list = list.Where(e => e.Cost.HasValue).ToList();

            if (budget == Budget.Expense)
            {
                foreach (Cost cost in Enum.GetValues(typeof(Cost)))
                {
                    if (((int)cost) >= 3)
                    {
                        if (cost == Cost.RealPay)
                        {
                            var rnone = list.Where(e => e.Cost==cost&& e.RID == null).ToList();
                            if (rnone.Count > 0)
                            {
                                dict.Add(cost.GetDescription()+"无二级类", rnone.Sum(e => e.Money));
                            }
                            var tnames = list.Where(e => e.RID.HasValue).GroupBy(e => e.TName).Select(e => e.Key);
                            foreach(var tname in tnames)
                            {
                                dict.Add(string.Format("{0}-{1}", cost.GetDescription(), tname), list.Where(e => e.RID.HasValue && e.TName == tname).Sum(e => e.Money));
                            }
                            /*等待修改*/
                            //foreach (Category category in Enum.GetValues(typeof(Category)))
                            //{
                            //    dict.Add(string.Format("{0}-{1}", cost.GetDescription(), category.GetDescription()), list.Where(e => e.Category.HasValue && e.Cost.Value == cost && e.Category.Value == category).Sum(e => e.Money));
                            //}
                        }
                        else
                        {
                            dict.Add(cost.GetDescription(), list.Where(e => e.Cost.Value == cost).Sum(e => e.Money));
                        }
                    }
                }
            }
            else
            {
                foreach (Cost cost in Enum.GetValues(typeof(Cost)))
                {
                    if (((int)cost) < 3)
                    {
                        dict.Add(cost.GetDescription(), list.Where(e => e.Cost.Value == cost).Sum(e => e.Money));
                    }

                }
            }

            return dict;
        }
        /// <summary>
        /// 作用：统计
        /// 作者：汪建龙
        /// 编写时间：2017年1月10日17:40:47
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public Dictionary<string,Dictionary<string,double>> Collect(List<BillRecordView> list)
        {
            var result = new Dictionary<string, Dictionary<string, double>>();
            if (list != null)
            {
                var none = list.Where(e => e.Budget == null).ToList();
                if (none.Count > 0)
                {

                }
                list = list.Where(e => e.Budget.HasValue).ToList();
                foreach(Budget budget in Enum.GetValues(typeof(Budget)))
                {
                    result.Add(budget.GetDescription(), Collect(list, budget));
                }
            }
            return result;
        }
        /// <summary>
        /// 作用：获取账单最后一笔交易
        /// 作者：汪建龙
        /// 编写时间：2017年1月10日17:56:55
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public BillRecord GetLast(int year,int month,Company company)
        {
            using (var db = GetDbContext())
            {
                var head = db.Bill_Heads.FirstOrDefault(e => e.Year == year && e.Month == month&&e.Company==company);
                if (head == null)
                {
                    return null;
                }
                else
                {
                    return db.Bill_Records.Where(e => e.HID == head.ID).OrderByDescending(e => e.SerialNumber).FirstOrDefault();
                }
            }
        }
        /// <summary>
        /// 作用：同步
        /// 作者：汪建龙
        /// 编写时间：2017年1月10日19:18:47
        /// </summary>
        /// <param name="list"></param>
        public void Sync(List<BillRecord> list)
        {
            using (var db = GetDbContext())
            {
                foreach(var item in list)
                {
                    var ad = new Bill
                    {
                        Time = item.Time,
                        Money = item.Money,
                        Account = string.IsNullOrEmpty(item.Name)?item.Account:item.Name,
                        Summary = item.Summary,
                        Remark = string.IsNullOrEmpty(item.PostScript)?item.Remark:item.PostScript,
                        Balance = item.Money,
                        Leave = item.Money,
                        HID = item.HID,
                        BBID = item.ID
                    };
                    var current = db.Bill_Records.Find(item.ID);
                    if (current != null && current.Budget == Budget.Income)
                    {
                        current.Sync = true;
                        db.Bills.Add(ad);
                        db.SaveChanges();
                    }
                }
                db.SaveChanges();
            }
        }
        /// <summary>
        /// 作用：获取
        /// 作者：汪建龙
        /// 编写时间：2017年1月10日19:32:09
        /// </summary>
        /// <returns></returns>
        public List<BillRecord> GetSync()
        {
            using (var db = GetDbContext())
            {
                return db.Bill_Records.Where(e => e.Budget == Budget.Income && e.Sync == false).ToList();
            }
        }

    }
}
