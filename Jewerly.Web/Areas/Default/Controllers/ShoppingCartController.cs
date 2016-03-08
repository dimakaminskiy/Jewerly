using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jewerly.Web.Areas.Default.Controllers
{
    public class ShoppingCartController : Controller
    {
        // GET: Default/ShoppingCart
        public ActionResult Index()
        {
            return View();
        }
    }
}