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
        public static List<Bill> Analyze(string filePath,int bid)
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
                        var bill = Analyze(row,bid);
                        if (bill != null)
                        {
                            list.Add(bill);
                        }
                    }
                }
            }
            return list;
        }


        public static Bill Analyze(IRow row,int bid)
        {
            ICell[] cells = new ICell[7];
            for(var i = 1; i < 8; i++)
            {
                cells[i - 1] = row.GetCell(i);
            }

            DateTime currentTime;
            if (cells[0] == null)
            {
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

            return null;
        }
    }
}
