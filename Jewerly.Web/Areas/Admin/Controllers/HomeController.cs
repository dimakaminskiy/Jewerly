using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Jewerly.Domain;
using Jewerly.Web.Controllers;

namespace Jewerly.Web.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        // GET: Admin/Home
        public ActionResult Index()
        {
            return View();
         }

        public HomeController(DataManager dataManager) : base(dataManager)
        {
        }
    }
}