using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztop.Todo.Common;

namespace Ztop.Todo.Model
{
    public static class BillClass
    {
        public static List<Bill> Analyze(string filePath,int bid,ref List<string> errors)
        {
            var list = new List<Bill>();
            IWorkbook workbook = filePath.OpenExcel();
            if (workbook != null)
            {
                ISheet sheet = workbook.GetSheetAt(0);
                if (sheet != null)
                {
                    IRow row = null;
                    for (var i = 1; i <= sheet.LastRowNum; i++)
                    {
                        row = sheet.GetRow(i);
                        if (row == null)
                        {
                            continue;
                        }
                        var error = string.Empty;
                        var bill = Analyze(row,bid,ref error);
                        if (bill != null)
                        {
                            list.Add(bill);
                        }
                        else
                        {
                            errors.Add(string.Format("在识别Excel中第{0}行中数据，存在错误：{1};", i + 1, error));
                        }
                    }
                }
            }
            return list;
        }


        public static Bill Analyze(IRow row,int bid,ref string error)
        {
            ICell[] cells = new ICell[7];
            for(var i = 1; i < 8; i++)
            {
                cells[i - 1] = row.GetCell(i);
            }

            DateTime currentTime;
            if (cells[0] == null)
            {
                error = string.Format("未填写时间，无法进行录入系统");
                return null;
            }
            else
            {
                try
                {
                    currentTime = cells[0].DateCellValue;
                }
                catch
                {
                    currentTime = DateTime.Parse(cells[0].ToString());
                }
            }
            double income = .0, pay = .0, balance = .0;
            if (cells[1] != null)
            {
                double.TryParse(cells[1].ToString(), out pay);
            }
            if (cells[2] != null)
            {
                double.TryParse(cells[2].ToString(), out income);
            }
            if (cells[3] != null)
            {
                double.TryParse(cells[3].ToString(), out balance);
            }
            if (income > 0 && pay > 0)//当支出收入同时填写时，为错误，返回null
            {
                error = string.Format("同时填写了支出和收入");
                return null;
            }
            var budget = income > 0 ? Budget.Income : Budget.Expense;
            var CostString = cells[6] == null ? string.Empty : cells[6].ToString();
            Cost cost;
            Category? category = null;
            switch (CostString)
            {
                case "实际收入":
                    cost = Cost.RealIncome;
                    break;
                case "还款":
                    cost = Cost.Repayment;
                    break;
                case "保证金退款":
                    cost = Cost.MarginIncome;
                    break;
                case "过账":
                    cost = Cost.Posting;
                    break;
                case "借款":
                    cost = Cost.Load;
                    break;
                case "保证金支出":
                    cost = Cost.MarginPay;
                    break;
                case "备用金":
                    cost = Cost.Petty;
                    break;
                case "日常办公":
                    cost = Cost.RealPay;
                    category = Category.OfficialBussiness;
                    break;
                case "固定资产":
                    cost = Cost.RealPay;
                    category = Category.FixedAsssets;
                    break;
                case "耗材":
                    cost = Cost.RealPay;
                    category = Category.Equipment;
                    break;
                case "交通费":
                    cost = Cost.RealPay;
                    category = Category.Traffic;
                    break;
                case "维修维护":
                    cost = Cost.RealPay;
                    category = Category.Maintenance;
                    break;
                case "邮电费":
                    cost = Cost.RealPay;
                    category = Category.Express;
                    break;
                case "印刷装订":
                    cost = Cost.RealPay;
                    category = Category.Print;
                    break;
                case "招待费":
                    cost = Cost.RealPay;
                    category = Category.Reception;
                    break;
                case "福利费":
                    cost = Cost.RealPay;
                    category = Category.Welfare;
                    break;
                case "评审费":
                    cost = Cost.RealPay;
                    category = Category.Evaluate;
                    break;
                case "招投标费":
                    cost = Cost.RealPay;
                    category = Category.Bidding;
                    break;
                case "财务费":
                    cost = Cost.RealPay;
                    category = Category.Financial;
                    break;
                case "其他":
                    cost = Cost.RealPay;
                    category = Category.Other;
                    break;
                default:
                    error = "类别填写不正确，类别应填写为：实际收入，还款、保证金退款、过账、借款、备用金、保证金支出、日常办公、固定资产、耗材、交通费、维修维护、邮电费、印刷装订、招待费、福利费、评审费、招投标费、财务费和其他";
                    return null;
            }

            if (budget == Budget.Income)
            {
                if (cost == Cost.RealIncome || cost == Cost.Repayment || cost == Cost.MarginIncome)
                {
                    return new Bill()
                    {
                        Time = currentTime,
                        Money = income,
                        Account = cells[4] == null ? string.Empty : cells[4].ToString(),
                        Budget = budget,
                        Cost = cost,
                        Category = category,
                        Summary = cells[5] == null ? string.Empty : cells[5].ToString(),
                        Remark= cells[5] == null ? string.Empty : cells[5].ToString(),
                        BID = bid,
                        Balance=balance
                    };
                }
            }
            else
            {
                if (cost == Cost.Posting || cost == Cost.Load || cost == Cost.MarginPay || cost == Cost.Petty)
                {
                    return new Bill()
                    {
                        Time = currentTime,
                        Money = pay,
                        Account = cells[4] == null ? string.Empty : cells[4].ToString(),
                        Budget = budget,
                        Cost = cost,
                        Category = category,
                        Summary = cells[5] == null ? string.Empty : cells[5].ToString(),
                        Remark= cells[5] == null ? string.Empty : cells[5].ToString(),
                        BID = bid,
                        Balance=balance
                    }; 
                }
            }
            error = "支出、收入与对应的类别不符";
            return null;
        }


