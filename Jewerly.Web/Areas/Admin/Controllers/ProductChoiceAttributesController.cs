using System.Linq;
using System.Net;
using System.Web.Mvc;
using Jewerly.Domain;
using Jewerly.Web.Controllers;

namespace Jewerly.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ProductChoiceAttributesController : BaseController
    {
        #region Actions

        public ActionResult Index()
        {
            return View(DataManager.ProductChoiceAttributes.GetAll().ToList());
        }

        public ActionResult Create()
        {
            ProductChoiceAttribute productChoiceAttribute = new ProductChoiceAttribute
            {
                DisplayOrder = 1
            };
            return View(productChoiceAttribute);
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

            var list =
                DataManager.MappingProductChoiceAttributeToProducts.SearchFor(t => t.ProductChoiceAttributeId == id)
                    .ToList();

            foreach (var m  in list)
            {
                DataManager.MappingProductChoiceAttributeToProducts.Delete(m);
            }
            var options = DataManager.ChoiceAttributeOptions.SearchFor(t => t.ProductChoiceAttributeId == id).ToList();

            foreach (var option in options)
            {
                DataManager.ChoiceAttributeOptions.Delete(option);
            }
            var attr = DataManager.ProductChoiceAttributes.GetById(id);
            DataManager.ProductChoiceAttributes.Delete(attr);
            TempData["message"] = string.Format("Атрибут \"{0}\" был удалён", attr.Name);
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
