using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
            return View();
        }

        public ActionResult Create(Company company)
        {
            ViewBag.Company = company;
            return View();
        }

        [HttpPost]
        public ActionResult Save(int year,int month,Company company,string[] Coding,DateTime[] Time,double[] Money,string[] Account,string[] Summary,Budget[] Budget)
        {
            var bank = Core.BillManager.GetBank(year, month, company);
            var bills = Core.BillManager.GetBills(bank.ID, Coding, Time, Budget, Money, Account, Summary);
            try
            {
                Core.BillManager.UpDateBills(bills, bank.ID);
            }
            catch(Exception ex)
            {
                return ErrorJsonResult(ex.ToString());
            }
           
            return SuccessJsonResult();
        }

        public ActionResult Detail(int id)
        {
            ViewBag.Bank = Core.BillManager.GetBank(id);
            return View();
        }

        public ActionResult Collect(int year,int month)
        {
            ViewBag.Year = year;
            ViewBag.Month = month;
            return View();
        }
    }
}