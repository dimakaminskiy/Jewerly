using System.Linq;
using System.Net;
using System.Web.Mvc;
using Jewerly.Domain;
using Jewerly.Web.Controllers;

namespace Jewerly.Web.Areas.Admin.Controllers
{
    public class ProductChoiceAttributesController : BaseController
    {
        #region Actions

        public ActionResult Index()
        {
            return View(DataManager.ProductChoiceAttributes.GetAll().ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            [Bind(Include = "ProductChoiceAttributeId,Name,DisplayOrder")] ProductChoiceAttribute productChoiceAttribute)
        {
            if (ModelState.IsValid)
            {
                DataManager.ProductChoiceAttributes.Insert(productChoiceAttribute);
                TempData["message"] = string.Format("Атрибут \"{0}\" был создан", productChoiceAttribute.Name);
                return RedirectToAction("Index");
            }

            return View(productChoiceAttribute);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductChoiceAttribute productChoiceAttribute = DataManager.ProductChoiceAttributes.GetById(id.Value);
            if (productChoiceAttribute == null)
            {
                return HttpNotFound();
            }
            return View(productChoiceAttribute);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            [Bind(Include = "ProductChoiceAttributeId,Name,DisplayOrder")] ProductChoiceAttribute productChoiceAttribute)
        {
            if (ModelState.IsValid)
            {
                DataManager.ProductChoiceAttributes.Edit(productChoiceAttribute);
                TempData["message"] = string.Format("Атрибут \"{0}\" был изменён", productChoiceAttribute.Name);
                return RedirectToAction("Index");
            }
            return View(productChoiceAttribute);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductChoiceAttribute productChoiceAttribute = DataManager.ProductChoiceAttributes.GetById(id.Value);
            if (productChoiceAttribute == null)
            {
                return HttpNotFound();
            }
            return View(productChoiceAttribute);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //ProductChoiceAttribute productChoiceAttribute = DataManager.ProductChoiceAttributes.GetById(id);
            //var avalibleAttrOptions = DataManager.AvalibleChoiceAttributeOptions.SearchFor(t => t.ChoiceAttributeOptionId == id)
            //   .ToList();
            //foreach (var m in avalibleAttrOptions)
            //{
            //    DataManager.AvalibleChoiceAttributeOptions.Delete(m);
            //}


            //var options = DataManager.ChoiceAttributeOptions.SearchFor(t=>t.)
            




            //TempData["message"] = string.Format("Атрибут \"{0}\" был удалён", productChoiceAttribute.Name);

            //DataManager.ProductChoiceAttributes.Delete(productChoiceAttribute);
            return RedirectToAction("Index");
        }

        #endregion

        #region Ctor

        public ProductChoiceAttributesController(DataManager dataManager) : base(dataManager)
        {
        }

        #endregion

    }
}
