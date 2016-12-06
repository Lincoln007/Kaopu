using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ztop.Todo.Common;
using Ztop.Todo.Model;

namespace Ztop.Todo.Web.Controllers
{
    public class ManagerController : ControllerBase
    {
        /// <summary>
        /// 作用：系统配置界面
        /// 作者：汪建龙
        /// 时间：2016年12月5日10:18:18
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(SystemCategory category=SystemCategory.City)
        {
            ViewBag.Category = category;
            return View();
        }

        /// <summary>
        /// 作用：获取所有城市信息
        /// 作者：汪建龙
        /// 编写时间：2016年12月5日16:38:02
        /// </summary>
        private void GetCitys()
        {
            var list = Core.CityManager.GetList();
            var provinces = list.Where(e => e.Rank == Rank.Province).ToList();
            var citys = list.Where(e => e.Rank == Rank.City).ToList();
            var districts = list.Where(e => e.Rank == Rank.District).ToList();
            var towns = list.Where(e => e.Rank == Rank.Town).ToList();
            ViewBag.Provinces = provinces;
            ViewBag.Citys = citys;
            ViewBag.Districts = districts;
            ViewBag.Towns = towns;
        }

        /// <summary>
        /// 作用：管理城市
        /// 作者：汪建龙
        /// 时间：2016年12月5日10:18:40
        /// </summary>
        /// <returns></returns>
        public ActionResult ManagerCity()
        {
            GetCitys();
            return View();
        }

        /// <summary>
        /// 作用：返回选择城市界面
        /// 作者：汪建龙
        /// 编写时间：2016年12月5日16:38:34
        /// </summary>
        /// <returns></returns>
        public ActionResult ChooseCity()
        {
            GetCitys();
            return View();
        }
        /// <summary>
        /// 作用：创建省份
        /// 作者：汪建龙
        /// 时间：2016年12月5日11:19:00
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateProvince()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SaveProvince(City city)
        {
            if (city == null)
            {
                return ErrorJsonResult("服务器参数错误！");
            }
            if (string.IsNullOrEmpty(city.Name) || string.IsNullOrEmpty(city.Code))
            {
                return ErrorJsonResult("城市名称以及代码不能空！");
            }
            
            if (Core.CityManager.Exist(city.Name, city.Code))
            {
                return ErrorJsonResult(string.Format("系统中已存在{0}-{1}的城市,请核对", city.Code, city.Name));
            }
            city.Rank = Rank.Province;
            var id = Core.CityManager.Add(city);

            return SuccessJsonResult();
        }
        /// <summary>
        /// 作用：添加城市（市级，区县级）
        /// 作者：汪建龙
        /// 编写时间：2016年12月5日14:12:06
        /// </summary>
        /// <param name="name">所属上级城市名称</param>
        /// <returns></returns>
        public ActionResult CreateCity(string name)
        {
            var preCity = Core.CityManager.Get(name);
            ViewBag.PreCity = preCity;
            return View();
        }
        [HttpPost]
        public ActionResult SaveCity(City city)
        {
            if (city == null)
            {
                return ErrorJsonResult("服务器参数错误！");
            }
            if (string.IsNullOrEmpty(city.Name) || string.IsNullOrEmpty(city.Code))
            {
                return ErrorJsonResult("城市名称和城市代码不能为空！");
            }
            if (Core.CityManager.Exist(city.Name, city.Code))
            {
                return ErrorJsonResult(string.Format("系统中已存在{0}-{1}的城市，请核对!", city.Code, city.Name));
            }
            var id = Core.CityManager.Add(city);
            return SuccessJsonResult();
        }

        /// <summary>
        /// 作用：城市编辑
        /// 作者：汪建龙
        /// 编写时间：2016年12月5日15:49:34
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult EditCity(int id)
        {
            var city = Core.CityManager.Get(id);
            ViewBag.City = city;
            return View();
        }

        [HttpPost]
        public ActionResult EditCity(City city)
        {
            if (city == null)
            {
                return ErrorJsonResult("服务器参数错误！");
            }
            if (!Core.CityManager.Edit(city))
            {
                return ErrorJsonResult("编辑城市错误！");
            }

            return SuccessJsonResult();
        }

