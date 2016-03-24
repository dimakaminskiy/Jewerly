using System.Linq;
using System.Net;
using System.Web.Mvc;
using Jewerly.Domain;
using Jewerly.Web.Controllers;

namespace Jewerly.Web.Areas.Admin.Controllers
{
    public class CurrenciesController : BaseController
    {

        #region Ctor

        public CurrenciesController(DataManager dataManager)
            : base(dataManager)
        {
        }

        #endregion

        #region Actions

        public ActionResult Index()
        {
            return View(DataManager.Currencies.GetAll().OrderBy(t => t.DisplayOrder).ThenBy(t => t.Name).ToList());
        }

        public ActionResult Create()
        {
            var model = new Currency
            {
                DisplayOrder = 0,
                Published = true
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            [Bind(Include = "CurrencyId,Name,CurrencyCode,Rate,Published,DisplayOrder")] Currency currency)
        {
            if (ModelState.IsValid)
            {
                currency.Name = currency.Name.Trim();
                DataManager.Currencies.Insert(currency);

                TempData["message"] = string.Format("Валюта \"{0}\" была создана", currency.Name);
                return RedirectToAction("Index");
            }

            return View(currency);
        }

        // GET: Admin/Currencies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Currency currency = DataManager.Currencies.GetById(id.Value);
            if (currency == null)
            {
                return HttpNotFound();
            }
            return View(currency);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            [Bind(Include = "CurrencyId,Name,CurrencyCode,Rate,Published,DisplayOrder")] Currency currency)
        {
            if (ModelState.IsValid)
            {
                DataManager.Currencies.Edit(currency);
                TempData["message"] = string.Format("Изменения в валюте \"{0}\" были сохранены", currency.Name);
                return RedirectToAction("Index");
            }
            return View(currency);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Currency currency = DataManager.Currencies.GetById(id.Value);
            if (currency == null)
            {
                return HttpNotFound();
            }
            return View(currency);
        }

        // POST: Admin/Currencies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Currency currency = DataManager.Currencies.GetById(id);
            DataManager.Currencies.Delete(currency);

            TempData["message"] = string.Format("Валюта \"{0}\" была удалена", currency.Name);
            return RedirectToAction("Index");
        }

        #endregion

    }
}
