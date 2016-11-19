using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Ztop.Todo.Common;
using Ztop.Todo.Model;

namespace Ztop.Todo.Manager
{
    public class BillManager:ManagerBase
    {
        private static string _uploadDir;
        static BillManager()
        {
            _uploadDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["upload_floder"] ?? "upload_files");
        }
        public List<Bill> GetBills(int bid,string[] coding,DateTime[] time,Budget[] budget,double[] money,string[] account,string[] summary,Cost[] cost,Category[] category)
        {
            if (coding == null)
            {
                throw new ArgumentException("参数错误");
            }
            var count = coding.Count();
            
            var list = new List<Bill>();
            var j = 0;
            for(var i = 0; i < count; i++)
            {
                var bill = new Bill()
                {
                    Time = time[i],
                    Money = money[i],
                    Account = account[i],
                    Budget = budget[i],
                    Summary = summary[i],
                    Remark=summary[i],
                    Cost = cost[i],
                    BID = bid
                };

                if (bill.Cost == Cost.RealPay)
                {
                    bill.Category = category[j++];
                }

                list.Add(bill);
            }
            return list;
        }

        public List<Bill> GetBills(int bid,DateTime[] time,double[] income,double[] pay,double[] balance,string[] account,string[] summary,Cost[] cost,Category[] category)
        {
            var list = new List<Bill>();
            if (time == null)
            {
                throw new ArgumentException("参数错误！");
            }
            var count = time.Count();
            var j = 0;
            for(var i = 0; i < count; i++)
            {
                if (time[i].Year == 1)
                {
                    continue;
                }
                var budget = income[i] > 0 ? Budget.Income : Budget.Expense;
                var bill = new Bill()
                {
                    Time = time[i],
                    Money = budget == Budget.Income ? income[i] : pay[i],
                    Balance = balance[i],
                    Account = account[i],
                    Budget = budget,
                    Summary = summary[i],
                    Remark=summary[i],
                    Cost = cost[i],
                    BID = bid
                };
                if (bill.Cost == Cost.RealPay)
                {
                    bill.Category = category[j++];
                }
                list.Add(bill);
            }
            return list;
        }

        public bool Exist(int year,int month,Company company)
        {
            using (var db = GetDbContext())
            {
                var entry = db.Banks.FirstOrDefault(e => e.Year == year && e.Company == company && e.Month == month);
                return entry != null;
            }
        }
        
        public Bank Get(int year,int month,Company company)
        {
            using (var db = GetDbContext())
            {
                return db.Banks.FirstOrDefault(e => e.Year == year && e.Company == company && e.Month == month);
            }
        }

        public Bank GetBank(int year,int month,Company company)
        {
            using (var db = GetDbContext())
            {
                var entry = db.Banks.FirstOrDefault(e => e.Year == year && e.Company == company && e.Month == month);
                if (entry == null)
                {
                    entry = new Bank() { Year = year, Month = month, Company = company };
                    db.Banks.Add(entry);
                    db.SaveChanges();
                }
                return entry;
            }
        }

        public Bank GetBank(int id)
        {
            if (id == 0)
            {
                return null;
            }
            using (var db = GetDbContext())
            {
                var entry = db.Banks.Find(id);
                if (entry != null)
                {
                    entry.Bills = db.Bills.Where(e => e.BID == entry.ID).ToList();
                }
                return entry;
            }
        }
        public Bank GetAllModelBank(int year,int month,Company company)
        {
            using (var db = GetDbContext())
            {
                var entry = db.Banks.FirstOrDefault(e => e.Year == year && e.Month == month && e.Company == company);
                if (entry != null)
                {
                    entry.Bills = db.Bills.Where(e => e.BID == entry.ID).ToList();
                }
                return entry;
            }
        }
        public void UpDateBills(List<Bill> list,int bid)
        {
            using (var db = GetDbContext())
            {
                var older = db.Bills.Where(e => e.BID == bid).ToList();
                if (older != null)
                {
                    db.Bills.RemoveRange(older);
                    db.SaveChanges();
                }
                db.Bills.AddRange(list);
                db.SaveChanges();
            }
        }

