using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Jewerly.Domain;
using Jewerly.Web.Controllers;

namespace Jewerly.Web.Areas.Admin.Controllers
{
    public class ChoiceAttributeOptionsController : BaseController
    {
       

        // GET: Admin/ChoiceAttributeOptions
        public ActionResult Index()
        {
            var choiceAttributeOptions = DataManager.ChoiceAttributeOptions.GetAll()
                .OrderBy(t => t.ProductChoiceAttribute.DisplayOrder)
                .ThenBy(t => t.ProductChoiceAttribute.Name)
                .ThenBy(t=>t.DisplayOrder).ToList();
            return View(choiceAttributeOptions);
        }

      // GET: Admin/ChoiceAttributeOptions/Create
        public ActionResult Create()
        {

            var model = new ChoiceAttributeOption { DisplayOrder = 1 };
            ViewBag.ProductChoiceAttributeId = new SelectList(DataManager.ProductChoiceAttributes.GetAll().ToList(), "ProductChoiceAttributeId", "Name");
            return View(model);
        }

        // POST: Admin/ChoiceAttributeOptions/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ChoiceAttributeOptionId,ProductChoiceAttributeId,Name,DisplayOrder")] ChoiceAttributeOption choiceAttributeOption)
        {
            if (ModelState.IsValid)
            {
                DataManager.ChoiceAttributeOptions.Insert(choiceAttributeOption);

                TempData["message"] = string.Format("Опция \"{0}\" была создана", choiceAttributeOption.Name);

                return RedirectToAction("Index");
            }

            ViewBag.ProductChoiceAttributeId = new SelectList(DataManager.ProductChoiceAttributes.GetAll().ToList(), "ProductChoiceAttributeId", "Name", choiceAttributeOption.ProductChoiceAttributeId);
            return View(choiceAttributeOption);
        }

        // GET: Admin/ChoiceAttributeOptions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChoiceAttributeOption choiceAttributeOption = DataManager.ChoiceAttributeOptions.GetById(id.Value);
            if (choiceAttributeOption == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductChoiceAttributeId = new SelectList(DataManager.ProductChoiceAttributes.GetAll().ToList(), "ProductChoiceAttributeId", "Name", choiceAttributeOption.ProductChoiceAttributeId);
            return View(choiceAttributeOption);
        }

        // POST: Admin/ChoiceAttributeOptions/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ChoiceAttributeOptionId,ProductChoiceAttributeId,Name,DisplayOrder")] ChoiceAttributeOption choiceAttributeOption)
        {
            if (ModelState.IsValid)
            {
                DataManager.ChoiceAttributeOptions.Insert(choiceAttributeOption);

                TempData["message"] = string.Format("Опция \"{0}\" была отредактирована", choiceAttributeOption.Name);
                return RedirectToAction("Index");
            }
            ViewBag.ProductChoiceAttributeId = new SelectList(DataManager.ProductChoiceAttributes.GetAll().ToList(), "ProductChoiceAttributeId", "Name", choiceAttributeOption.ProductChoiceAttributeId);
            return View(choiceAttributeOption);
        }

        // GET: Admin/ChoiceAttributeOptions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChoiceAttributeOption choiceAttributeOption = DataManager.ChoiceAttributeOptions.GetById(id.Value);
            if (choiceAttributeOption == null)
            {
                return HttpNotFound();
            }
            return View(choiceAttributeOption);
        }

        // POST: Admin/ChoiceAttributeOptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
          
            ChoiceAttributeOption choiceAttributeOption = DataManager.ChoiceAttributeOptions.GetById(id);

            DataManager.ChoiceAttributeOptions.Delete(choiceAttributeOption);

            TempData["message"] = string.Format("Опция \"{0}\" была удалена", choiceAttributeOption.Name);

            return RedirectToAction("Index");
        }

      

        public ChoiceAttributeOptionsController(DataManager dataManager) : base(dataManager)
        {
        }
    }
}
