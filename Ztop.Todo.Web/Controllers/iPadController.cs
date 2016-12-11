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
        public ActionResult Index(iPadCategory category=iPadCategory.iPad,iPadType type=iPadType.Mini2)
        {
            if (!Identity.iPad)
            {
                return View("Fate");
            }
            ViewBag.Category = category;
            ViewBag.Type = type;
            ViewBag.Contracts = Core.iPad_ContractManager.Get();
            ViewBag.Invoices = Core.iPad_InvoiceManager.Get();
            ViewBag.Accounts = Core.iPad_AccountManager.Get();

            return View();
        }
        /// <summary>
        /// 作用：返回借用记录界面
        /// 作者：汪建龙
        /// 编写时间：2016年12月11日10:52:33
        /// </summary>
        /// <returns></returns>
        public ActionResult ManagerRegisters()
        {
            return View();
        }

        public ActionResult RegisterList(bool revert)
        {
            var list = Core.iPad_registerManager.Get();
            list = list.Where(e => e.Revert == revert).OrderByDescending(e=>e.Number).ToList();
            ViewBag.List = list;
            return View();
        }

        /// <summary>
        /// 作用：返回平板列表界面
        /// 作者：汪建龙
        /// 编写时间：2016年12月9日11:10:22
        /// </summary>
        /// <param name="iPadTye"></param>
        /// <returns></returns>
        public ActionResult ManageriPad(iPadType type=iPadType.Mini2)
        {
            ViewBag.Type = type;
            return View();
        }
        /// <summary>
        /// 作用：返回某一类型的平板列表界面
        /// 作者：汪建龙
        /// 编写时间：2016年12月9日11:10:48
        /// </summary>
        /// <param name="iPadType"></param>
        /// <returns></returns>
        public ActionResult iPadList(iPadType iPadType)
        {
            var list = Core.iPadManager.Get(iPadType);
            ViewBag.List = list;
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

        /// <summary>
        /// 作用：查看所有联系人列表
        /// 作者：汪建龙
        /// 编写时间：2016年12月8日10:36:04
        /// </summary>
        /// <returns></returns>
        public ActionResult ManagerContact()
        {
            var list = Core.iPad_ContactManager.Get();
            ViewBag.List = list;
            return View();
        }

        /// <summary>
        /// 作用：添加联系人
        /// 作者：汪建龙
        /// 编写时间：2016年12月8日10:36:26
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateContact()
        {
            return View();
        }

        /// <summary>
        /// 作用：保存联系人
        /// 作者：汪建龙
        /// 编写时间：2016年12月8日10:36:49
        /// </summary>
        /// <param name="contact"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveContact(iPadContact contact)
        {
            if (contact == null)
            {
                return ErrorJsonResult("服务器参数错误！");
            }
            if (string.IsNullOrEmpty(contact.Name) || string.IsNullOrEmpty(contact.City) || string.IsNullOrEmpty(contact.Address))
            {
                return ErrorJsonResult("联系人名字以及所属城市和地址不能为空!");
            }
            if (Core.iPad_ContactManager.Exist(contact.Name, contact.Address, contact.City))
            {
                return ErrorJsonResult(string.Format("系统中已存在{0}-{1}-{2}", contact.Name, contact.Address, contact.City));
            }
            var id = Core.iPad_ContactManager.Add(contact);
            if (id > 0)
            {
                return SuccessJsonResult();
            }
            return ErrorJsonResult("保存失败！");
        }

        /// <summary>
        /// 作用：编辑联系人
        /// 作者：汪建龙
        /// 编写时间：2016年12月8日10:37:09
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult EditContact(int id)
        {
            var contact = Core.iPad_ContactManager.Get(id);
            ViewBag.Contact = contact;
            return View();
        }

        /// <summary>
        /// 作用：编辑
        /// 作者：汪建龙
        /// 编写时间：2016年12月8日10:37:26
        /// </summary>
        /// <param name="contact"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditContact(iPadContact contact)
        {
            if (contact == null)
            {
                return ErrorJsonResult("服务器参数错误，无法编辑");
            }
            if (!Core.iPad_ContactManager.Edit(contact))
            {
                return ErrorJsonResult("编辑失败！原因：系统中未找到相关记录");
            }

            return SuccessJsonResult();
        }
        /// <summary>
        /// 作用:删除联系人
        /// 作者：汪建龙
        /// 编写时间：2016年12月8日10:45:16
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeleteContact(int id)
        {
            if (!Core.iPad_ContactManager.Delete(id))
            {
                return ErrorJsonResult("删除失败，请核对联系人是否已删除！");
            }
            return SuccessJsonResult();
        }

        public ActionResult ManagerDatum()
        {
            var list = Core.iPad_DatumManager.Get();
            ViewBag.List = list;
            return View();
        }

        public ActionResult CreateDatum()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SaveDatum(iPadDatum datum)
        {
            var id = Core.iPad_DatumManager.Add(datum);
            if (id == 0)
            {
                return ErrorJsonResult("添加失败！");
            }
            return SuccessJsonResult();
        }

    }
}