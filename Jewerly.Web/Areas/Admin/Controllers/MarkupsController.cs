using System.Linq;
using System.Net;
using System.Web.Mvc;
using Jewerly.Domain;
using Jewerly.Web.Controllers;


namespace Jewerly.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class MarkupsController : BaseController
    {
        #region Actions

        public ActionResult Index()
        {
            return View(DataManager.Markups.GetAll().ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Retail,Trade")] Markup markup)
        {
            if (ModelState.IsValid)
            {
                DataManager.Markups.Insert(markup);
                TempData["message"] = string.Format("Наценка \"{0}\" была создана", markup.Name);
                return RedirectToAction("Index");
            }

            return View(markup);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Markup markup = DataManager.Markups.GetById(id.Value);
            if (markup == null)
            {
                return HttpNotFound();
            }
            return View(markup);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Retail,Trade")] Markup markup)
        {
            if (ModelState.IsValid)
            {
                DataManager.Markups.Edit(markup);
                TempData["message"] = string.Format("Наценка \"{0}\" была отредактирована", markup.Name);
                return RedirectToAction("Index");
            }
            return View(markup);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Markup markup = DataManager.Markups.GetById(id.Value);
            if (markup == null)
            {
                return HttpNotFound();
            }
             return View(markup);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (DataManager.Products.SearchFor(t => t.MarkupId == id).Count() != 0)
            {
                TempData["error"] = "Произошла ошибка при удалении наценки. Обнаружены продукты с этой наценкой.";
                return RedirectToAction("Delete", new { id = id });
             }


            Markup markup = DataManager.Markups.GetById(id);
            TempData["message"] = string.Format("Наценка \"{0}\" была создана", markup.Name);
            DataManager.Markups.Delete(markup);

            return RedirectToAction("Index");
        }

        #endregion

        #region Ctor

        public MarkupsController(DataManager dataManager) : base(dataManager)
        {
        }

        #endregion

    }
}
