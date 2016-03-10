using System.Linq;
using System.Net;
using System.Web.Mvc;
using Jewerly.Domain;
using Jewerly.Web.Controllers;

namespace Jewerly.Web.Areas.Admin.Controllers
{
    public class SpecificationAttributeOptionsController : BaseController
    {
       

        // GET: Admin/SpecificationAttributeOptions
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



        // GET: Admin/SpecificationAttributeOptions/Create
        public ActionResult Create()
        {
            var model = new SpecificationAttributeOption();
            model.DisplayOrder = 1;
            ViewBag.ProductSpecificationAttributeId = new SelectList(DataManager.ProductSpecificationAttributes.GetAll().ToList(), "ProductSpecificationAttributeId", "Name");
            return View(model);
        }

        // POST: Admin/SpecificationAttributeOptions/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: Admin/SpecificationAttributeOptions/Edit/5
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

        // POST: Admin/SpecificationAttributeOptions/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: Admin/SpecificationAttributeOptions/Delete/5
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

        // POST: Admin/SpecificationAttributeOptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SpecificationAttributeOption specificationAttributeOption = DataManager.SpecificationAttributeOptions.GetById(id);
            DataManager.SpecificationAttributeOptions.Delete(specificationAttributeOption);

            TempData["message"] = string.Format("Опция Атрибута \"{0}\" была удалена", specificationAttributeOption.Name);
            return RedirectToAction("Index");
        }

       
        public SpecificationAttributeOptionsController(DataManager dataManager) : base(dataManager)
        {
        }
    }
}
