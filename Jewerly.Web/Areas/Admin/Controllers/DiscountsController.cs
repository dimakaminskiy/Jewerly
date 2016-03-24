using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Jewerly.Domain;
using Jewerly.Web.Controllers;


namespace Jewerly.Web.Areas.Admin.Controllers
{
    public class DiscountsController : BaseController
    {
        public ActionResult Index()
        {
            return View(DataManager.Discounts.GetAll().ToList());
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Value")] Discount discount)
        {
            if (ModelState.IsValid)
            {
                DataManager.Discounts.Insert(discount);
                TempData["message"] = string.Format("Скидка \"{0}\" была создана", discount.Name);
                return RedirectToAction("Index");
            }

            return View(discount);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Discount discount = DataManager.Discounts.GetById(id.Value);
            if (discount == null)
            {
                return HttpNotFound();
            }
            return View(discount);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Value")] Discount discount)
        {
            if (ModelState.IsValid)
            {
                DataManager.Discounts.Edit(discount);
                TempData["message"] = string.Format("Скидка \"{0}\" была отредактирована", discount.Name);

                return RedirectToAction("Index");
            }
            return View(discount);
        }

        // GET: Admin/Discounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Discount discount = DataManager.Discounts.GetById(id.Value);
            if (discount == null)
            {
                return HttpNotFound();
            }
            return View(discount);
        }

        // POST: Admin/Discounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Discount discount = DataManager.Discounts.GetById(id);
            //удаление скидок на продуктах
            var list = DataManager.Products.SearchFor(t => t.DiscountId == discount.Id).ToList();
            foreach (var p in list)
            {
                p.DiscountId = null;
                DataManager.Products.Edit(p);
            }

            DataManager.Discounts.Delete(discount);
            TempData["message"] = string.Format("Скидка \"{0}\" была удалена", discount.Name);
            return RedirectToAction("Index");
        }

       

        public DiscountsController(DataManager dataManager) : base(dataManager)
        {
        }
    }
}
