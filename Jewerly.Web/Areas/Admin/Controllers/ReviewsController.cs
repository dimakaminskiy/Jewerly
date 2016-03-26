using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Jewerly.Domain;
using Jewerly.Web.Controllers;

namespace Jewerly.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ReviewsController : BaseController
    {
       

        // GET: Admin/Reviews
        public ActionResult Index()
        {
            return View(DataManager.Reviews.GetAll().OrderBy(t=>t.Date).ToList());
        }

        public ActionResult Create()
        {
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Text")] Review review)
        {
            if (ModelState.IsValid)
            {
                review.Date= DateTime.Now;
                TempData["message"] = string.Format("Отзыв от \"{0}\" был создан", review.Name);
               DataManager.Reviews.Insert(review);
               return RedirectToAction("Index");
            }

            return View(review);
        }

        // GET: Admin/Reviews/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = DataManager.Reviews.GetById(id.Value);
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }

        // POST: Admin/Reviews/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Text")] Review review)
        {
            if (ModelState.IsValid)
            {
                review.Date = DateTime.Now;
               DataManager.Reviews.Edit(review);
               TempData["message"] = string.Format("Отзыв от \"{0}\" был отредактирован", review.Name);
                return RedirectToAction("Index");
            }
            return View(review);
        }

        // GET: Admin/Reviews/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = DataManager.Reviews.GetById(id.Value);
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }

        // POST: Admin/Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Review review = DataManager.Reviews.GetById(id);
            DataManager.Reviews.Delete(review);
            TempData["message"] = string.Format("Отзыв от \"{0}\" был удален", review.Name);
            return RedirectToAction("Index");
        }

        

        public ReviewsController(DataManager dataManager) : base(dataManager)
        {
        }
    }
}
