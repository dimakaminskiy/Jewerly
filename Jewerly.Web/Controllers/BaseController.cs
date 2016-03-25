using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Jewerly.Domain;
using Microsoft.AspNet.Identity;

namespace Jewerly.Web.Controllers
{
    public class BaseController : Controller
    {
        protected readonly DataManager DataManager;

      protected  static string RenderViewToString(ControllerContext context,
                                    string viewPath,object model)
        {
            // first find the ViewEngine for this view
            ViewEngineResult viewEngineResult = null;
            viewEngineResult = ViewEngines.Engines.FindView(context, viewPath, null);

            if (viewEngineResult == null)
                throw new FileNotFoundException("View cannot be found.");

            // get the view and attach the model to view data
            var view = viewEngineResult.View;
            context.Controller.ViewData.Model = model;

            string result = null;

            using (var sw = new StringWriter())
            {
                var ctx = new ViewContext(context, view,
                                            context.Controller.ViewData,
                                            context.Controller.TempData,
                                            sw);
                view.Render(ctx, sw);
                result = sw.ToString();
            }

            return result;
        }

      protected string FullyQualifiedApplicationPath(HttpContextBase context)
      {
          //Return variable declaration
          var appPath = string.Empty;

          //Getting the current context of HTTP request
          // var context = HttpContext.Current;

          //Checking the current context content
          if (context != null)
          {
              //Formatting the fully qualified website url/name
              appPath = string.Format("{0}://{1}{2}{3}",
                  context.Request.Url.Scheme,
                  context.Request.Url.Host,
                  context.Request.Url.Port == 80
                      ? string.Empty
                      : ":" + context.Request.Url.Port,
                  context.Request.ApplicationPath);
          }

          if (appPath.EndsWith("/"))
              appPath = appPath.Remove(appPath.Length - 1);


          return appPath;

      }

        public BaseController(DataManager dataManager)
        {
            DataManager = dataManager;
        }

   

        public int GetCurrentCurrency()
        {
            var cookie = Request.Cookies["Currency"];
            if (cookie == null) return DefaultCurrency;
            return int.Parse(cookie.Value);
        }

        public string GetCurrentUserId()
        {
           return User.Identity.GetUserId<string>();
        }

        public void SetCookie(string name, string value)
        {
            var httpCookie = HttpContext.Response.Cookies[name];
            if (httpCookie != null)
            {
                httpCookie.Value = value;
            }
            else
            {
                HttpCookie newCookie = new HttpCookie(name, value);
                newCookie.Expires = DateTime.Now.AddDays(365);
                Response.Cookies.Add(newCookie);
            }
        }

        public string GetCookie(string name)
        {
          var httpCookie = HttpContext.Request.Cookies[name];
           if (httpCookie != null) return httpCookie.Value;
           return string.Empty;
        }


        protected int DefaultCurrency = 3;
        protected override void OnActionExecuting(ActionExecutingContext context)
        {
            HttpCookie cookie = Request.Cookies["Currency"];
           
            if (cookie == null)
            {
                var value = 0;
                if (User.Identity.IsAuthenticated)
                {
                     var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
                     var currency = identity.Claims.Where(c => c.Type == "Currency").Select(c => c.Value).SingleOrDefault();
                    value = int.Parse(currency);
                }
                else
                {
                    value = DefaultCurrency;
                }


                HttpCookie newCookie = new HttpCookie("Currency", value.ToString());
                newCookie.Expires = DateTime.Now.AddDays(365);
                Response.Cookies.Add(newCookie);
             }
            base.OnActionExecuting(context);
        }
    }

}