        public List<string> GetYearMonth()
        {
            using (var db = GetDbContext())
            {
                return db.Banks.ToList().Select(e=>e.YearMonth).Distinct().ToList();
            }
        }

        public IWorkbook GetWorkbook(int year,int month,Company company)
        {
            var bank = GetAllModelBank(year, month, company);
            if (bank == null)
            {
                throw new Exception(string.Format("未找到{0}年{1}月的{2}公司银行对账单清单列表", year, month, company.GetDescription()));
            }

            IWorkbook workbook = ExcelClass.GetAbsolutePath(System.Configuration.ConfigurationManager.AppSettings["BANK"].ToString()).OpenExcel();
            if (workbook != null)
            {
                ISheet sheet = workbook.GetSheetAt(0);
                if (sheet != null)
                {
                    var title = string.Format("杭州智拓{0}咨询有限公司{1}年{2}月银行对账单", company == Company.Evaluation ? "房地产土地评估" : "土地规划设计", year, month);
                    IRow row = sheet.GetRow(0);
                    if (row == null)
                    {
                        row = sheet.CreateRow(0);
                    }
                    ICell cell = ExcelClass.GetCell(row, 0);
                    cell.SetCellValue(title);
                    if (bank.Bills != null&&bank.Bills.Count!=0)
                    {
                        row = sheet.GetRow(7);
                        ExcelClass.GetCell(row, 0).SetCellValue(bank.Bills.Where(e => e.Budget == Budget.Income).Sum(e => e.Money));
                        ExcelClass.GetCell(row, 1).SetCellValue(bank.Bills.Where(e => e.Budget == Budget.Income && e.Cost == Cost.RealIncome).Sum(e => e.Money));
                        ExcelClass.GetCell(row, 2).SetCellValue(bank.Bills.Where(e => e.Budget == Budget.Income && e.Cost == Cost.Repayment).Sum(e => e.Money));
                        ExcelClass.GetCell(row, 3).SetCellValue(bank.Bills.Where(e => e.Budget == Budget.Income && e.Cost == Cost.MarginIncome).Sum(e => e.Money));
                        ExcelClass.GetCell(row, 4).SetCellValue(bank.Bills.Where(e => e.Budget == Budget.Expense).Sum(e => e.Money));
                        ExcelClass.GetCell(row, 5).SetCellValue(bank.Bills.Where(e => e.Budget == Budget.Expense && e.Cost == Cost.Posting).Sum(e => e.Money));
                        ExcelClass.GetCell(row, 6).SetCellValue(bank.Bills.Where(e => e.Budget == Budget.Expense && e.Cost == Cost.Load).Sum(e => e.Money));
                        ExcelClass.GetCell(row, 7).SetCellValue(bank.Bills.Where(e => e.Budget == Budget.Expense && e.Cost == Cost.MarginPay).Sum(e => e.Money));
                        ExcelClass.GetCell(row, 8).SetCellValue(bank.Bills.Where(e => e.Budget == Budget.Expense && e.Cost == Cost.RealPay).Sum(e => e.Money));
                        ExcelClass.GetCell(row, 9).SetCellValue(bank.Bills.Where(e => e.Budget == Budget.Expense && e.Cost == Cost.Petty).Sum(e => e.Money));

                        var line = 11;
                        var modelRow = sheet.GetRow(line);
                        var serial = 1;
                        foreach(var bill in bank.Bills)
                        {
                            row = sheet.GetRow(line);
                            if (row == null)
                            {
                                row = sheet.CreateRow(line);
                            }
                            line++;
                            cell = ExcelClass.GetCell(row, 0, modelRow);
                            cell.SetCellValue(serial++);
                            ExcelClass.GetCell(row, 1, modelRow).SetCellValue(bill.Time.ToShortDateString());
                            if (bill.Budget == Budget.Income)
                            {
                                ExcelClass.GetCell(row, 2, modelRow);
                                ExcelClass.GetCell(row, 3, modelRow).SetCellValue(bill.Money);
                            }
                            else
                            {
                                ExcelClass.GetCell(row, 2, modelRow).SetCellValue(bill.Money);
                                ExcelClass.GetCell(row, 3, modelRow);
                            }
                            ExcelClass.GetCell(row, 4, modelRow).SetCellValue(bill.Balance);
                            ExcelClass.GetCell(row, 5, modelRow).SetCellValue(bill.Account);
                            ExcelClass.GetCell(row, 6, modelRow).SetCellValue(bill.Summary);
                            var description = bill.Cost.GetDescription();
                            if (bill.Category.HasValue)
                            {
                                description += "-" + bill.Category.Value.GetDescription();
                            }
                            ExcelClass.GetCell(row, 7, modelRow).SetCellValue(description);
                     
                        }
                    }
                   
                }
            }

            return workbook;
        }

