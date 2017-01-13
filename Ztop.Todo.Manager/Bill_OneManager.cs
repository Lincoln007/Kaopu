using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztop.Todo.Common;
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
        /// 作用：添加BillTwo  存在则替换  不存在追加
        /// 作者：汪建龙
        /// 编写时间：2016年12月11日14:34:54
        /// </summary>
        /// <param name="billTwo"></param>
        /// <returns></returns>
        public int Add(BillTwo billTwo)
        {
            if (billTwo != null)
            {
                return Core.Bill_RecordManager.Add(new BillRecord
                {
                    ID = billTwo.ID,
                    SerialNumber = billTwo.SerialNumber,

                    Date = billTwo.Date,
                    Time = billTwo.Time,

                    Budget = billTwo.Budget,
                    Money = billTwo.Money,

                    Balance = billTwo.Balance,
                    Account = billTwo.Account,

                    Summary = billTwo.Summary,
                    Remark = billTwo.Remark,

                    Remark2 = billTwo.Remark2,
                    HID = billTwo.HID,

                    Cost = billTwo.Cost,
                    //Category = billTwo.Category,

                    TimeStamp = billTwo.TimeStamp,
                    CommissionCharge = billTwo.CommissionCharge,

                    Way = billTwo.Way,
                    Bank = billTwo.Bank,

                    Type = billTwo.Type,
                    Address = billTwo.Address,

                    Name = billTwo.Name,
                    PostScript = billTwo.PostScript,
                    Sync = billTwo.Sync
                });
            }
            return 0;
            //using (var db = GetDbContext())
            //{
            //    var entry = db.BillTwos.FirstOrDefault(e => e.SerialNumber == billTwo.SerialNumber && e.HID == billTwo.HID);
            //    if (entry != null)
            //    {
            //        billTwo.ID = entry.ID;
            //        db.Entry(entry).CurrentValues.SetValues(billTwo);
            //    }
            //    else
            //    {
            //        db.BillTwos.Add(billTwo);
            //    }
            //    db.SaveChanges();
            //    return billTwo.ID;
            //}
        }

        /// <summary>
        /// 作用：添加BillOne  存在则替换  不存在追加
        /// 作者：汪建龙
        /// 编写时间：2016年11月16日16:11:02
        /// </summary>
        /// <param name="billOne"></param>
        /// <returns></returns>
        public int Add(BillOne billOne)
        {
            if (billOne != null)
            {
                return Core.Bill_RecordManager.Add(new BillRecord
                {
                    ID = billOne.ID,
                    SerialNumber = billOne.SerialNumber,
                    Date = billOne.Date,
                    Time = billOne.Time,
                    Voucher = billOne.Voucher,
                    Budget = billOne.Budget,
                    Money = billOne.Money,
                    Balance = billOne.Balance,
                    CounterPart = billOne.CounterPart,
                    Account = billOne.Account,
                    Summary = billOne.Summary,
                    Remark = billOne.Remark,
                    Remark2 = billOne.Remark2,
                    Cost = billOne.Cost,
                    //Category = billOne.Category,
                    HID = billOne.HID,
                    Sync = billOne.Sync
                });
            }
            return 0;
            //using (var db = GetDbContext())
            //{
            //    var entry = db.BillOnes.FirstOrDefault(e => e.SerialNumber == billOne.SerialNumber && e.HID == billOne.HID);
            //    if (entry != null)
            //    {
            //        billOne.ID = entry.ID;
            //        db.Entry(entry).CurrentValues.SetValues(billOne);
            //    }
            //    else
            //    {
            //        db.BillOnes.Add(billOne);
            //    }
               
            //    db.SaveChanges();
            //    return billOne.ID;
            //}
        }

        
        /// <summary>
        /// 作用：将规划公司银行对账单导入
        /// 作者：汪建龙
        /// 编写时间：2016年12月11日14:36:41
        /// </summary>
        /// <param name="hid"></param>
        /// <param name="list"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public List<string> Input2(int hid,List<BillTwo> list,int year,int month,Company company)
        {
            var errors = new List<string>();
            var preSerialNumber = 1;
            BillTwo preEntry = null;
            foreach(var item in list)
            {
                var error = string.Empty;
                if (preEntry != null)
                {
                    if (!BillClass.CheckLogic(item, preEntry))
                    {
                        error = "序号前后不一致，应递增或者账户月收支前后不一致;";
                    }
                    if (!BillClass.CheckTime(item, year, month))
                    {
                        error += string.Format("交易日期不在{0}年{1}月,请核对！", year, month);
                    }
                }
                else
                {
                    if (item.SerialNumber == 1)
                    {
                        BillRecord last = null;
                        if (month == 1)
                        {
                            last = Core.Bill_RecordManager.GetLast(year - 1, 12,company);
                        }
                        else
                        {
                            last = Core.Bill_RecordManager.GetLast(year, month - 1,company);
                        }
                        if (last != null)
                        {
                            if (!BillClass.CheckLogic(last, item))
                            {
                                error += string.Format("当前月初第一笔收支与上个月不符，请核对！");
                            }
                        }
                    }
                }
                if (!string.IsNullOrEmpty(error))
                {
                    errors.Add(string.Format("第{0}个数据存在如下错误：{1}", preSerialNumber, error));
                    continue;
                }
                item.HID = hid;
                if (Add(item) == 0)
                {
                    errors.Add(string.Format("第{0}个数据：保存到数据库失败；", preSerialNumber));
                }
                preEntry = item;
                preSerialNumber++;
            }
            return errors;
        }
        /// <summary>
        /// 作用：
        /// 作者：汪建龙
        /// 编写时间：2016年11月16日15:46:10
        /// </summary>
        /// <param name="hid">账单头文件ID</param>
        /// <param name="list">追加的列表</param>
        /// <returns></returns>
        public List<string> Input(int hid,List<BillOne> list,int year,int month,Company company)
        {
            var errors = new List<string>();
            var preSerialNumber = 1;
            BillOne preEntry = null;
            foreach(var item in list)
            {
                var error = string.Empty;
                if (preEntry != null)
                {
                    if (!BillClass.CheckLogic(item, preEntry))
                    {
                        error = "序号前后不一致，应递增，账户余额收支前后不一致；";
                    }
                    if (!BillClass.CheckTime(item, year, month))
                    {
                        error += string.Format("交易日期不在{0}年{1}月,请核对!", year, month);
                    }
                }
                else
                {
                    if (item.SerialNumber == 1)
                    {
                        BillRecord last = null;
                        if (month == 1)
                        {
                            last = Core.Bill_RecordManager.GetLast(year - 1, 12,company);
                        }
                        else
                        {
                            last = Core.Bill_RecordManager.GetLast(year, month - 1,company);
                        }
                        if (last != null)
                        {
                            if (!BillClass.CheckLogic(last, item))
                            {
                                error += string.Format("当前月初第一笔收支与上个月不符，请核对！");
                            }
                        }
                    }
                }
                if (!string.IsNullOrEmpty(error))
                {
                    errors.Add(string.Format("第{0}个数据存在如下错误：{1}", preSerialNumber, error));
                    continue;
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
        /// 修改：追加规划公司银行对账
        /// 修改时间：2016年12月11日14:41:40
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Bill_Head GetHead(int id)
        {
            using (var db = GetDbContext())
            {
                var entry = db.Bill_Heads.Find(id);
                return entry;
            }
        }
        /// <summary>
        /// 作用：获取上个月和下个月信息
        /// 作者：汪建龙
        /// 编写时间：2017年1月12日10:04:35
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public Bill_Head[] GetNearBy(int year,int month,Company company)
        {
            using (var db = GetDbContext())
            {
                var pre = month == 1 ? db.Bill_Heads.FirstOrDefault(e => e.Year == year - 1 && e.Month == 12&&e.Company==company) : db.Bill_Heads.FirstOrDefault(e => e.Year == year && e.Month == month - 1&&e.Company==company);
                var next = month == 12 ? db.Bill_Heads.FirstOrDefault(e => e.Year == year + 1 && e.Month == 1&&e.Company==company) : db.Bill_Heads.FirstOrDefault(e => e.Year == year && e.Month == month + 1&&e.Company==company);
                return new Bill_Head[] { pre, next };
            }
        }

        /// <summary>
        /// 作用：通过HID获取评估公司银行对账单
        /// 作者：汪建龙
        /// 编写时间：2016年12月11日16:11:09
        /// </summary>
        /// <param name="hid"></param>
        /// <returns></returns>
        public List<BillOne> GetBillOneList(int hid)
        {
            using (var db = GetDbContext())
            {
                return db.BillOnes.Where(e => e.HID == hid).OrderBy(e => e.SerialNumber).ToList();
            }
        }

        /// <summary>
        /// 作用：通过HID获取规划公司银行对账单
        /// 作者：汪建龙
        /// 编写时间：2016年12月11日16:13:14
        /// </summary>
        /// <param name="hid"></param>
        /// <returns></returns>
        public List<BillTwo> GetBillTwoList(int hid)
        {
            using (var db = GetDbContext())
            {
                return db.BillTwos.Where(e => e.HID == hid).OrderBy(e => e.SerialNumber).ToList();
            }
        }
        /// <summary>
        /// 作用:获得规划公司账单单条信息
        /// 作者：汪建龙
        /// 编写时间：2016年12月11日20:41:21
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BillTwo GetBillTwo(int id)
        {
            using (var db = GetDbContext())
            {
                return db.BillTwos.Find(id);
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
        /// 作用：归类银行账目----------评估
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
        /// <summary>
        /// 作用：备注银行对账——评估
        /// 作者：汪建龙
        /// 编写时间：2017年1月3日14:04:38
        /// </summary>
        /// <param name="id"></param>
        /// <param name="remark2"></param>
        /// <returns></returns>
        public BillOne Remark(int id,string remark2)
        {
            using (var db = GetDbContext())
            {
                var entry = db.BillOnes.Find(id);
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
        /// 作用：备注银行对账——规划
        /// 作者：汪建龙
        /// 编写时间：2017年1月3日14:49:21
        /// </summary>
        /// <param name="id"></param>
        /// <param name="remark2"></param>
        /// <returns></returns>
        public BillTwo Remark2(int id,string remark2)
        {
            using (var db = GetDbContext())
            {
                var entry = db.BillTwos.Find(id);
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
        /// 作用：归类--------------规划
        /// 作者：汪建龙
        /// 编写时间：2016年12月11日20:46:50
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cost"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        public BillTwo Classify2(int id,Cost cost,Category? category)
        {
            using (var db = GetDbContext())
            {
                var entry = db.BillTwos.Find(id);
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

        /// <summary>
        /// 作用：获取所有账单头
        /// 作者：汪建龙
        /// 编写时间：2016年11月18日17:31:57
        /// </summary>
        /// <returns></returns>
        public List<Bill_Head> GetAllHeads()
        {
            using (var db = GetDbContext())
            {
                return db.Bill_Heads.ToList().OrderByDescending(e=>e.Head).ToList();
            }
        }
        public Dictionary<string,double> Collect<T>(List<T> list,Budget budget)where T :BillBase
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
                foreach(Cost cost in Enum.GetValues(typeof(Cost)))
                {
                    if (((int)cost) >= 3)
                    {
                        if (cost == Cost.RealPay)
                        {
                            foreach (Category category in Enum.GetValues(typeof(Category)))
                            {
                                dict.Add(string.Format("{0}-{1}", cost.GetDescription(), category.GetDescription()), list.Where(e => e.Category.HasValue && e.Cost.Value == cost && e.Category.Value == category).Sum(e => e.Money));
                            }
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
        /// 作用：统计银行对账指定收支情况——评估
        ///
        /// 作者：汪建龙
        /// 编写时间：2016年11月23日11:09:59
        /// </summary>
        /// <param name="list"></param>
        /// <param name="budget"></param>
        /// <returns></returns>
        public Dictionary<string,double> Collect(List<BillOne> list,Budget budget)
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
               foreach(Cost cost in Enum.GetValues(typeof(Cost)))
                {
                    if (((int)cost >= 3))
                    {
                        if (cost == Cost.RealPay)
                        {
                            foreach (Category category in Enum.GetValues(typeof(Category)))
                            {
                                dict.Add(string.Format("{0}-{1}", cost.GetDescription(), category.GetDescription()), list.Where(e =>e.Category.HasValue&& e.Cost.Value == cost && e.Category.Value == category).Sum(e => e.Money));
                            }
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


        ///// <summary>
        ///// 作用：统计银行对账收支情况——评估
        ///// 作者：汪建龙
        ///// 编写时间：2016年11月23日11:11:05
        ///// </summary>
        ///// <param name="list"></param>
        ///// <returns></returns>
        //public Dictionary<string,Dictionary<string,double>> Collect(List<BillOne> list)
        //{
        //    var result = new Dictionary<string, Dictionary<string, double>>();
        //    if (list != null)
        //    {
        //        var none = list.Where(e => e.Budget == null).ToList();
        //        if (none.Count > 0)
        //        {
                    
        //        }
        //        list = list.Where(e => e.Budget.HasValue).ToList();
        //        foreach(Budget budget in Enum.GetValues(typeof(Budget)))
        //        {
        //            result.Add(budget.GetDescription(), Collect(list, budget));
        //        }
        //    }
        //    return result;
        //}
        /// <summary>
        /// 作用：统计银行对账收支情况—
        /// 作者：汪建龙
        /// 编写时间：2016年11月23日11:11:05
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public Dictionary<string, Dictionary<string, double>> Collect<T>(List<T> list)where T:BillBase
        {
            var result = new Dictionary<string, Dictionary<string, double>>();
            if (list != null)
            {
                var none = list.Where(e => e.Budget == null).ToList();
                if (none.Count > 0)
                {

                }
                list = list.Where(e => e.Budget.HasValue).ToList();
                foreach (Budget budget in Enum.GetValues(typeof(Budget)))
                {
                    result.Add(budget.GetDescription(), Collect(list, budget));
                }
            }
            return result;
        }


    }
}