        private static Bill Analyze(IRow row,ref string error)
        {
            ICell[] cells = new ICell[7];
            for (var i = 1; i < 8; i++)
            {
                cells[i - 1] = row.GetCell(i);
            }

            DateTime currentTime;
            if (cells[0] == null||string.IsNullOrEmpty(cells[0].ToString()))
            {
                error = string.Format("未填写时间，无法进行录入系统");
                return null;
            }
            else
            {
                try
                {
                    currentTime = cells[0].DateCellValue;
                }
                catch
                {
                    currentTime = DateTime.Parse(cells[0].ToString());
                }
            }
            double income = .0, pay = .0, balance = .0;
            if (cells[1] != null)
            {
                double.TryParse(cells[1].ToString(), out pay);
            }
            if (cells[2] != null)
            {
                double.TryParse(cells[2].ToString(), out income);
            }
            if (cells[3] != null)
            {
                double.TryParse(cells[3].ToString(), out balance);
            }
            if (income > 0 && pay > 0)//当支出收入同时填写时，为错误，返回null
            {
                error = string.Format("同时填写了支出和收入");
                return null;
            }
            var budget = income > 0 ? Budget.Income : Budget.Expense;
            var CostString = cells[6] == null ? string.Empty : cells[6].ToString();
            Cost cost;
            Category? category = null;
            switch (CostString)
            {
                case "实际收入":
                    cost = Cost.RealIncome;
                    break;
                case "还款":
                    cost = Cost.Repayment;
                    break;
                case "保证金退款":
                    cost = Cost.MarginIncome;
                    break;
                case "过账":
                    cost = Cost.Posting;
                    break;
                case "借款":
                    cost = Cost.Load;
                    break;
                case "保证金支出":
                    cost = Cost.MarginPay;
                    break;
                case "备用金":
                    cost = Cost.Petty;
                    break;
                case "日常办公":
                    cost = Cost.RealPay;
                    category = Category.OfficialBussiness;
                    break;
                case "固定资产":
                    cost = Cost.RealPay;
                    category = Category.FixedAsssets;
                    break;
                case "耗材":
                    cost = Cost.RealPay;
                    category = Category.Equipment;
                    break;
                case "交通费":
                    cost = Cost.RealPay;
                    category = Category.Traffic;
                    break;
                case "维修维护":
                    cost = Cost.RealPay;
                    category = Category.Maintenance;
                    break;
                case "邮电费":
                    cost = Cost.RealPay;
                    category = Category.Express;
                    break;
                case "印刷装订":
                    cost = Cost.RealPay;
                    category = Category.Print;
                    break;
                case "招待费":
                    cost = Cost.RealPay;
                    category = Category.Reception;
                    break;
                case "福利费":
                    cost = Cost.RealPay;
                    category = Category.Welfare;
                    break;
                case "评审费":
                    cost = Cost.RealPay;
                    category = Category.Evaluate;
                    break;
                case "招投标费":
                    cost = Cost.RealPay;
                    category = Category.Bidding;
                    break;
                case "财务费":
                    cost = Cost.RealPay;
                    category = Category.Financial;
                    break;
                case "其他":
                    cost = Cost.RealPay;
                    category = Category.Other;
                    break;
                default:
                    error = "类别填写不正确，类别应填写为：实际收入，还款、保证金退款、过账、借款、备用金、保证金支出、日常办公、固定资产、耗材、交通费、维修维护、邮电费、印刷装订、招待费、福利费、评审费、招投标费、财务费和其他";
                    return null;
            }

            if (budget == Budget.Income)
            {
                if (cost == Cost.RealIncome || cost == Cost.Repayment || cost == Cost.MarginIncome)
                {
                    return new Bill()
                    {
                        Time = currentTime,
                        Money = income,
                        Account = cells[4] == null ? string.Empty : cells[4].ToString(),
                        Budget = budget,
                        Cost = cost,
                        Category = category,
                        Summary = cells[5] == null ? string.Empty : cells[5].ToString(),
                        Remark = cells[5] == null ? string.Empty : cells[5].ToString(),
                        Balance = balance
                    };
                }
            }
            else
            {
                if (cost == Cost.Posting || cost == Cost.Load || cost == Cost.MarginPay || cost == Cost.Petty||cost==Cost.RealPay)
                {
                    return new Bill()
                    {
                        Time = currentTime,
                        Money = pay,
                        Account = cells[4] == null ? string.Empty : cells[4].ToString(),
                        Budget = budget,
                        Cost = cost,
                        Category = category,
                        Summary = cells[5] == null ? string.Empty : cells[5].ToString(),
                        Remark = cells[5] == null ? string.Empty : cells[5].ToString(),
                        Balance = balance
                    };
                }
            }
            error = "支出、收入与对应的类别不符";
            return null;
        }

