using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ztop.Todo.Model;

namespace Ztop.Todo.Web.Controllers
{
    public class FinanceController : ControllerBase
    {
        // GET: Finance
        public ActionResult Index()
        {
            ViewBag.List = Core.ContractManager.Get();
            return View();
        }

        public ActionResult CreateContract()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SaveContract(Contract contract)
        {
            var id = Core.ContractManager.Save(contract);
            return SuccessJsonResult(id);
        }


        public ActionResult Detail(int id)
        {
            var contract = Core.ContractManager.Get(id);
            if (contract != null)
            {
                contract.Invoices = Core.InvoiceManager.GetByCID(contract.ID);
            }
            ViewBag.Contract = contract;
            return View();
        }

        [HttpPost]
        public ActionResult SaveInvoice(Invoice invoice)
        {
            var id = Core.InvoiceManager.Save(invoice);
            return SuccessJsonResult(invoice.CID);
        }


        /// <summary>
        /// 补充发票信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fillTime"></param>
        /// <param name="number"></param>
        /// <param name="remark"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ImproveInvoice(int id,DateTime fillTime,string number,string remark,InvoiceState state)
        {
            return Core.InvoiceManager.Improve(id, fillTime, number, remark,state) ? SuccessJsonResult() : ErrorJsonResult("补充信息失败");
        }

        /// <summary>
        /// 修改发票状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public ActionResult Change(int id,InvoiceState state)
        {
            return Core.InvoiceManager.Change(id, state)? SuccessJsonResult() : ErrorJsonResult("修改发票状态失败！");
        }


        public ActionResult CreateEntry()
        {
            return View();
        }



    }
}