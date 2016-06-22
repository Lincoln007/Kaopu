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
            return View();
        }

        [HttpPost]
        public ActionResult Save(int year,int month,Company company,string[] Coding,DateTime[] Time,double[] Money,string[] Account,string[] Summary,Budget[] Budget,Cost[] Cost,Category[] Category,bool Edit=false)
        {
            if (Core.BillManager.Exist(year, month, company)&&!Edit)
            {
                return ErrorJsonResult(string.Format("系统中已经存在{0}年{1}月{2}公司的银行对账清单，对有需要更改请前往编辑！", year, month, company.GetDescription()));
            }
            var bank = Core.BillManager.GetBank(year, month, company);
            var bills = Core.BillManager.GetBills(bank.ID, Coding, Time, Budget, Money, Account, Summary,Cost,Category);
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
            var file = HttpContext.Request.Files[0];
            var saveFullFilePath = Core.BillManager.Upload(file);
            return SuccessJsonResult();
        }
    }
}