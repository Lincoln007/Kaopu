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
            ViewBag.List = Core.ContractManager.Search(new ContractParameter() { Deleted = false, UserName = Identity.Project ? Identity.Name : null });
            ViewBag.Bills = Core.BillManager.Search();
            ViewBag.BillAccount = Core.BillAccountManager.Search(new BillAccountParameter() { Page = new PageParameter(1, 20) });
            ViewBag.Invoices = Core.InvoiceManager.Search(new InvoiceParameter() { Status = InvoiceState.Have, Instance = false });
            return View();
        }

        public ActionResult CreateContract(int id=0)
        {
            if (id > 0)
            {
                var contract = Core.ContractManager.Get(id);
                if (contract != null)
                {
                    contract.ContractFiles = Core.ContractFileManager.GetContractFiles(contract.ID);
                }
                ViewBag.Contract = contract;
            }
            return View();
        }

        [HttpPost]
        public ActionResult SaveContract(Contract contract,int[] articleid,string articlename, int[] leaves)
        {
            if (string.IsNullOrEmpty(contract.Department))
            {
                contract.Department = Core.UserManager.GetGroupName(Identity.Name);
            }
            var id = Core.ContractManager.Save(contract);
            if (leaves != null && leaves.Count() > 0)
            {
                Core.ContractFileManager.Update(leaves, id);
            }
            if (HttpContext.Request.Files.Count > 0)
            {
                Core.ContractFileManager.SaveContractFile(HttpContext, id);
            }
            Core.ContractArticleManager.Update(id, articleid);
            return RedirectToAction("Detail", new { id = id });
        }


        public ActionResult Detail(int id)
        {
            var contract = Core.ContractManager.Get(id);
            if (contract != null)
            {
                contract.Invoices = Core.InvoiceManager.GetByCID(contract.ID);
                contract.ContractFiles = Core.ContractFileManager.GetContractFiles(contract.ID);
                var CAList = Core.ContractArticleManager.GetByContractID(contract.ID);
                if (CAList != null && CAList.Count > 0)
                {
                    contract.Articles = Core.ArticleManager.GetByIDList(CAList.Select(e=>e.ArticleID).ToList());
                }
            }
            ViewBag.Contract = contract;
            return View();
        }

        [HttpPost]
        public ActionResult Edit(Contract contract)
        {
            
            var id = Core.ContractManager.Edit(contract);
            return SuccessJsonResult(id);
        }

        public ActionResult Delete(int id)
        {
            return Core.ContractManager.Delete(id) ? SuccessJsonResult() : ErrorJsonResult("删除合同失败！参数错误或是系统不存在该合同！");
        }

        public ActionResult Archive(int id)
        {
            return Core.ContractManager.Archive(id) ? SuccessJsonResult(id) : ErrorJsonResult("归档合同失败！参数错误或是系统中未找到该合同！");
        }

        public ActionResult CreateInvoice()
        {
            ViewBag.Contract = Core.ContractManager.Search(new ContractParameter() { Archived = false,Deleted=false });
            return View();
        }

        [HttpPost]
        public ActionResult SaveInvoice(Invoice invoice)
        {
            var id = Core.InvoiceManager.Save(invoice);
            return SuccessJsonResult(invoice);
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
            var cid = Core.InvoiceManager.Improve(id, fillTime, number, remark, state);
            if (cid > 0)
            {
                return Core.ContractManager.UpdateState(cid) ? SuccessJsonResult() : ErrorJsonResult("更新合同发票开具情况失败！");
            }
            else
            {
                return ErrorJsonResult("补充信息失败！");
            }
        }

        /// <summary>
        /// 修改发票状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public ActionResult Change(int id,InvoiceState state)
        {
            var cid = Core.InvoiceManager.Change(id, state);
            if (cid > 0)
            {
                return Core.ContractManager.UpdateState(cid) ? SuccessJsonResult() : ErrorJsonResult("更新合同发票开具情况失败！");
            }
            else
            {
                return ErrorJsonResult("修改发票状态失败！");
            }
            
        }


        /// <summary>
        /// 创建到账信息
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateEntry()
        {
            var dict = Core.InvoiceManager.Search(new InvoiceParameter() { Status=InvoiceState.Have,Instance=true}).Where(e=>e.BAID>0).GroupBy(e => e.Contract.Name).ToDictionary(e => e.Key, e => e.ToList());
            ViewBag.Invoices = dict;
            return View();
        }
        public ActionResult SearchContract(string name,string company,DateTime? startTime,DateTime? endTime)
        {
            var list = Core.ContractManager.Search(new ContractParameter() { Name = name, OtherSide = company, StartTime = startTime, EndTime = endTime,Archived=false,Deleted=false });
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        public ActionResult SearchInvoice(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return Json(new List<Invoice>(), JsonRequestBehavior.AllowGet);
            }
            else
            {
                var list = Core.InvoiceManager.Search(new InvoiceParameter() { Key = key, Instance = true });
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            
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
        public ActionResult SaveBillAccount(Bill bill)
        {
            bill.Budget = Budget.Income;
            bill.Leave = bill.Money;
            bill.Summary = bill.Remark;
            Core.BillManager.Save(bill);
            return SuccessJsonResult();
        }


        [HttpGet]
        public ActionResult Relate(int id)
        {
            ViewBag.BillAccount = Core.BillAccountManager.Get(id);
            return View();
        }

        public ActionResult OnAccount()
        {
            ViewBag.Results = Core.BillAccountManager.Search(new BillAccountParameter() { Association = Association.None });
            ViewBag.Department = Core.UserManager.GetUserGroups().Select(e => e.Name).ToList();
            return View();
        }
        [HttpGet]
        public ActionResult GetJsonBill(DateTime?startTime=null,DateTime? endTime=null,double?minMoney=null,double?maxMoney=null,string otherside=null,string remark=null,string association=null)
        {
            var parameter = new BillParamter()
            {
                StartTime = startTime,
                EndTime = endTime,
                MinMoney = minMoney,
                MaxMoney = maxMoney,
                OtherSide = otherside,
                Remark = remark
            };
            var list = Core.BillManager.Search(parameter).Where(e => e.Association != Association.Full);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 到账挂入发票的查询  发票状态为已开并且该发票未挂账
        /// </summary>
        /// <param name="ztopcompany"></param>
        /// <param name="department"></param>
        /// <param name="time"></param>
        /// <param name="otherside"></param>
        /// <param name="minmoney"></param>
        /// <param name="maxmoney"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetJsonInvoice(string ztopcompany=null,string department=null,string time=null,string otherside=null,double?minmoney=null,double? maxmoney=null)
        {
            var parameter = new InvoiceParameter()
            {
                Status=InvoiceState.Have,
                Recevied=Recevied.None,
                Department = department,
                Time = time,
                OtherSide = otherside,
                MinMoney = minmoney,
                MaxMoney = maxmoney,
                Instance = true
            };
            if (!string.IsNullOrEmpty(ztopcompany))
            {
                parameter.ZtopCompany = EnumHelper.GetEnum<ZtopCompany>(ztopcompany);
            }
            var list= Core.InvoiceManager.Search(parameter);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Relate(int bid,string iid)
        {
            return Core.InvoiceManager.Relate(bid, iid) ? SuccessJsonResult() : ErrorJsonResult("关联发票失败！");
        }

        public ActionResult BillSearch(DateTime? startTime=null,DateTime? endTime=null,double? minMoney=null,double? maxMoney=null,string otherside=null,string remark=null,string association=null,int page=1)
        {
            var parameter = new BillParamter()
            {
                StartTime = startTime,
                EndTime = endTime,
                MinMoney = minMoney,
                MaxMoney = maxMoney,
                OtherSide = otherside,
                Remark=remark,
                Page = new PageParameter(page, 20)
            };
            if (!string.IsNullOrEmpty(association))
            {
                parameter.Association = EnumHelper.GetEnum<Association>(association);
            }

            ViewBag.Result = Core.BillManager.Search(parameter);
            ViewBag.Parameter = parameter;

            return View();
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
            ViewBag.Invoice = Core.InvoiceManager.GetFullStance(id);
            return View();
        }

        public ActionResult ContractSearch(string name=null,string OtherSide=null,DateTime? startime=null,DateTime? endtime=null,string status=null,string recevied=null,double? minmoney=null,double? maxmoney=null,string department=null, string archived=null,string ztopcompany=null, int page=1)
        {
            var parameter = new ContractParameter()
            {
                OtherSide = OtherSide,
                Name = name,
                StartTime=startime,
                EndTime=endtime,
                MinMoney=minmoney,
                MaxMoney=maxmoney,
                Department=department,
                Page = new PageParameter(page, 20)
            };
            if (!string.IsNullOrEmpty(archived))
            {
                if (archived == "未归档")
                {
                    parameter.Archived = false;
                }
                else
                {
                    parameter.Archived = true;
                }
            }
            if (!string.IsNullOrEmpty(recevied))
            {
                parameter.Recevied = EnumHelper.GetEnum<Recevied>(recevied);
            }
            if (!string.IsNullOrEmpty(status))
            {
                parameter.Status = EnumHelper.GetEnum<ContractState>(status);
            }
            if (!string.IsNullOrEmpty(ztopcompany))
            {
                parameter.ZtopCompany = EnumHelper.GetEnum<ZtopCompany>(ztopcompany);
            }
            ViewBag.Results = Core.ContractManager.Search(parameter);
            ViewBag.Department = Core.UserManager.GetUserGroups().Select(e => e.Name).ToList();
            ViewBag.Parameter = parameter;
            return View(); 
        }


        public ActionResult BillAccountDetail(int id)
        {
            ViewBag.BillAccount = Core.BillAccountManager.Get(id, true);
            return View();
        }

        public ActionResult CreateArticle()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SaveArticle(Article article)
        {
            if (Core.ArticleManager.Exist(article))
            {
                return ErrorJsonResult("系统中已经存在相同名称和对方单位的项目，请核对！");
            }
            Core.ArticleManager.Save(article);
            return SuccessJsonResult();
        }

        public ActionResult DetailArticle(int id)
        {
            ViewBag.Article = Core.ArticleManager.Get(id);
            return View();
        }
        
        [HttpGet]
        public ActionResult GetJsonArticle(string name=null,string otherside=null,double? minmoney=null,double? maxmoney=null)
        {
            var parameter = new ArticleParameter()
            {
                Name = name,
                OtherSide = otherside,
                MinMoney = minmoney,
                MaxMoney = maxmoney
            };
            var list = Core.ArticleManager.Search(parameter);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ArticleSearch(string name=null,string otherside=null,double? minmoney=null,double? maxmoney=null,int page=1)
        {
            var parameter = new ArticleParameter
            {
                Name = name,
                OtherSide = otherside,
                MinMoney = minmoney,
                MaxMoney = maxmoney,
                Page = new PageParameter(page, 20)
            };
            ViewBag.List = Core.ArticleManager.Search(parameter);
            ViewBag.Parameter = parameter;
            return View();
        }
        public ActionResult ArticleContract()
        {
            return View();
        }

    }
}