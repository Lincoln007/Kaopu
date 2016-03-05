using System.Web.Mvc;

namespace Ztop.Todo.Web.Areas.Jurisdiction
{
    public class JurisdictionAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Jurisdiction";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Jurisdiction_default",
                "Jurisdiction/{controller}/{action}/{id}",
                new { controller="Admin", action = "Index", id = UrlParameter.Optional },
                new[] { "Ztop.Todo.Web.Areas.Jurisdiction.Controllers" }
            );
        }
    }
}