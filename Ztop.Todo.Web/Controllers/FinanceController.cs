using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ztop.Todo.Common;
using Ztop.Todo.Model;

namespace Ztop.Todo.Web.Controllers
{
    public class FinanceController : ControllerBase
    {
        // GET: Finance
        public ActionResult Index()
        {
            ViewBag.List = Core.ContractManager.Get();
            ViewBag.Bills = Core.BillManager.Search();
            ViewBag.Invoices = Core.InvoiceManager.Search(new InvoiceParameter() { Status = InvoiceState.Have, Instance = false });
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


        /// <summary>
        /// 创建到账信息
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateEntry()
        {
            var dict = Core.InvoiceManager.Search(new InvoiceParameter() { Status=InvoiceState.Have,Instance=true}).Where(e=>e.Recevied!=Recevied.ALL).GroupBy(e => e.Contract.Name).ToDictionary(e => e.Key, e => e.ToList());
            ViewBag.Invoices = dict;
            return View();
        }

        /// <summary>
        /// 删除开具发票申请
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeleteInvoice(int id)
        {
            return Core.InvoiceManager.Delete(id) ? SuccessJsonResult() : ErrorJsonResult("删除失败！可能已经通过财务审核，请刷新");
        }

        [HttpPost]
        public ActionResult EditInvoice(int id,ZtopCompany ztopcompany,string othersidecompany,double money,string content)
        {
            return Core.InvoiceManager.Edit(id, ztopcompany, othersidecompany, money, content) ? SuccessJsonResult() : ErrorJsonResult("编辑失败！可能已经用过财务审核，请刷新查看！");
        }

        [HttpPost]
        public ActionResult SaveInvoiceBill(Bill bill)
        {
            bill.Budget = Budget.Income;
            bill.Coding = DateTime.Now.Ticks.ToString();
            bill.Cost = Cost.RealIncome;
            bill.Summary = string.Format("{0}录入的到账信息", Identity.Name);
            var bid = Core.BillManager.Save(bill);
            var list = Core.InvoiceBillManager.Get(HttpContext, bid);
            if (Math.Abs(bill.Money - list.Sum(e => e.Price)) > 0.01)
            {
                return ErrorJsonResult("请核对关联发票金额");
            }
            Core.InvoiceBillManager.Save(list);

            return Core.InvoiceManager.UpdateRecevied(list.Select(e=>e.IID).ToList()) ? SuccessJsonResult() : ErrorJsonResult("更新到账情况错误，错误如：未找到发票信息，或者系统参数错误");
        }

        public ActionResult InvoiceSearch(string status=null,string recevied=null,string ztopcompany=null, string department=null,string time=null,string otherside=null,double? minMoney=null,double? maxMoney=null, int page=1)
        {
            var parameter = new InvoiceParameter()
            {
                Department = department,
                Time = time,
                OtherSide = otherside,
                Page = new PageParameter(page, 20),
                MinMoney=minMoney,
                MaxMoney=maxMoney,
                Instance=true
            };
            if (!string.IsNullOrEmpty(status))
            {
                parameter.Status = EnumHelper.GetEnum<InvoiceState>(status);
            }
            if (!string.IsNullOrEmpty(recevied))
            {
                parameter.Recevied = EnumHelper.GetEnum<Recevied>(recevied);
            }
            if (!string.IsNullOrEmpty(ztopcompany))
            {
                parameter.ZtopCompany = EnumHelper.GetEnum<ZtopCompany>(ztopcompany);
            }
            ViewBag.Results = Core.InvoiceManager.Search(parameter);
            ViewBag.Department = Core.UserManager.GetUserGroups().Select(e => e.Name).ToList();
            ViewBag.Parameter = parameter;
            return View();
        }

        /// <summary>
        /// 查看已开发票
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DetailInvoice(int id)
        {
            ViewBag.Invoice = Core.InvoiceManager.Get(id);
            return View();
        }

        public ActionResult ContractSearch()
        {
            return View(); 
        }

    }
}