        /// <summary>
        /// 作用：删除城市
        /// 作者：汪建龙
        /// 编写时间：2016年12月5日15:56:58
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeleteCity(int id)
        {
            if (!Core.CityManager.Delete(id))
            {
                return ErrorJsonResult("删除失败，请核对是否又下级行政区！");
            }
            return SuccessJsonResult();
        }

        /// <summary>
        /// 作用：获取相关项目类型信息
        /// 作者：汪建龙
        /// 编写时间：2016年12月6日14:50:18
        /// </summary>
        private void GetProjectTyps()
        {
            var list = Core.Project_TypeManager.Get();
            ViewBag.First = list.Where(e => e.Degree == Degree.First).OrderBy(e => e.Chars).ToList();
            ViewBag.Second = list.Where(e => e.Degree == Degree.Second).ToList();
        }
        public ActionResult ManagerProjectType()
        {
            GetProjectTyps();
            return View();
        }
        /// <summary>
        /// 作用：返回选择项目类型界面
        /// 作者：汪建龙
        /// 编写时间：2016年12月6日14:55:07
        /// </summary>
        /// <returns></returns>
        public ActionResult ChooseProjectType()
        {
            GetProjectTyps();
            return View();
        }
        /// <summary>
        /// 作用：添加项目类型
        /// 作者：汪建龙
        /// 编写时间：2016年12月6日11:06:08
        /// </summary>
        /// <param name="degree">级别（需要添加的级别）</param>
        /// <param name="pName">当添加的级别为二级类的时候，所属一级类的名称</param>
        /// <returns></returns>
        public ActionResult CreateProjectType(Degree degree,string pchars,string pName)
        {
            if (degree == Degree.Second)
            {
                var preProjectType = Core.Project_TypeManager.Get(pName,Degree.First,pchars);
                ViewBag.First = preProjectType;
            }
            ViewBag.Degree = degree;
            return View();
        }

        [HttpPost]
        public ActionResult SaveProjectType(ProjectType type)
        {
            if (type == null)
            {
                return ErrorJsonResult("服务器参数错误！");
            }
            if (string.IsNullOrEmpty(type.Name)||string.IsNullOrEmpty(type.Chars))
            {
                return ErrorJsonResult("请输入项目类型名称和代码");
            }
            if (Core.Project_TypeManager.Exist(type.Name, type.Degree,type.Chars))
            {
                return ErrorJsonResult(string.Format("当前系统中已存在{0}：{1}{2},请核对！", type.Degree.GetDescription(),type.Chars, type.Name));
            }
            var id = Core.Project_TypeManager.Add(type);
            if (id > 0)
            {
                return SuccessJsonResult();
            }
            return ErrorJsonResult("保存到系统失败！");
        }

        /// <summary>
        /// 作用：编辑项目类型
        /// 作者：汪建龙
        /// 编写时间：2016年12月6日13:15:57
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult EditProjectType(int id)
        {
            var projectType = Core.Project_TypeManager.Get(id);
            ViewBag.ProjectType = projectType;
            return View();
        }
        [HttpPost]
        public ActionResult EditProjectTyope(ProjectType type)
        {
            if (type == null)
            {
                return ErrorJsonResult("服务器参数错误！");
            }

            if (string.IsNullOrEmpty(type.Chars) || string.IsNullOrEmpty(type.Name))
            {
                return ErrorJsonResult("代码和项目类型名称不能为空!");
            }
            if (!Core.Project_TypeManager.Edit(type))
            {
                return ErrorJsonResult("编辑失败！");
            }

            return SuccessJsonResult();
        }
        /// <summary>
        /// 作用：删除项目类型
        /// 作者：汪建龙
        /// 编写时间:2016年12月6日14:51:00
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeleteProjectType(int id)
        {
            if (!Core.Project_TypeManager.Delete(id))
            {
                return ErrorJsonResult("删除失败，请核对是否存在二级类!");
            }
            return SuccessJsonResult();
        }


        


    }
}