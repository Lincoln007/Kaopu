using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ztop.Todo.Common;
using Ztop.Todo.Model;

namespace Ztop.Todo.Web.Areas.Project.Controllers
{
    public class HomeController : ProjectControllerBase
    {
        
        /// <summary>
        /// 作用：高级查询   当前公司所有项目（登记编号已登记）
        /// </summary>
        /// <param name="name"></param>
        /// <param name="town"></param>
        /// <param name="year"></param>
        /// <param name="cityName"></param>
        /// <param name="fid"></param>
        /// <param name="groupName"></param>
        /// <param name="order"></param>
        /// <param name="chargeId"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        // GET: Project/Home
        public ActionResult Index(
            string name=null,string town=null,
            int? year=null,string cityName=null,
            int? fid=null,int? seid=null, string groupName=null,
            ProjectOrder order=ProjectOrder.ID,int? chargeId=null,
            int page=1,int rows=20
            )
        {
            var parameter = new ProjectParameter
            {
                Name = name,
                Town = town,
                Year = year,
                CityName = cityName,
                GroupName = groupName,
                FID=fid,
                SEID=seid,
                Order=order,
                ChargeID=chargeId,
                IsRecord=true,
                //Page = new PageParameter(page, rows)
            };
            var stringkey = Identity.UserID + ParameterManager.ParameterKey;
            SessionHelper.SetSession(stringkey, parameter);
            parameter.Page = new PageParameter(page, rows);
            SearchBase(parameter);
            ViewBag.Group = Core.UserGroupManager.Get();
            ViewBag.Charges = Core.ProjectUserManager.GetChargeList(groupName);
            return View();
        }
        private void SearchBase(ProjectParameter parameter)
        {
            ViewBag.Year = Core.ProjectManager.GetYears();
            ViewBag.Provinces = Core.CityManager.Search(new Model.CityParameter { Rank = Model.Rank.Province });
            ViewBag.Types = Core.Project_TypeManager.GetByPPID(0);
            var list = Core.ProjectManager.Search(parameter);
            ViewBag.List = list;
            ViewBag.Parameter = parameter;
            if (parameter.FID.HasValue)
            {
                ViewBag.STypes = Core.Project_TypeManager.GetByPPID(parameter.FID.Value);
            }
        }

        /// <summary>
        /// 作用：个人录入未登记编号的项目 （未登记项目查询）
        /// </summary>
        /// <returns></returns>
        public ActionResult Search(
            string name = null, string town = null,
            int? year = null, string cityName = null,
            int? fid = null, ProjectOrder order = ProjectOrder.ID, int page = 1, int rows = 20)
        {
            var parameter = new ProjectParameter
            {
                Name = name,
                Town = town,
                Year = year,
                CityName = cityName,
                FID = fid,
                Order = order,
                ChargeID = Identity.UserID,
                IsRecord=false,
                Page = new PageParameter(page, rows)
            };
            SearchBase(parameter);

            return View();
        }


        /// <summary>
        /// 作用：主要针对袁洋  查询公司未登记编号的项目 (公司未登记项目查询)
        /// </summary>
        /// <param name="name"></param>
        /// <param name="town"></param>
        /// <param name="year"></param>
        /// <param name="cityName"></param>
        /// <param name="fid"></param>
        /// <param name="order"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        public ActionResult Manager(
            string name = null, string town = null,
            int? year = null, string cityName = null, string groupName = null,
            int? fid = null, ProjectOrder order = ProjectOrder.ID, int? chargeId = null,
            int page = 1, int rows = 20)
        {
            var parameter = new ProjectParameter
            {
                Name = name,
                Town = town,
                Year = year,
                CityName = cityName,
                GroupName=groupName,
                FID = fid,
                Order = order,
                ChargeID = chargeId,
                IsRecord=false,
                Page = new PageParameter(page, rows)
            };
            SearchBase(parameter);
            ViewBag.Group = Core.UserGroupManager.Get();
            ViewBag.Charges = Core.UserManager.GetAllUsers();
            return View();
        }

        public ActionResult ChargeSearch(
            string name=null,string town=null,
            int? year=null,string cityName=null,
            int? fid=null,ProjectOrder order=ProjectOrder.ID,
            int page=1,int rows=20)
        {
            var parameter = new ProjectParameter
            {
                Name = name,
                Town = town,
                Year = year,
                CityName = cityName,
                FID = fid,
                Order = order,
                ChargeID = Identity.UserID,
                IsRecord = true,
               // Page = new PageParameter(page, rows)
            };
            var stringkey = Identity.UserID + ParameterManager.ParameterKey;
            SessionHelper.SetSession(stringkey, parameter);
            parameter.Page = new PageParameter(page, rows);
            SearchBase(parameter);
            return View();
        }

        public ActionResult PartSearch(string name = null, string town = null,
            int? year = null, string cityName = null,
            int? fid = null, ProjectOrder order = ProjectOrder.ID,
            int page = 1, int rows = 20)
        {
            var parameter = new ProjectParameter
            {
                Name = name,
                Town = town,
                Year = year,
                CityName = cityName,
                FID = fid,
                Order = order,
                Participation=Identity.Name,
                IsRecord = true,
                Page = new PageParameter(page, rows)
            };
            SearchBase(parameter);
            return View();
        }

