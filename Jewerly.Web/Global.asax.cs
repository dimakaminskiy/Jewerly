using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Jewerly.Web.Areas.Admin;
using Jewerly.Web.Areas.Default;

namespace Jewerly.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {

            var adminArea = new AdminAreaRegistration();
            var adminAreaContext = new AreaRegistrationContext(adminArea.AreaName, RouteTable.Routes);
            adminArea.RegisterArea(adminAreaContext);

            var defaultArea = new DefaultAreaRegistration();
            var defaultAreaContext = new AreaRegistrationContext(defaultArea.AreaName, RouteTable.Routes);
            defaultArea.RegisterArea(defaultAreaContext);

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // RouteConfig.RegisterRoutes(RouteTable.Routes);
            //AreaRegistration.RegisterAllAreas();

        }
    }
}
