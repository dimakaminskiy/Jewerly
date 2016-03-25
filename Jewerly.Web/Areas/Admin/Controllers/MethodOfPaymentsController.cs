using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Jewerly.Domain;
using Jewerly.Domain.Entities;
using Jewerly.Web.Controllers;

namespace Jewerly.Web.Areas.Admin.Controllers
{
    public class MethodOfPaymentsController : BaseController
    {

        #region  Action
        
        public ActionResult Index()
        {
            return View(DataManager.MethodOfPayments.GetAll());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Available,DisplayOrder")] MethodOfPayment methodOfPayment)
        {
            if (ModelState.IsValid)
            {
                DataManager.MethodOfPayments.Insert(methodOfPayment);
                TempData["message"] = string.Format("Способ оплаты \"{0}\" был создан", methodOfPayment.Name);
                return RedirectToAction("Index");
            }

            return View(methodOfPayment);
        }

        // GET: Admin/MethodOfPayments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MethodOfPayment methodOfPayment = DataManager.MethodOfPayments.GetById(id.Value);
            if (methodOfPayment == null)
            {
                return HttpNotFound();
            }
            return View(methodOfPayment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Available,DisplayOrder")] MethodOfPayment methodOfPayment)
        {
            if (ModelState.IsValid)
            {
                DataManager.MethodOfPayments.Edit(methodOfPayment);
                TempData["message"] = string.Format("Способ оплаты \"{0}\" был изменен", methodOfPayment.Name);

                return RedirectToAction("Index");
            }
            return View(methodOfPayment);
        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MethodOfPayment methodOfPayment = DataManager.MethodOfPayments.GetById(id.Value);
            if (methodOfPayment == null)
            {
                return HttpNotFound();
            }
          
            return View(methodOfPayment);
        }

        // POST: Admin/MethodOfPayments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MethodOfPayment methodOfPayment = DataManager.MethodOfPayments.GetById(id);

            if (DataManager.Orders.SearchFor(t => t.MethodOfPaymentId == id).Count() != 0)
            {
                TempData["error"] = "Произошла ошибка при удалении. Обнаружены заказы с этим способом оплаты.";
                return RedirectToAction("Delete", new { id = id });
            }

            DataManager.MethodOfPayments.Delete(methodOfPayment);
            TempData["message"] = string.Format("Способ оплаты \"{0}\" был удален", methodOfPayment.Name);

            return RedirectToAction("Index");
        }

        #endregion

        #region ctor

        public MethodOfPaymentsController(DataManager dataManager) : base(dataManager)
        {
        }

        #endregion

    }
}
