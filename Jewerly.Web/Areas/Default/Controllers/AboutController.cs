using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Jewerly.Domain;
using Jewerly.Web.Controllers;
using Jewerly.Web.Models;
using Jewerly.Web.Utils;

namespace Jewerly.Web.Areas.Default.Controllers
{
    public class AboutController : BaseController
    {

        public ActionResult Delivery()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult SliderPictures()
        {
            return PartialView("_SliderPictures",DataManager.SliderPictures.GetAll().OrderBy(t=> Guid.NewGuid()).Take(5).ToList());
        }

        public ActionResult Contacts()
        {
            return View();
        }

        public ActionResult Index()
        {
            var model = new MenuCategories(null, StoreHelper.GetListMenuCategories());
            return View(model);
        }
        public AboutController(DataManager dataManager) : base(dataManager)
        {
             StoreHelper = new StoreHelper(dataManager);
        }
        StoreHelper StoreHelper { get; set; }
    }
}