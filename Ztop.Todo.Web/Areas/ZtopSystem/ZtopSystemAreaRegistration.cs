using System.Web.Mvc;

namespace Ztop.Todo.Web.Areas.ZtopSystem
{
    public class ZtopSystemAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ZtopSystem";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "ZtopSystem_default",
                "ZtopSystem/{controller}/{action}/{id}",
                new { Controller="Home", action = "Index", id = UrlParameter.Optional },
                new [] { "Ztop.Todo.Web.Areas.ZtopSystem.Controllers" }
            );
        }
    }
}