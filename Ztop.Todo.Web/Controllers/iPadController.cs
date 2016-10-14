using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ztop.Todo.Common;
using Ztop.Todo.Model;

namespace Ztop.Todo.Web.Controllers
{
    public class iPadController : ControllerBase
    {
        // GET: iPad
        public ActionResult Index()
        {
            if (!Identity.iPad)
            {
                return View("Fate");
            }
            ViewBag.List = Core.iPadManager.Get();
            ViewBag.Registers = Core.iPad_registerManager.Get();
            ViewBag.Contracts = Core.iPad_ContractManager.Get();
            ViewBag.Invoices = Core.iPad_InvoiceManager.Get();
            ViewBag.Accounts = Core.iPad_AccountManager.Get();
            return View();
        }

        public ActionResult Create(int id=0,bool edit=false)
        {
            ViewBag.Edit = edit;
            ViewBag.iPad = Core.iPadManager.Get(id);
            ViewBag.Accounts = Core.iPad_AccountManager.Get();
            return View();
        }

        [HttpPost]
        public ActionResult Create(iPad ipad,bool edit)
        {
            try
            {
                var id = Core.iPadManager.Add(ipad,edit);
            }
            catch(Exception ex)
            {
                return ErrorJsonResult(ex.ToString());
            }
            return SuccessJsonResult();
        }

        public ActionResult Delete(int id)
        {
            var relations = Core.Register_iPadManager.GetByiPadID(id);
            if (relations.Count > 0)
            {
                return ErrorJsonResult("不能删除该平板，该平板已录入使用中");
            }
            try
            {
                Core.iPadManager.Delete(id);
            }catch(Exception ex)
            {
                return ErrorJsonResult(ex.ToString());
            }
            return SuccessJsonResult();
        }


        public ActionResult CreateInvoice()
        {

            ViewBag.List = Core.iPadManager.Get().Where(e =>!e.HasInvoice).ToList();
            return View();
        }
        

        public ActionResult CreateContract(int id=0)
        {
            ViewBag.Contract = Core.iPad_ContractManager.Get(id);
            ViewBag.List = Core.iPadManager.Get().Where(e => e.Statue == iPadStatue.Vacant).ToList();
            return View();
        }

        public ActionResult CreateRegister(int id=0)
        {
            ViewBag.Register = Core.iPad_registerManager.Get(id);
            ViewBag.List = Core.iPadManager.Get().Where(e => e.Statue == iPadStatue.Vacant).ToList();
            return View();
        }

        [HttpPost]
        public ActionResult SaveRegister(iPadRegister register,int[] ipads)
        {
            if (ipads == null)
            {
                return ErrorJsonResult("当前未选择平板，请选择");
            }
            try
            {
                var rid=Core.iPad_registerManager.Save(register);
                var list = ipads.Select(e => new Register_iPad { IID = e, RID = rid, Relation = Relation.Register_iPad }).ToList();
                Core.Register_iPadManager.Add(list, rid, Relation.Register_iPad);
                if (!Core.iPadManager.Update(ipads, iPadStatue.Borrow))
                {
                    return ErrorJsonResult("更改平板状态失败，请检查iPad使用状态");
                }
            }
            catch(Exception ex)
            {
                return ErrorJsonResult(ex.ToString());
            }
            return SuccessJsonResult();
        }

        public ActionResult DeleteRegister(int id)
        {
            var register = Core.iPad_registerManager.Get(id);
            if (register == null)
            {
                return ErrorJsonResult("未找到使用登记");
            }
            if (register.Register_iPads != null)
            {
                Core.Register_iPadManager.Delete(register.Register_iPads);
                Core.iPadManager.Update(register.Register_iPads.Select(e => e.IID).ToArray(), iPadStatue.Vacant);
            }
            if (!Core.iPad_registerManager.Delete(id))
            {
                return ErrorJsonResult("删除失败");
            }

            return SuccessJsonResult();
        }

        public ActionResult Restore(int rid)
        {
            ViewBag.Register = Core.iPad_registerManager.Get(rid);
            return View();
        }

        [HttpPost]
        public ActionResult Restore(int[] iPadID,DateTime Time,string Restorer,int rid)
        {
            if (iPadID == null)
            {
                return ErrorJsonResult("归还平板失败，未读取到平板信息！");
            }
            var relations = iPadID.Select(e => new Register_iPad { RID = rid, IID = e, Revert = iPadRevert.Back, Restorer = Restorer, Time = Time, Relation = Relation.Register_iPad }).ToList();

            Core.Register_iPadManager.Change(relations);
            Core.iPadManager.Update(iPadID, iPadStatue.Vacant);
            return SuccessJsonResult();
        }

