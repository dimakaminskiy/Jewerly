using System.Linq;
using System.Net;
using System.Web.Mvc;
using Jewerly.Domain;
using Jewerly.Web.Controllers;

namespace Jewerly.Web.Areas.Admin.Controllers
{
    public class ProductSpecificationAttributesController : BaseController
    {
        #region Actions

        public ActionResult Index()
        {
            return
                View(
                    DataManager.ProductSpecificationAttributes.GetAll()
                        .OrderBy(t => t.DisplayOrder)
                        .ThenBy(t => t.Name)
                        .ToList());
        }

        public ActionResult Create()
        {
            var model = new ProductSpecificationAttribute();
            model.DisplayOrder = 1;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            [Bind(Include = "ProductSpecificationAttributeId,Name,SeoName,AllowFiltering,DisplayOrder")] ProductSpecificationAttribute productSpecificationAttribute)
        {
            if (ModelState.IsValid)
            {
                if (productSpecificationAttribute.DisplayOrder < 1)
                {
                    productSpecificationAttribute.DisplayOrder = 1;
                }

                productSpecificationAttribute.Name = productSpecificationAttribute.Name.Trim();
                DataManager.ProductSpecificationAttributes.Insert(productSpecificationAttribute);
                TempData["message"] = string.Format("Атрибут \"{0}\" был создан", productSpecificationAttribute.Name);

                return RedirectToAction("Index");
            }

            return View(productSpecificationAttribute);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductSpecificationAttribute productSpecificationAttribute =
                DataManager.ProductSpecificationAttributes.GetById(id.Value);
            if (productSpecificationAttribute == null)
            {
                return HttpNotFound();
            }
            return View(productSpecificationAttribute);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            [Bind(Include = "ProductSpecificationAttributeId,Name,SeoName,AllowFiltering,DisplayOrder")] ProductSpecificationAttribute productSpecificationAttribute)
        {
            if (ModelState.IsValid)
            {
                if (productSpecificationAttribute.DisplayOrder < 1)
                {
                    productSpecificationAttribute.DisplayOrder = 1;
                }

                productSpecificationAttribute.Name = productSpecificationAttribute.Name.Trim();
                DataManager.ProductSpecificationAttributes.Edit(productSpecificationAttribute);

                TempData["message"] = string.Format("Изменения в атрибуте \"{0}\" были сохранены",
                    productSpecificationAttribute.Name);
                return RedirectToAction("Index");
            }
            return View(productSpecificationAttribute);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductSpecificationAttribute productSpecificationAttribute =
                DataManager.ProductSpecificationAttributes.GetById(id.Value);
            if (productSpecificationAttribute == null)
            {
                return HttpNotFound();
            }
            return View(productSpecificationAttribute);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductSpecificationAttribute productSpecificationAttribute =
            DataManager.ProductSpecificationAttributes.GetById(id);
            
            DataManager.ProductSpecificationAttributes.Delete(productSpecificationAttribute);
            TempData["message"] = string.Format("Атрибут \"{0}\" был удален", productSpecificationAttribute.Name);
            return RedirectToAction("Index");
        }

        #endregion
        
        #region ctor

        public ProductSpecificationAttributesController(DataManager dataManager) : base(dataManager)
        {
        }

        #endregion

    }
}
