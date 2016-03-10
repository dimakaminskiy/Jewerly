using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Jewerly.Domain;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Ninject.Activation;

namespace Jewerly.Web.Controllers
{
    public class BaseController : Controller
    {
        protected readonly DataManager DataManager;

        public BaseController(DataManager dataManager)
        {
            DataManager = dataManager;
        }

        //private void SetAuthenticatedUserCorrency(int id)
        //{
        //    var manager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
        //    var u = manager.FindById(GetCurrentUserId());
        //    u.CurrencyId = id;
        //    manager.Update(u);
        //}

        //private void SetCookieCorrency(int id)
        //{
        //    HttpCookie cookie = Request.Cookies["Currency"];
        //    if (cookie == null)
        //    {
        //        HttpCookie newCookie = new HttpCookie("Currency", id.ToString());
        //        newCookie.Expires = DateTime.Now.AddDays(365);
        //        Response.Cookies.Add(newCookie);
        //    }
        //    else
        //    {
        //        cookie.Value = id.ToString();
        //        Response.Cookies.Set(cookie);
        //    }


        //}

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

        //public void  SetCurrency(int id)
        //{
        //    int oldValue = GetCurrentCurrency();
        //    if (id != oldValue ) 
        //    {
        //        if (DataManager.Currencies.Count(t => t.CurrencyId == id) > 0)
        //        {
        //            if (User.Identity.IsAuthenticated)
        //            {
        //                SetAuthenticatedUserCorrency(id);
        //            }
        //             SetCookieCorrency(id);
        //        }
        //        else
        //        {
        //            if (User.Identity.IsAuthenticated)
        //            {
        //                SetAuthenticatedUserCorrency(DefaultCurrency);
        //            }
        //            SetCookieCorrency(DefaultCurrency);   
        //        }
                
        //    }
        //}



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
            //var responseCookie = Response.Cookies[name];
            //if (responseCookie != null)
            //{
            //    if (responseCookie.Value != null)
            //    {
            //        return responseCookie.Value;
            //    }
                    
            //}

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