        [HttpPost]
        public ActionResult SaveContract(iPadContract contract,int[] iPadID)
        {
            var file = HttpContext.Request.Files[0];
            var saveFullFilePath = FileManager.Upload(file);
            contract.File = saveFullFilePath;
            var id = Core.iPad_ContractManager.Save(contract);
            if (id > 0)
            {
                if (iPadID != null)
                {

                    var list = iPadID.Select(e => new Register_iPad { RID = id, IID = e, Relation = Relation.Contract_iPad }).ToList();
                    Core.Register_iPadManager.Add(list, id, Relation.Contract_iPad);
                    if (!Core.iPadManager.Update(iPadID, iPadStatue.Deliver))
                    {
                        throw new ArgumentException("更改平板状态失败，请检查iPad使用状态");
                    }
                }
            }
            else
            {
                throw new ArgumentException("保存合同失败！");
            }
            return RedirectToAction("Index");
        }

        public ActionResult DetailRelation(int rid,Relation relation)
        {
            ViewBag.List = Core.Register_iPadManager.Get(rid, relation);
            ViewBag.Relation = relation;
            return View();
        }

        [HttpPost]
        public ActionResult SaveInvoice(iPadInvoice invoice,int[] iPadID)
        {
            if (iPadID == null)
            {
                throw new ArgumentException("未选择平板");
            }
            var file = HttpContext.Request.Files[0];
            var saveFullFilePath = FileManager.Upload(file);
            invoice.File = saveFullFilePath;

            var id = Core.iPad_InvoiceManager.Save(invoice);
            if (id > 0)
            {
                var list = iPadID.Select(e => new Register_iPad { IID = e, RID = id, Relation = Relation.Invoice_iPad }).ToList();
                Core.Register_iPadManager.Add(list, id, Relation.Invoice_iPad);
            }
            else
            {
                throw new ArgumentException("保存平板发票信息错误！");
            }


            return RedirectToAction("Index");
        }

        public ActionResult Translate(int rid)
        {
            var register = Core.iPad_registerManager.Get(rid);
            if (register == null||(register.iPads!=null&&register.iPads.Count==0))
            {
                throw new ArgumentException("未查询到平板信息");
            }
            ViewBag.Register = register;
            ViewBag.Contracts = Core.iPad_ContractManager.Get();
            return View();
        }

        [HttpPost]
        public ActionResult Translate(int cid,int[] iPadID,int rid)
        {
            if (cid == 0 || iPadID == null)
            {
                return ErrorJsonResult("请选择项目或者平板");
            }
            var contract = Core.iPad_ContractManager.Get(cid);
            if (contract == null)
            {
                return ErrorJsonResult("项目合同未找到");
            }
            var list = iPadID.Select(e => new Register_iPad { RID = rid, IID = e, Relation = Relation.Register_iPad,Time=DateTime.Now,Revert=iPadRevert.Projection }).ToList();
            Core.Register_iPadManager.Change(list);
            list = iPadID.Select(e => new Register_iPad { RID = cid, IID = e, Relation = Relation.Contract_iPad }).ToList();
            Core.Register_iPadManager.Append(list);
            //Core.Register_iPadManager.Add(list, cid, Relation.Contract_iPad);
            if (!Core.iPadManager.Update(iPadID, iPadStatue.Deliver))
            {
                return ErrorJsonResult("更新平板状态失败");
            }
            return SuccessJsonResult();
        }

        [HttpPost]
        public ActionResult Upload()
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
            
            var saveFilePath = FileManager.Upload2(file);
            return SuccessJsonResult(new { saveFilePath, fileName });
        }

        public ActionResult CreateAccount(int id=0)
        {
            ViewBag.Account = Core.iPad_AccountManager.Get(id);
            ViewBag.Edit = id > 0;
            return View();
        }

        [HttpPost]
        public ActionResult SaveAccount(int id,DateTime time,string enter,string email,string epassword,string account,string password,bool edit)
        {
            var aid = Core.iPad_AccountManager.Save(new iPadAccount { ID=id, Time=time,Enter=enter,EMail=email,EPassword=epassword,Account=account,Password=password}, edit);
            if (aid == 0)
            {
                return ErrorJsonResult("保存账号失败");
            }
            return SuccessJsonResult();
        }
    }
}