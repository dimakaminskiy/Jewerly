using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Jewerly.Domain;
using Jewerly.Web.Controllers;

namespace Jewerly.Web.Areas.Default.Controllers
{
    public class ReviewsController : BaseController
    {
        // GET: Default/Reviews
        public ActionResult Index()
        {
            var list = DataManager.Reviews.GetAll().OrderByDescending(t => t.Date).ToList();
            return View(list);
        }

        public ActionResult Create()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult Create(Review model)
        {
            if (ModelState.IsValid)
            {
                model.Date = DateTime.Now;

                DataManager.Reviews.Insert(model);
                var url = @Url.Action("Index", "Reviews", new {area = "Default"});
                TempData["message"] = string.Format("\"{0}\" ваш отзыв был добавлен",model.Name);
                //return RedirectToAction("Index", new {area = "Default"});
                return Json(new { success = true, url = url });
            }


            return PartialView();
        }

        public ReviewsController(DataManager dataManager) : base(dataManager)
        {
        }
    }
}