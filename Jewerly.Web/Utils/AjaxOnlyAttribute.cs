using System.Web;
using System.Web.Mvc;

namespace Jewerly.Web.Utils
{
    public class AjaxOnlyAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.HttpContext.Request.IsAjaxRequest())
                filterContext.HttpContext.Response.Redirect("/error/404");
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {

        }
    }

//    public class LocalizationAwareAttribute : ActionFilterAttribute
//{
//    public override void OnActionExecuting(ActionExecutingContext filterContext)
//    {
//        var cookies = filterContext.HttpContext.Request.Cookies;

//        if (!cookies.Keys.Contains("language"))
//        {
//            httpContext.Response.AppendCookie(new HttpCookie("language", 1));
//        }
//        if (!httpContext.Cookies.Keys.Contains("country"))
//        {
//            httpContext.Response.AppendCookie(new HttpCookie("country", 7));
//        }
//    }
//}

}