        /// <summary>
        ///作用： 银行对账导入的表头
        ///作者：汪建龙
        ///编写时间：2016年11月12日17:25:21
        /// </summary>
        private static string[] CurrentHead = { "序号", "交易日期", "交易时间", "凭证号", "借方金额", "贷方金额", "余额", "对方账号", "对方户名", "摘要", "备注" };
        /// <summary>
        /// 作用：验证当前行是否为表头  是：true，不是：false
        /// 作者：汪建龙
        /// 编写时间：2016年11月12日17:26:14
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private static bool CheckHead(IRow row)
        {
            if (row == null)
            {
                return false;
            }
            ICell cell = null;
            for(var i = 0; i < CurrentHead.Length; i++)
            {
                cell = row.GetCell(i);
                if (cell == null)
                {
                    return false;
                }
                var str = cell.ToString();
                if (str.ToUpper() != CurrentHead[i].ToUpper())
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 作用：将20161101这样的时间字符串转换成DateTime类型
        /// 作者：汪建龙
        /// 编写时间：2016年11月12日20:04:56
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private static DateTime? GetDateTime(string date)
        {
            if (string.IsNullOrEmpty(date) || date.Length != 8)
            {
                return null;
            }
            var year = date.Substring(0, 4);
            var month = date.Substring(4, 2);
            var data = date.Substring(6);
            DateTime time;
            if(DateTime.TryParse(string.Format("{0}-{1}-{2}", year, month, data), out time))
            {
                return time;
            }
            return null;
        }
        /// <summary>
        /// 作用：分析获得BillOne对象实例
        /// 作者：汪建龙
        /// 编写时间：2016年11月12日17:30:26
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private static BillOne Analyze(IRow row)
        {
            if (row == null)
            {
                return null;
            }
            var array = new string[CurrentHead.Length];
            for(var i = 0; i < CurrentHead.Length; i++)
            {
                var cell = row.GetCell(i);
                if (cell == null)
                {
                    continue;
                }
                array[i] = cell.ToString();
            }

            var a = 0;
            var b = .0;
            var entry = new BillOne {
                SerialNumber = int.TryParse(array[0], out a) ? a : 0,
                Date = array[1],
                Time = GetDateTime(array[1]),
                Voucher = array[3],
                Balance = double.TryParse(array[6], out b) ? b : .0,
                CounterPart=array[7],
                Account=array[8],
                Summary=array[9],
                Remark=array[10]
            };
      
            if (string.IsNullOrEmpty(array[4]))
            {
                if (!string.IsNullOrEmpty(array[5]))
                {
                    entry.Budget = Budget.Income;
                    entry.Money = double.TryParse(array[5], out b) ? b : .0;
                }
            }
            else
            {
                entry.Budget = Budget.Expense;
                entry.Money = double.TryParse(array[4], out b) ? b : .0;
            }

            return entry;

            
        }

        /// <summary>
        /// 作用：比较两个对账记录逻辑
        /// 作者：汪建龙
        /// 编写时间：2016年11月16日15:40:58
        /// </summary>
        /// <param name="billOne"></param>
        /// <param name="pre"></param>
        /// <returns></returns>
        public static bool CheckLogic(BillOne billOne,BillOne pre)
        {
            if (billOne == null || pre == null)
            {
                return false;
            }

            if (billOne.SerialNumber == (pre.SerialNumber+1))//验证序号前后是否递增
            {
                //验证账户余额 前后收支一致
                if (billOne.Budget == Budget.Income && Math.Abs(pre.Balance + billOne.Money - billOne.Balance) < 0.01)//当收入时，上一笔余额+当前金额=当前余额
                {
                    return true;
                }else if (billOne.Budget == Budget.Expense && Math.Abs(pre.Balance - billOne.Money - billOne.Balance) < 0.01)//当支出时，上一笔余额-当前金额=当前余额
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 作用：验证该笔账单是否是时间范围内
        /// 作者：汪建龙
        /// 编写时间：2016年11月22日10:03:06
        /// </summary>
        /// <param name="billone"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public static bool CheckTime(BillOne billone,int year,int month)
        {
            if (billone == null || year == 0 || month == 0)
            {
                return false;
            }
            if (billone.Time.HasValue)
            {
                if (billone.Time.Value.Year == year && billone.Time.Value.Month == month)
                {
                    return true;
                }

            }
            return false;

        }

        /// <summary>
        /// 作用：分析Excel文件，获取银行信息
        /// 作者：汪建龙
        /// 编写时间：2016年11月12日20:57:36
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        public static List<BillOne> AnalyzeExcel(string filePath,ref List<string> errors)
        {
            var list = new List<BillOne>();
            var workbook = filePath.OpenExcel();
            if (workbook == null)
            {
                errors.Add(string.Format("打开Excel失败"));
                return null;
            }
            var sheet = workbook.GetSheetAt(0);
            if (sheet == null)
            {
                errors.Add("未获取Sheet信息");
                return null;
            }
            IRow row = null;
            bool start = false;
            for(var i = 0; i <= sheet.LastRowNum; i++)
            {
                row = sheet.GetRow(i);
                if (row == null)
                {
                    errors.Add(string.Format("无法读取第{0}行的数据", i + 1));
                    continue;
                }
                if (!start)
                {
                    var isHead = CheckHead(row);
                    start = isHead;
                    if (isHead)
                    {
                        continue;
                    }
                }

                if (start)
                {
                    var entry = Analyze(row);
                    if (entry != null)
                    {
                        list.Add(entry);
                    }
                    else
                    {
                        errors.Add(string.Format("Excel第{0}行：未分析到数据", i + 1));
                    }
                }
            }
            return list;

        }

        public static List<Bill> Analyze(string filePath,ref List<string> errors)
        {
            var list = new List<Bill>();
            IWorkbook workbook = filePath.OpenExcel();
            Bill temp = null;
            if (workbook != null)
            {
                ISheet sheet = workbook.GetSheetAt(0);
                if (sheet != null)
                {
                    IRow row = null;
                    for (var i = 1; i <= sheet.LastRowNum; i++)
                    {
                        row = sheet.GetRow(i);
                        if (row == null)
                        {
                            continue;
                        }
                        var error = string.Empty;
                        
                        var bill = Analyze(row, ref error);
                        if (bill != null)
                        {
                            list.Add(bill);
                            if (temp != null)
                            {
                                var sum = bill.Budget == Budget.Income ? bill.Balance - bill.Money : bill.Balance + bill.Money;
                                if (Math.Abs(sum - temp.Balance) > 0.01)
                                {
                                    errors.Add(string.Format("第{0}行的账户余额收支不正确", i + 1));
                                }
                            }
                            temp = bill;
                        }
                        else
                        {
                            errors.Add(string.Format("在识别Excel中第{0}行中数据，存在错误：{1};", i + 1, error));
                        }
                    }
                }
            }
            return list;
        }
    }
}
