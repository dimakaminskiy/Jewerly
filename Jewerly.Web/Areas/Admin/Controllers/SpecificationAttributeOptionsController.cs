using System.Linq;
using System.Net;
using System.Web.Mvc;
using Jewerly.Domain;
using Jewerly.Web.Controllers;

namespace Jewerly.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class SpecificationAttributeOptionsController : BaseController
    {
       
        public ActionResult Index()
        {
            var specificationAttributeOptions =
                DataManager.SpecificationAttributeOptions.GetAll()
                    .OrderBy(t => t.ProductSpecificationAttribute.Name)
                    .ThenBy(t => t.Name)
                    .ThenBy(t => t.DisplayOrder)
                    .ToList();
            return View(specificationAttributeOptions);
        }
        public ActionResult Create()
        {
            var model = new SpecificationAttributeOption();
            model.DisplayOrder = 1;
            ViewBag.ProductSpecificationAttributeId = new SelectList(DataManager.ProductSpecificationAttributes.GetAll().ToList(), "ProductSpecificationAttributeId", "Name");
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SpecificationAttributeOptionId,ProductSpecificationAttributeId,Name,DisplayOrder")] SpecificationAttributeOption specificationAttributeOption)
        {
            if (ModelState.IsValid)
            {
                if (specificationAttributeOption.DisplayOrder < 1)
                {
                    specificationAttributeOption.DisplayOrder = 1;
                }
                specificationAttributeOption.Name = specificationAttributeOption.Name.Trim();
                
                DataManager.SpecificationAttributeOptions.Insert(specificationAttributeOption);

                TempData["message"] = string.Format("Опция Атрибута \"{0}\" была создана", specificationAttributeOption.Name);
                return RedirectToAction("Index");
            }

            ViewBag.ProductSpecificationAttributeId = new SelectList(DataManager.ProductSpecificationAttributes.GetAll().ToList(), "ProductSpecificationAttributeId", "Name", specificationAttributeOption.ProductSpecificationAttributeId);
            return View(specificationAttributeOption);
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SpecificationAttributeOption specificationAttributeOption = DataManager.SpecificationAttributeOptions.GetById(id.Value);
            if (specificationAttributeOption == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductSpecificationAttributeId = new SelectList(DataManager.ProductSpecificationAttributes.GetAll().ToList(), "ProductSpecificationAttributeId", "Name", specificationAttributeOption.ProductSpecificationAttributeId);
            return View(specificationAttributeOption);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SpecificationAttributeOptionId,ProductSpecificationAttributeId,Name,DisplayOrder")] SpecificationAttributeOption specificationAttributeOption)
        {
            if (ModelState.IsValid)
            {
                if (specificationAttributeOption.DisplayOrder < 1)
                {
                    specificationAttributeOption.DisplayOrder = 1;
                }
                specificationAttributeOption.Name = specificationAttributeOption.Name.Trim();
                
                DataManager.SpecificationAttributeOptions.Edit(specificationAttributeOption);

               
               TempData["message"] = string.Format("Изменение опция Атрибута \"{0}\" были сохранены", specificationAttributeOption.Name);
               return RedirectToAction("Index");
            }
            ViewBag.ProductSpecificationAttributeId = new SelectList(DataManager.ProductSpecificationAttributes.GetAll().ToList(), "ProductSpecificationAttributeId", "Name", specificationAttributeOption.ProductSpecificationAttributeId);
            return View(specificationAttributeOption);
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SpecificationAttributeOption specificationAttributeOption = DataManager.SpecificationAttributeOptions.GetById(id.Value);
            if (specificationAttributeOption == null)
            {
                return HttpNotFound();
            }
            return View(specificationAttributeOption);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SpecificationAttributeOption specificationAttributeOption = DataManager.SpecificationAttributeOptions.GetById(id);

            var list =
                DataManager.MappingProductSpecificationAttributeToProducts.SearchFor(
                    t => t.SpecificationAttributeOptionId == id).ToList();


            foreach (var m in list)
            {
                DataManager.MappingProductSpecificationAttributeToProducts.Delete(m);
            }




            DataManager.SpecificationAttributeOptions.Delete(specificationAttributeOption);

            TempData["message"] = string.Format("Опция Атрибута \"{0}\" была удалена", specificationAttributeOption.Name);
            return RedirectToAction("Index");
        }

       
        public SpecificationAttributeOptionsController(DataManager dataManager) : base(dataManager)
        {
        }
    }
}
