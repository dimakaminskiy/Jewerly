using System.Linq;
using System.Net;
using System.Web.Mvc;
using Jewerly.Domain;
using Jewerly.Domain.Entities;
using Jewerly.Web.Controllers;

namespace Jewerly.Web.Areas.Admin.Controllers
{
    public class MethodOfDeliveriesController : BaseController
    {
        #region Actions

        public ActionResult Index()
        {
            return View(DataManager.MethodOfDeliveries.GetAll());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Available,DisplayOrder")] MethodOfDelivery methodOfDelivery)
        {
            if (ModelState.IsValid)
            {
                DataManager.MethodOfDeliveries.Insert(methodOfDelivery);
                TempData["message"] = string.Format("Способ доставки \"{0}\" был создан", methodOfDelivery.Name);
                return RedirectToAction("Index");
            }

            return View(methodOfDelivery);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MethodOfDelivery methodOfDelivery = DataManager.MethodOfDeliveries.GetById(id.Value);
            if (methodOfDelivery == null)
            {
                return HttpNotFound();
            }
            return View(methodOfDelivery);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Available,DisplayOrder")] MethodOfDelivery methodOfDelivery)
        {
            if (ModelState.IsValid)
            {
                DataManager.MethodOfDeliveries.Edit(methodOfDelivery);
                TempData["message"] = string.Format("Способ доставки \"{0}\" был изменен", methodOfDelivery.Name);
                return RedirectToAction("Index");
            }
            return View(methodOfDelivery);
        }

        // GET: Admin/MethodOfDeliveries/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MethodOfDelivery methodOfDelivery = DataManager.MethodOfDeliveries.GetById(id.Value);
            if (methodOfDelivery == null)
            {
                return HttpNotFound();
            }
            var error = Request.Params["msg"];
            if (!string.IsNullOrEmpty(error))
            {
                ModelState.AddModelError("", error);
            }
            return View(methodOfDelivery);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MethodOfDelivery methodOfDelivery = DataManager.MethodOfDeliveries.GetById(id);

            if (DataManager.Orders.SearchFor(t => t.MethodOfDeliveryId == id).Count() != 0)
            {
                return RedirectToAction("Delete",
                    new {msg = "Произошла ошибка при удалении. Обнаружены заказы с этим способом доставки."});
            }

            DataManager.MethodOfDeliveries.Delete(methodOfDelivery);
            TempData["message"] = string.Format("Способ доставки \"{0}\" был удален", methodOfDelivery.Name);

            return RedirectToAction("Index");
        }

        #endregion

        #region ctor

        public MethodOfDeliveriesController(DataManager dataManager) : base(dataManager)
        {
        }

        #endregion

    }
}
