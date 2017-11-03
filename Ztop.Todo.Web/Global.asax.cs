using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Ztop.Todo.Manager;

namespace Ztop.Todo.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //BundleConfig.RegisterBundles(BundleTable.Bundles);

            Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
        }

        protected virtual void Application_BeginRequest()
        {
            HttpDbContextContainer.OnBeginRequest(Context, new DataContext());
        }
        protected virtual void Application_EndRequest()
        {
            HttpDbContextContainer.OnEndRequest(Context);
        }
    }
}
