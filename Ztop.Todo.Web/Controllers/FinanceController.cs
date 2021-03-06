﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Ztop.Todo.Common;
using Ztop.Todo.Model;
using Ztop.Todo.Web.Common;

namespace Ztop.Todo.Web.Controllers
{
    public class FinanceController : ControllerBase
    {
        
        // GET: Finance
        public ActionResult Index()
        {
            ViewBag.List = Core.ContractManager.Search(new ContractParameter() { Deleted = false, UserName = Identity.Project ? Identity.Name : null });
            ViewBag.Bills = Core.BillManager.Search(new BillParamter() { Page = new PageParameter(1, 20) });
            ViewBag.Invoices = Core.InvoiceManager.Search(new InvoiceParameter() { Status = InvoiceState.Have, Instance = false,Page=new PageParameter(1,20) });
            ViewBag.Articles = Core.ArticleManager.Search(new ArticleParameter() { Deleted=false, Page = new PageParameter(1, 20) });
            if (Identity.Name=="俞海峰")
            {
                ViewBag.None = Core.InvoiceManager.Search(new InvoiceParameter() { Status = InvoiceState.None, Instance = false,Page=new PageParameter(1,20) });
            }
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
            if (!string.IsNullOrEmpty(contract.Number)&&!Regex.IsMatch(contract.Number, @"^[0-9]{7}$"))
            {
                throw new ArgumentException("请核对登记编号，登记编号是7位数字！");
            }
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

        public ActionResult Simliar(string company,double money)
        {
            var list = Core.ContractManager.Simliar(company, money);
            ViewBag.List = list;
            return View();
        }

        /// <summary>
        /// 作用：录入合同扫描件
        /// 作者：汪建龙
        /// 编写时间：2016年11月19日16:19:49
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult UploadPdf(int id)
        {
            var contract = Core.ContractManager.Get(id);
            ViewBag.Contract = contract;
            return View();
        }

        /// <summary>
        /// 作用：用户上传合同扫描件之后POST
        /// 作者：汪建龙
        /// 编写时间：2016年11月19日16:38:34
        /// </summary>
        /// <param name="id"></param>
        /// <param name="addfolder"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Retake(int id,string addfolder)
        {
            if (Request.Files.Count == 0)
            {
                throw new ArgumentException("请选择上传文件");
            }
            var file = HttpContext.Request.Files[0];
            var fileName = file.FileName;
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentException("请选择上传文件");
            }
            var ext = System.IO.Path.GetExtension(fileName);
            var saveFilePath = string.Empty;
            if (ext != ".pdf")
            {
                throw new ArgumentException("请上传pdf文件");
            }
            saveFilePath = FileManager.UploadContract(file, addfolder);
            if (!Core.ContractManager.Retake(id, saveFilePath))
            {
                throw new ArgumentException("上传合同文件失败！");
            }

            return RedirectToAction("Detail", new { id });
        }

        /// <summary>
        /// 作用：查看发票申请开具信息界面
        /// 作者：汪建龙
        /// 编写时间：2016年11月19日16:43:50
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult InvoiceDetail(int id)
        {
            var invoice = Core.InvoiceManager.GetFullStance(id);
            ViewBag.Invoice = invoice;
            /*
             * 如果是财务查看发票信息，如俞海峰查看
             * 将对当前发票申请提示进行已读操作
             */
            if (Identity.Finance)
            {
                Core.NotificationManager.FlagInvoiceRead(id);
            }
            return View();
        }

        /// <summary>
        /// 作用：俞海峰 开具发票之后，将发票信息录入界面
        /// 作者：汪建龙
        /// 编写时间：2016年11月19日17:41:01
        /// </summary>
        /// <returns></returns>
        public ActionResult SuppleMentary(int id)
        {
            ViewBag.Invoice = Core.InvoiceManager.Get(id);
            return View();
        }

