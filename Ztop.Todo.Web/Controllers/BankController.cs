using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ztop.Todo.Common;
using Ztop.Todo.Model;

namespace Ztop.Todo.Web.Controllers
{
    public class BankController : ControllerBase
    {
        // GET: Bank
        public ActionResult Index()
        {
            ViewBag.Evaluations = Core.BillManager.GetBanks(Company.Evaluation);
            ViewBag.Projections = Core.BillManager.GetBanks(Company.Projection);
            ViewBag.Districts = Core.BillManager.GetYearMonth();
            ViewBag.Sheets = Core.SheetManager.Collect();
            return View();
        }

        public ActionResult Create(Company company)
        {
            ViewBag.Company = company;
            var year = DateTime.Now.Year;
            var month = DateTime.Now.Month-1;
            if (month == 1)
            {
                year--;
                month = 12;
            }
            else
            {
                month--;
            }
            var PreBank = Core.BankManager.GetBank(year, month, company);
            ViewBag.PreBank = PreBank;
            return View();
        }

        [HttpPost]
        public ActionResult Save(int year,int month,Company company,DateTime[] Time,double[] Income,double[] Pay,double[] Balance, string[] Account,string[] Summary,Cost[] Cost,Category[] Category,bool Edit=false)
        {
            if (Core.BillManager.Exist(year, month, company)&&!Edit)
            {
                return ErrorJsonResult(string.Format("系统中已经存在{0}年{1}月{2}公司的银行对账清单，对有需要更改请前往编辑！", year, month, company.GetDescription()));
            }
            var bank = Core.BillManager.GetBank(year, month, company);
            var bills = Core.BillManager.GetBills(bank.ID, Time, Income, Pay, Balance, Account, Summary, Cost, Category);
            //var bills = Core.BillManager.GetBills(bank.ID, Coding, Time, Budget, Money, Account, Summary,Cost,Category);
            try
            {
                Core.BillManager.UpDateBills(bills, bank.ID);
            }
            catch(Exception ex)
            {
                return ErrorJsonResult(ex.ToString());
            }
           
            return SuccessJsonResult(bank);
        }

        public ActionResult SaveOne(int year,int month,Company company,DateTime time,double income,double pay,double balance,string account,string summary,Cost cost,Category category)
        {
            var bank = Core.BillManager.GetBank(year, month, company);
            var id = Core.BillManager.Save(new Bill()
            {
                Time = time,
                Money = income > 0 ? income : pay,
                Account = account,
                Balance = balance,
                Budget = income > 0 ? Budget.Income : Budget.Expense,
                Summary = summary,
                Remark = summary,
                Cost = cost,
                Category = category,
                BID = bank.ID
            });
            return SuccessJsonResult();
        }

        public ActionResult Detail(int id,bool edit=false)
        {
            ViewBag.Bank = Core.BillManager.GetBank(id);
            ViewBag.EditFlag = edit;
            return View();
        }

        public ActionResult Collect(int year,int month)
        {
            ViewBag.Year = year;
            ViewBag.Month = month;
            ViewBag.Evaluation = Core.BillManager.GetAllModelBank(year, month, Company.Evaluation);
            ViewBag.Projection = Core.BillManager.GetAllModelBank(year, month, Company.Projection);
            ViewBag.Cash = Core.SheetManager.Sum(year, month);
            return View();
        }

        public ActionResult Download(int year,int month,Company company)
        {
            IWorkbook workbook = Core.BillManager.GetWorkbook(year, month, company);
            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            ms.Flush();
            byte[] fileContents = ms.ToArray();
            return File(fileContents, "application/ms-excel", string.Format("杭州智拓{0}咨询有限公司{1}年{2}月银行对账单.xls", company == Company.Evaluation ? "房地产土地评估" : "土地规划设计", year, month));
        }

        [HttpPost]
        public ActionResult Upload(int year,int month,Company company)
        {
            if (Core.BillManager.Exist(year, month, company))
            {
                throw new ArgumentException(string.Format("系统中已经存在{0}年{1}月{2}公司的银行对账清单，对有需要更改请前往编辑！", year, month, company.GetDescription()));
            }
            var bank = Core.BillManager.GetBank(year, month, company);
            var file = HttpContext.Request.Files[0];
            var saveFullFilePath = Core.BillManager.Upload(file);
            var errors = new List<string>();
            var bills = BillClass.Analyze(saveFullFilePath, bank.ID,ref errors);
            if (errors.Count > 0)
            {
                throw new ArgumentException(string.Join("\r\n", errors.ToArray()));
            }
            try
            {
                Core.BillManager.UpDateBills(bills, bank.ID);
            }catch(Exception ex)
            {
                throw new ArgumentException(ex.ToString());
            }
            return RedirectToAction("Detail", new { id = bank.ID });
        }

        public ActionResult Gain(int year,int month,Company company)
        {
            if (month == 1)
            {
                year--;
                month = 12;
            }
            else
            {
                month--;
            }
            var bank = Core.BankManager.GetBank(year, month, company);
            return Json(bank, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SetBalance()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SetBalance(int year,int month,double balance,Company company)
        {
            var currrent = Core.BankManager.GetBank(year, month, company);
            if (currrent != null)
            {
                if (currrent.Balance > 0)
                {
                    return ErrorJsonResult(string.Format("当前系统中查询到{0}的{1}年{2}月的账户余额不为零，无法进行设置", company.GetDescription(), year, month));
                }
            }
            Core.BankManager.Save(new Bank { Year = year, Month = month, Company = company, Balance = balance });
            return SuccessJsonResult();
        }

        #region
        public ActionResult InputFile()
        {
            return View();
        }

        public ActionResult Check(int year,int month,Company company)
        {
            var bank = Core.BillManager.Get(year, month, company)??new Bank();
            return Json(bank, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Input(int year, int month, Company company)
        {
            var file = HttpContext.Request.Files[0];
            if (file == null)
            {
                throw new ArgumentException("请上传Excel文件");
            }

            var saveFullFilePath = Core.BillManager.Upload(file);
            var errors = new List<string>();
            var bills = BillClass.Analyze(saveFullFilePath, ref errors);
            ViewBag.Bank = new Bank { Year = year, Month = month, Company = company };
            ViewBag.Bills = bills;
            ViewBag.Errors = errors;
            return View();
        }

        [HttpPost]
        public ActionResult CheckInput(int year,int month,Company company,List<Bill> bills)
        {
            return SuccessJsonResult();
        }

        #endregion
    }
}