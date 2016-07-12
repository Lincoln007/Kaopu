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
            ViewBag.Articles = Core.ArticleManager.Search(new ArticleParameter() { Deleted=false, Page = new PageParameter(1, 20) });
            return View();
        }

        public ActionResult CreateContract(int id=0,string otherside=null)
        {
            if (id > 0)
            {
                var contract = Core.ContractManager.Get(id);
                if (contract != null)
                {
                    contract.ContractFiles = Core.ContractFileManager.GetContractFiles(contract.ID);
                    var AList = Core.ContractArticleManager.GetByContractID(contract.ID);
                    if (AList != null && AList.Count > 0)
                    {
                        contract.Articles = Core.ArticleManager.GetByIDList(AList.Select(e=>e.ArticleID).ToList());
                    }
                }
                ViewBag.Contract = contract;
            }
            ViewBag.OtherSide = otherside;
            return View();
        }

        [HttpPost]
        public ActionResult SaveContract(Contract contract,int[] articleid,string articlename, int[] leaves)
        {
            if (contract.ID == 0)
            {
                contract.Leave = contract.Money;
            }
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
            if (articleid != null)
            {
                Core.ContractArticleManager.UpdateContract(id, articleid);
            }
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
        [HttpGet]
        public ActionResult GetJsonContract(string name = null, string otherside = null, DateTime? starttime = null, DateTime? endtime = null, double? minmoney = null, double? maxmoney = null, string ztopcompany = null)
        {
            var parameter = new ContractParameter()
            {
                Name = name,
                OtherSide = otherside,
                StartTime = starttime,
                EndTime = endtime,
                MinMoney = minmoney,
                MaxMoney = maxmoney
            };
            if (!string.IsNullOrEmpty(ztopcompany))
            {
                parameter.ZtopCompany = EnumHelper.GetEnum<ZtopCompany>(ztopcompany);
            }
            var list = Core.ContractManager.Search(parameter);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Relate(int bill_id,int[] contract_id,double[] contract_price)
        {
            var bill = Core.BillManager.GetBill(bill_id);
            if (bill == null||contract_id==null||contract_price==null||contract_id.Count()!=contract_price.Count())
            {
                return ErrorJsonResult("未找到相关到账信息或者未找到相关合同信息，请核对！");
            }
            if (bill.Leave >= contract_price.Sum())
            {
                var billList = Core.BillContractManager.Get(bill.ID, contract_id, contract_price);
                if (billList.Count > 0)
                {
                    Core.BillContractManager.AddRange(billList);
                    if (!Core.BillManager.UpdateLeave(bill_id, billList.Sum(e => e.Price)))
                    {
                        return ErrorJsonResult("更新到账信息表失败！");
                    }

                }

            }else
            {
                return ErrorJsonResult("关联金额超出到账可关联金额，请核对");
            }
            return SuccessJsonResult();
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

        public ActionResult CreateArticle(int id=0)
        {
            if (id > 0)
            {
                var article = Core.ArticleManager.Get(id);
                if (article != null)
                {
                    var Clist = Core.ContractArticleManager.GetByArticleID(article.ID);
                    if (Clist != null && Clist.Count > 0)
                    {
                        article.Contracts = Core.ContractManager.GetByIDList(Clist.Select(e => e.ContractID).ToList());
                    }
                }
                ViewBag.Article = article;
            }
            return View();
        }
        [HttpPost]
        public ActionResult SaveArticle(Article article,int[] contractid,string contractName)
        {
            var id = Core.ArticleManager.Save(article);
            if (contractid != null)
            {
                Core.ContractArticleManager.UpdateArticle(id, contractid);
            }  
            return SuccessJsonResult(id);
        }

        public ActionResult DetailArticle(int id)
        {
            var article = Core.ArticleManager.Get(id);
            if (article != null)
            {
                var CList = Core.ContractArticleManager.GetByArticleID(article.ID);
                if (CList != null && CList.Count > 0)
                {
                    article.Contracts = Core.ContractManager.GetByIDList(CList.Select(e=>e.ContractID).ToList());
                }
            }
            ViewBag.Article = article;
            return View();
        }

        public ActionResult DeletedArticle(int id)
        {
            return Core.ArticleManager.Deleted(id) ? SuccessJsonResult() : ErrorJsonResult("删除失败！参数错误，未找到相关项目信息");
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

    }
}