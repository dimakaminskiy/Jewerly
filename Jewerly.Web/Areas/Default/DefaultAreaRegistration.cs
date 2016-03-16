using System.Linq.Expressions;
using System.Web.Mvc;

namespace Jewerly.Web.Areas.Default
{
    public class DefaultAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Default";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {

            context.MapRoute(
               "",
               "product/{name}/{id}",
               new { controller = "Store", action = "Details" },
                  constraints: new { id = @"\d+" },
                   namespaces: new[] { "Jewerly.Web.Areas.Default.Controllers" }
           );


           // context.MapRoute(
           //    "",
           //    "{name}/{id}/sort-{sort}/page-{page}",
           //    new { controller = "Store", action = "Index" },
           //       constraints: new { id = @"\d+", sort="cheap|expensive|novelty", page = @"\d+" },
           //        namespaces: new[] { "Jewerly.Web.Areas.Default.Controllers" }
           //);


            context.MapRoute(
              "",
              "{name}/{id}/sort-{sort}",
              new { controller = "Store", action = "Index" },
                 constraints: new { id = @"\d+", sort = "cheap|expensive|novelty|null" },
                  namespaces: new[] { "Jewerly.Web.Areas.Default.Controllers" }
          );

          //  context.MapRoute(
          //    "",
          //    "{name}/{id}/page-{page}",
          //    new { controller = "Store", action = "Index", },
          //       constraints: new { id = @"\d+", page = @"\d+" },
          //        namespaces: new[] { "Jewerly.Web.Areas.Default.Controllers" }
          //);



      
            context.MapRoute(
                "",
                "{name}/{id}",
                new { controller = "Store", action = "Index", id = UrlParameter.Optional },
                constraints: new { id = @"\d+" },
                 namespaces: new[] { "Jewerly.Web.Areas.Default.Controllers" }
            );

            context.MapRoute(
                "",
                "{controller}/{action}/{id}",
                new {controller="Store" ,action = "Index", id = UrlParameter.Optional },
                 namespaces: new[] { "Jewerly.Web.Areas.Default.Controllers" }
            );
        }
    }
}