        public ActionResult Detail(int id,int ?notId=null)
        {
            var contract = Core.ContractManager.Get(id);
            if (contract != null)
            {
                contract.Invoices = Core.InvoiceManager.GetByCID(contract.ID);
                contract.ContractFiles = Core.ContractFileManager.GetContractFiles(contract.ID);
                contract.Article = Core.ArticleManager.Get(contract.Number);


                var CAList = Core.ContractArticleManager.GetByContractID(contract.ID);
                if (CAList != null && CAList.Count > 0)
                {
                    contract.Articles = Core.ArticleManager.GetByIDList(CAList.Select(e=>e.ArticleID).ToList());
                }
                contract.BillContracts = Core.BillContractManager.GetByContractID(contract.ID);
                if (notId.HasValue)
                {
                    Core.NotificationManager.FlagRead(notId.Value);
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
        
        /// <summary>
        /// 作用：用户申请开票 POST之后，保存
        /// 作者：汪建龙
        /// 编写时间：2016年11月19日18:06:10
        /// </summary>
        /// <param name="invoice"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveInvoice(Invoice invoice)
        {
            var contract = Core.ContractManager.Get(invoice.CID);
            if (contract != null)//如果发票有关联合同，则需要验证合同金额和发票总额
            {
                var list = Core.InvoiceManager.GetByCID(contract.ID);
                var sum = .0;
                if (list != null)
                {
                    //合计当前可开发票金额等于已开发票金额和已申请未开票金额
                    list = list.Where(e => e.State == InvoiceState.Have || e.State == InvoiceState.None).ToList();

                    sum = list.Sum(e => e.Money) + invoice.Money;
                }
                if (sum > contract.Money)
                {
                    return ErrorJsonResult("发票累计金额超出合同金额");
                }
            }
           
            var id = Core.InvoiceManager.Save(invoice);
            Core.NotificationManager.Add(invoice);
            return SuccessJsonResult(invoice);
        }

        public ActionResult EditInvoice(int id)
        {
            ViewBag.Invoice = Core.InvoiceManager.Get(id);
            return View();
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
            //bill.Budget = Budget.Income;
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

        public ActionResult OnAccount(int id)
        {
            var billView = Core.Bill_ViewManager.Get(id);
            if (billView != null)
            {
                billView.Bill_Contracts = Core.BillContractManager.GetByBillID(billView.ID);
            }
            ViewBag.Bill = billView;
            ViewBag.Department = Core.UserManager.GetUserGroups().Select(e => e.Name).ToList();
            return View();
        }
 
        [HttpGet]
        public ActionResult GetJsonContract(
            string name = null, string otherside = null,
            DateTime? starttime = null, DateTime? endtime = null,
            double? minmoney = null, double? maxmoney = null, 
            string ztopcompany = null,int page=1)
        {

            var parameter = new ContractParameter()
            {
                Name = name,
                OtherSide = otherside,
                StartTime = starttime,
                EndTime = endtime,
                MinMoney = minmoney,
                MaxMoney = maxmoney,
                Page = new PageParameter(page, 15)
            };
            if (!string.IsNullOrEmpty(ztopcompany))
            {
                parameter.ZtopCompany = EnumHelper.GetEnum<ZtopCompany>(ztopcompany);
            }
            var list = Core.ContractManager.Search(parameter).Where(e => e.Leave > 0).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult SearchContract(
            string name = null, string otherside = null,
            DateTime? starttime = null, DateTime? endtime = null,
            double? minmoney = null, double? maxmoney = null,double? money=null,
            string ztopcompany = null, int page = 1)
        {
            var parameter = new ContractParameter()
            {
                Name = name,
                OtherSide = otherside,
                StartTime = starttime,
                EndTime = endtime,
                MinMoney = minmoney,
                MaxMoney = maxmoney,
                Money=money,
                Page = new PageParameter(page, 15)
            };
            if (!string.IsNullOrEmpty(ztopcompany))
            {
                parameter.ZtopCompany = EnumHelper.GetEnum<ZtopCompany>(ztopcompany);
            }
            var list = Core.ContractManager.Search(parameter).Where(e => e.Leave > 0).ToList();
            list = Core.ContractManager.Rebody(list);
            ViewBag.List = list;
            ViewBag.Page = parameter.Page;
            ViewBag.Parameter = parameter;
            return View();
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
                    foreach(var contractid in billList.Select(e => e.ContractID).Distinct())
                    {
                        Core.ContractManager.UpdateRecevied(contractid);
                    }
                }

            }else
            {
                return ErrorJsonResult("关联金额超出到账可关联金额，请核对");
            }
            return SuccessJsonResult(bill.ID);
        }


        public ActionResult DetailBill(int id)
        {
            var bill = Core.BillManager.GetBill(id);
            if (bill != null)
            {
                bill.BillContracts = Core.BillContractManager.GetByBillID(bill.ID);
            }
            ViewBag.Bill = bill;
            return View();
        }

        /// <summary>
        /// 作用：到账  与合同挂账的时候，查询到账信息
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="minMoney"></param>
        /// <param name="maxMoney"></param>
        /// <param name="otherside"></param>
        /// <param name="remark"></param>
        /// <param name="association"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetJsonBill(string company = null, DateTime? startTime = null, DateTime? endTime = null, double? minMoney = null, double? maxMoney = null, string otherside = null, string remark = null, string association = null)
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

        public ActionResult LoadBill(
            DateTime? startTime = null, DateTime? endTime = null, 
            double? minMoney = null, double? maxMoney = null, 
            string otherside = null, string remark = null, 
            string association = null,int page = 1)
        {
            var parameter = new BillParamter
            {
                StartTime = startTime,
                EndTime = endTime,
                MinMoney = minMoney,
                MaxMoney = maxMoney,
                OtherSide = otherside,
                Remark = remark,
                Page = new PageParameter(page, 20)
            };
            parameter.Association = Association.None;

            return View();
        }
        /// <summary>
        /// 作用：同步银行账单
        /// 作者：汪建龙
        /// 编写时间：2017年1月10日10:19:33
        /// </summary>
        /// <returns></returns>
        public ActionResult Synchronous()
        {
            var list1 = Core.Bill_RecordManager.GetSync();
            if (list1 == null || list1.Count == 0)
            {
                return ErrorJsonResult("当前系统中没有可以同步到账信息");
            }
            try
            {
                Core.Bill_RecordManager.Sync(list1);
            }catch(Exception ex)
            {
                return ErrorJsonResult(ex);
            }
            return SuccessJsonResult();
        }

        public ActionResult Validate()
        {
            var errors = Core.ContractManager.Validate();
            if (errors.Count > 0)
            {
                return ErrorJsonResult(string.Join(";", errors.ToArray()));
            }
            return SuccessJsonResult();
        }
        /// <summary>
        /// 作用：基础搜索
        /// 作者：汪建龙
        /// 编写时间：2017年1月11日09:51:39
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="minMoney"></param>
        /// <param name="maxMoney"></param>
        /// <param name="otherside"></param>
        /// <param name="remark"></param>
        /// <param name="association"></param>
        /// <param name="cost"></param>
        /// <param name="company"></param>
        /// <param name="page"></param>
        private void BillSearchBase(
            DateTime? startTime = null, DateTime? endTime = null,
            double? minMoney = null, double? maxMoney = null,
            string otherside = null, string remark = null,
            string association = null, string cost = "实际收入", string company = null, PageParameter page=null)
        {
            var parameter = new BillParamter()
            {
                StartTime = startTime,
                EndTime = endTime,
                MinMoney = minMoney,
                MaxMoney = maxMoney,
                OtherSide = otherside,
                Remark = remark,
                Page = page
            };
            if (!string.IsNullOrEmpty(company))
            {
                parameter.Company = EnumHelper.GetEnum<Company>(company);
            }
            if (!string.IsNullOrEmpty(association))
            {
                parameter.Association = EnumHelper.GetEnum<Association>(association);
            }
            else
            {
                parameter.Association = Association.None;
            }
            if (!string.IsNullOrEmpty(cost))
            {
                parameter.Cost = EnumHelper.GetEnum<Cost>(cost);
            }
            var blist = Core.Bill_ViewManager.Search(parameter);

            ViewBag.BResult = blist;
            ViewBag.Parameter = parameter;
            ViewBag.Page = parameter.Page;
        }
        /// <summary>
        /// 作用：合同挂账查询
        /// 作者：汪建龙
        /// 编写时间：2017年1月11日09:52:22
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="minMoney"></param>
        /// <param name="maxMoney"></param>
        /// <param name="otherside"></param>
        /// <param name="remark"></param>
        /// <param name="company"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult SearchBill(
            DateTime?startTime=null,DateTime?endTime=null,
            double? minMoney=null,double?maxMoney=null,
            string otherside=null,string remark=null,string company=null,int page = 1)
        {
            BillSearchBase(startTime, endTime, minMoney, maxMoney, otherside, remark, Association.None.GetDescription(), Cost.RealIncome.GetDescription(), company, new PageParameter(page,20));
            return View();
        }
  
        /// <summary>
        /// 作用：单独页面查询 主要针对袁总开放到账查询
        /// 作者：汪建龙
        /// 编写时间：2017年1月11日09:51:57
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="minMoney"></param>
        /// <param name="maxMoney"></param>
        /// <param name="otherside"></param>
        /// <param name="remark"></param>
        /// <param name="association"></param>
        /// <param name="cost"></param>
        /// <param name="company"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult BillSearch(
            DateTime? startTime=null,DateTime? endTime=null,
            double? minMoney=null,double? maxMoney=null,
            string otherside=null,string remark=null,
            string association=null,string cost="实际收入",string company=null,int page=1)
        {
            BillSearchBase(startTime, endTime, minMoney, maxMoney, otherside, remark, association, cost, company, new PageParameter(page,20));
            return View();
        }

        /// <summary>
        /// 作用：针对市场部人员查看保证金退款
        /// 作者：汪建龙
        /// 编写时间：2017年4月28日17:19:32
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="minMoney"></param>
        /// <param name="maxMoney"></param>
        /// <param name="otherside"></param>
        /// <param name="remark"></param>
        /// <param name="association"></param>
        /// <param name="cost"></param>
        /// <param name="company"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult Cashdeposit(
            DateTime? startTime = null, DateTime? endTime = null,
            double? minMoney = null, double? maxMoney = null,
            string otherside = null, string remark = null,
            string association = null, string cost = "保证金退款", string company = null, int page = 1)
        {
            BillSearchBase(startTime, endTime, minMoney, maxMoney, otherside, remark, association, cost, company,new PageParameter(page,20));
            return View();
        }
        /// <summary>
        /// 作用：针对市场部开放实际收入未关联的到账信息
        /// 作者：汪建龙
        /// 编写时间：2017年4月19日10:17:25
        /// </summary>
        /// <returns></returns>
        public ActionResult BillSearchMarket()
        {
            BillSearchBase(null, null, null, null, null, null, Association.None.GetDescription(), "实际收入");
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
            ViewBag.Page = parameter.Page;
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


        public ActionResult ContractSearch(
            string name=null,string OtherSide=null,
            DateTime? startime=null,DateTime? endtime=null,
            string status=null,string recevied=null,
            double? minmoney=null,double? maxmoney=null,
            string department=null, string archived=null,
            string ztopcompany=null,string number=null, int page=1)
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
                Deleted=false,
                Number=number,
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
            ViewBag.Page = parameter.Page;
            return View(); 
        }
        /// <summary>
        /// 作用：下载合同统计表
        /// 作者：汪建龙
        /// 编写时间：2017年1月16日19:53:58
        /// </summary>
        /// <param name="name"></param>
        /// <param name="OtherSide"></param>
        /// <param name="startime"></param>
        /// <param name="endtime"></param>
        /// <param name="status"></param>
        /// <param name="recevied"></param>
        /// <param name="minmoney"></param>
        /// <param name="maxmoney"></param>
        /// <param name="department"></param>
        /// <param name="archived"></param>
        /// <param name="ztopcompany"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public ActionResult DownContract(
            string name = null, string OtherSide = null,
            DateTime? startime = null, DateTime? endtime = null,
            string status = null, string recevied = null,
            double? minmoney = null, double? maxmoney = null,
            string department = null, string archived = null,
            string ztopcompany = null, string number = null)
        {

            var parameter = new ContractParameter()
            {
                OtherSide = OtherSide,
                Name = name,
                StartTime = startime,
                EndTime = endtime,
                MinMoney = minmoney,
                MaxMoney = maxmoney,
                Department = department,
                Deleted = false,
                Number = number
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
            var result= Core.ContractManager.Search(parameter);
            result = Core.ContractManager.Rebody(result);
            var workbook = ExcelManager.DownContractWorkbook(result);
            if (workbook != null)
            {
                byte[] fileContents = ExcelManager.Translate(workbook);
                return File(fileContents, "application/ms-excel", string.Format("合同统计表-{0}.xls", DateTime.Now.ToLongDateString()));
            }
            
            return Content("内部服务器错误！");
        }


        public ActionResult CreateArticle(int id=0)
        {
            var article = Core.ArticleManager.Get(id);
            if (article != null)
            {
                article.CEntry = Core.CityManager.Get(article.CID);
                article.PEntry = Core.Project_TypeManager.Get(article.PID);
            }
            ViewBag.Article = article;
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
            var article = Core.ArticleManager.Get2(id);
            if (article != null)
            {
                article.Contracts = Core.ContractManager.GetByNumber(article.Number);
                //var CList = Core.ContractArticleManager.GetByArticleID(article.ID);
                //if (CList != null && CList.Count > 0)
                //{
                //    article.Contracts = Core.ContractManager.GetByIDList(CList.Select(e=>e.ContractID).ToList());
                //}
            }
            ViewBag.Article = article;
            return View();
        }

        public ActionResult DeletedArticle(int id)
        {
            return Core.ArticleManager.Deleted(id) ? SuccessJsonResult() : ErrorJsonResult("删除失败！参数错误，未找到相关项目信息");
        }

        public ActionResult ArticleState(int id)
        {
            var article = Core.ArticleManager.Get(id);
            ViewBag.Article = article;
            return View();
        }
        [HttpPost]
        public ActionResult EditState(int id,ArticleState state)
        {
            return Core.ArticleManager.Edit(id, state) ? SuccessJsonResult() : ErrorJsonResult("修改失败！");
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

        public ActionResult ArticleSearch(
            string name=null,string otherside=null,double? minmoney=null, double? maxmoney=null,
            string number=null,string year=null, string city=null, string town=null,
            string projecttype=null, string state=null,string payCompany=null,
            int page=1)
        {
            var parameter = new ArticleParameter
            {
                Name = name,
                OtherSide = otherside,
                MinMoney = minmoney,
                MaxMoney = maxmoney,
                Number=number,
                Year=year,
                City=city,
                Town=town,
                ProjectType=projecttype,
                PayCompany=payCompany,
                Page = new PageParameter(page, 20)
            };
            if (!string.IsNullOrEmpty(state))
            {
                parameter.State = EnumHelper.GetEnum<ArticleState>(state);
            }
            ViewBag.List = Core.ArticleManager.Search2(parameter);
            ViewBag.Parameter = parameter;
            ViewBag.Page = parameter.Page;
            return View();
        }

        /// <summary>
        /// 作用：下载项目洽谈表格
        /// 作者：汪建龙
        /// 编写时间：2017年1月16日19:53:26
        /// </summary>
        /// <param name="name"></param>
        /// <param name="otherside"></param>
        /// <param name="minmoney"></param>
        /// <param name="maxmoney"></param>
        /// <param name="number"></param>
        /// <param name="year"></param>
        /// <param name="city"></param>
        /// <param name="town"></param>
        /// <param name="projecttype"></param>
        /// <param name="state"></param>
        /// <param name="payCompany"></param>
        /// <returns></returns>
        public ActionResult DownloadArticle(
            string name = null, string otherside = null, double? minmoney = null, double? maxmoney = null,
            string number = null, string year = null, string city = null, string town = null,
            string projecttype = null, string state = null, string payCompany = null)
        {
            var parameter = new ArticleParameter
            {
                Name = name,
                OtherSide = otherside,
                MinMoney = minmoney,
                MaxMoney = maxmoney,
                Number = number,
                Year = year,
                City = city,
                Town = town,
                ProjectType = projecttype,
                PayCompany = payCompany
            };
            if (!string.IsNullOrEmpty(state))
            {
                parameter.State = EnumHelper.GetEnum<ArticleState>(state);
            }

            var list = Core.ArticleManager.Search2(parameter);
            var workbook = ExcelManager.DownloadArticleWorkbook(list);
            if (workbook != null)
            {
                byte[] fileContents = ExcelManager.Translate(workbook);
                return File(fileContents, "application/ms-excel", string.Format("项目洽谈统计表-{0}.xls", DateTime.Now.ToLongDateString()));
            }
            return Content("内部服务器错误！");
        }


  

    

    }
}