        public ActionResult Create(int id=0)
        {
            ViewBag.Project = Core.ProjectManager.Get(id);
            ViewBag.Group = Group;
            return View();
        }

        [HttpPost]
        public ActionResult Save(Ztop.Todo.Model.Project project,int chargeID,string UserIDs)
        {
            var city = Core.CityManager.Get(project.CityId);
            if (city == null)
            {
                return ErrorJsonResult("请填写城市（县级）信息！");
            }
            if (project.ProjectTypeId == 0)
            {
                return ErrorJsonResult("请选择项目类型！");
            }


            var relations = new List<ProjectUser>();
            if (!string.IsNullOrEmpty(UserIDs))
            {
                relations = UserIDs.Split(';').Distinct().Select(e => new ProjectUser { UserId = int.Parse(e), Relation = ProjectRelation.Participation, ProjectId = project.ID }).ToList();
                var entry = relations.FirstOrDefault(e => e.UserId == chargeID);
                if (entry != null)
                {
                    relations.Remove(entry);
                }
            }
            relations.Add(new ProjectUser { UserId = chargeID, Relation = ProjectRelation.InCharge,ProjectId=project.ID } );

            var last = Core.ProjectManager.GetLast(project.Year);
            var serialNumber= last == null ? 1 : last.SerialNumber + 1;
            if (project.ID > 0)//编辑项目
            {
                
                if (!Core.ProjectManager.Edit(project,serialNumber))
                {
                    return ErrorJsonResult("编辑项目失败,未找到相关编辑项目信息");
                }
                if (!Core.ProjectUserManager.Edit(relations, project.ID))
                {
                    return ErrorJsonResult("编辑人物关系失败");
                }
                return SuccessJsonResult(project.ID);
            }
            else//录入项目 
            {
                project.ProjectUser = relations;

                project.SerialNumber = serialNumber;
                var id = Core.ProjectManager.Add(project);
                if (id > 0)
                {
                    //Core.NotificationManager.Add(project);
                    var recordId = Core.ProjectRecordManager.Save(new ProjectRecord { ProjectId = id, Content = string.Format("{0}录入项目{1}", Identity.Name, project.Name) });
                    var flowwData = Core.FlowwDataManager.Save(new FlowwData
                    {
                        InfoId = id,
                        FlowwId = Floww.ID
                    });
                    return SuccessJsonResult(id);
                }
                return ErrorJsonResult("保存失败");
            }
          
        }

        public ActionResult Detail(int id)
        {
            var model = Core.ProjectManager.Get(id);
            ViewBag.Model = model;
            ViewBag.FlowwData = Core.FlowwDataManager.Get(id, Floww.ID);
            var progress = Core.Project_ProgressManager.Get(id);
            ViewBag.Progress = progress;
            var records = Core.ProjectRecordManager.Get(id);
            ViewBag.Records = records;
            return View();
        }

        public ActionResult Number(int id)
        {
            var project = Core.ProjectManager.Get(id);
            ViewBag.Model = project;
            return View();
        }

        [HttpPost]
        public ActionResult SaveNumber(int id,string Number)
        {
            if (string.IsNullOrEmpty(Number))
            {
                return ErrorJsonResult("未获取登记编号信息");
            }
            if (Number.Length != 7)
            {
                return ErrorJsonResult("登记编号长度不符合要求，请核对位数");
            }
            if (!Core.ProjectManager.Number(id, Number))
            {
                return ErrorJsonResult("更改登记编号失败，未找到相关项目信息");
            }
            var recordId = Core.ProjectRecordManager.Save(new ProjectRecord() { ProjectId = id, Content = string.Format("{0}录入登记编号{1}", Identity.Name, Number) });
            return SuccessJsonResult();
        }

        public ActionResult Delete(int id)
        {
            if (!Core.ProjectManager.Delete(id))
            {
                return ErrorJsonResult("删除失败，未查询到相关项目信息");
            }
            return SuccessJsonResult();
        }

        public ActionResult Charge(int id)
        {
            var project = Core.ProjectManager.Get(id);
            ViewBag.Model = project;
            ViewBag.Groups = Core.UserGroupManager.Get();
            return View();
        }

        [HttpPost]
        public ActionResult ChangeCharge(int id,int userId)
        {
            var newUser = Core.UserManager.GetUser(userId);
            if (newUser == null)
            {
                return ErrorJsonResult("新的项目负责人信息不正确，请核查");
            }
            if (!Core.ProjectUserManager.ChangeCharge(id, userId))
            {
                return ErrorJsonResult("修改项目负责人失败,未找到相关项目负责人信息或者当前项目负责人与修改人的信息一致");
            }
            var recordId = Core.ProjectRecordManager.Save(new ProjectRecord { ProjectId = id, Content = string.Format("{0}更改项目负责人为{1}", Identity.Name, newUser.RealName) });
            return SuccessJsonResult();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Groups(int page=1,int rows=20)
        {
            var parameter = new ProjectParameter
            {
                GroupId = Group.ID,
                Page = new PageParameter(page, rows)
            };
            SearchBase(parameter);
            return View();
        }
    }
}