        public int Save(Bill bill)
        {
            using (var db = GetDbContext())
            {
                db.Bills.Add(bill);
                db.SaveChanges();
                return bill.ID;
            }
        }

        public List<Bill> Search()
        {
            using (var db = GetDbContext())
            {
                return db.Bills.ToList();
            }
        }

        public string Upload(HttpPostedFileBase file)
        {
            if (file.ContentLength == 0) return string.Empty;
            if (!Directory.Exists(_uploadDir))
            {
                Directory.CreateDirectory(_uploadDir);
            }
            var newFileName = DateTime.Now.Ticks.ToString() + Path.GetExtension(file.FileName);
            var saveFileFullPath = Path.Combine(_uploadDir, newFileName);
            file.SaveAs(saveFileFullPath);
            return saveFileFullPath;
            
        }

        public List<Bill> Search(BillParamter parameter)
        {
            using (var db = GetDbContext())
            {
                var query = db.Bills.AsQueryable();
                if (parameter.MinMoney.HasValue)
                {
                    query = query.Where(e => e.Money >= parameter.MinMoney.Value);
                }
                if (parameter.MaxMoney.HasValue)
                {
                    query = query.Where(e => e.Money <= parameter.MaxMoney.Value);
                }
                if (parameter.StartTime.HasValue)
                {
                    query = query.Where(e => e.Time >= parameter.StartTime.Value);
                }
                if (parameter.EndTime.HasValue)
                {
                    query = query.Where(e => e.Time <= parameter.EndTime.Value);
                }
                if (!string.IsNullOrEmpty(parameter.OtherSide))
                {
                    query = query.Where(e => e.Account.Contains(parameter.OtherSide));
                }
                if (parameter.Association.HasValue)
                {
                    query = query.Where(e => e.Association == parameter.Association.Value);
                }
                if (!string.IsNullOrEmpty(parameter.Remark))
                {
                    query = query.Where(e => e.Remark.Contains(parameter.Remark));
                }
                query = query.OrderBy(e => e.ID).SetPage(parameter.Page);
                return query.ToList();
            }
        }

        public Bill GetBill(int id)
        {
            using (var db = GetDbContext())
            {
                return db.Bills.Find(id);
            }
        }

        public bool UpdateLeave(int bill_id,double sum)
        {
            using (var db = GetDbContext())
            {
                var bill = db.Bills.FirstOrDefault(e => e.ID == bill_id);
                if (bill == null||bill.Leave<sum)
                {
                    return false;
                }
                bill.Leave = bill.Leave - sum;
                bill.Association = bill.Leave > 0 ? (bill.Money > bill.Leave ? Association.Part : Association.Full) : Association.Full;
                db.SaveChanges();
            }
            return true;
        }

        
    }
}
