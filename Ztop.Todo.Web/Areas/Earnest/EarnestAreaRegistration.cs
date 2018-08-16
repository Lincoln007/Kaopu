using System.Web.Mvc;

namespace Ztop.Todo.Web.Areas.Earnest
{
    public class EarnestAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Earnest";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Earnest_default",
                "Earnest/{controller}/{action}/{id}",
                new { Controller="Home", action = "Index", id = UrlParameter.Optional },
                new [] { "Ztop.Todo.Web.Areas.Earnest.Controllers" }
            );
        }
    }
}