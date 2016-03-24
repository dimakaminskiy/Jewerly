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
            var list = DataManager.SliderPictures.GetAll().OrderBy(t => Guid.NewGuid()).Take(5).ToList();
            return View(list);
         }
        
        public HomeController(DataManager dataManager) : base(dataManager)
        {
        }
    }
}