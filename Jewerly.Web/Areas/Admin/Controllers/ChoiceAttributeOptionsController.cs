using System.Linq;
using System.Net;
using System.Web.Mvc;
using Jewerly.Domain;
using Jewerly.Web.Controllers;

namespace Jewerly.Web.Areas.Admin.Controllers
{
    public class ChoiceAttributeOptionsController : BaseController
    {

        #region Actions

        public ActionResult Index()
        {
            var choiceAttributeOptions = DataManager.ChoiceAttributeOptions.GetAll()
                .OrderBy(t => t.ProductChoiceAttribute.Name)
                .ThenBy(t => t.ProductChoiceAttribute.DisplayOrder)
                .ThenBy(t => t.Name).ToList();
            return View(choiceAttributeOptions);
        }

        public ActionResult Create()
        {
            var model = new ChoiceAttributeOption {DisplayOrder = 1};
            ViewBag.ProductChoiceAttributeId = new SelectList(DataManager.ProductChoiceAttributes.GetAll().ToList(),
                "ProductChoiceAttributeId", "Name");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            [Bind(Include = "ChoiceAttributeOptionId,ProductChoiceAttributeId,Name,DisplayOrder")] ChoiceAttributeOption
                choiceAttributeOption)
        {
            if (ModelState.IsValid)
            {
                DataManager.ChoiceAttributeOptions.Insert(choiceAttributeOption);

                TempData["message"] = string.Format("Опция \"{0}\" была создана", choiceAttributeOption.Name);

                return RedirectToAction("Index");
            }

            ViewBag.ProductChoiceAttributeId = new SelectList(DataManager.ProductChoiceAttributes.GetAll().ToList(),
                "ProductChoiceAttributeId", "Name", choiceAttributeOption.ProductChoiceAttributeId);
            return View(choiceAttributeOption);
        }

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
            ViewBag.ProductChoiceAttributeId = new SelectList(DataManager.ProductChoiceAttributes.GetAll().ToList(),
                "ProductChoiceAttributeId", "Name", choiceAttributeOption.ProductChoiceAttributeId);
            return View(choiceAttributeOption);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            [Bind(Include = "ChoiceAttributeOptionId,ProductChoiceAttributeId,Name,DisplayOrder")] ChoiceAttributeOption
                choiceAttributeOption)
        {
            if (ModelState.IsValid)
            {
                DataManager.ChoiceAttributeOptions.Insert(choiceAttributeOption);

                TempData["message"] = string.Format("Опция \"{0}\" была отредактирована", choiceAttributeOption.Name);
                return RedirectToAction("Index");
            }
            ViewBag.ProductChoiceAttributeId = new SelectList(DataManager.ProductChoiceAttributes.GetAll().ToList(),
                "ProductChoiceAttributeId", "Name", choiceAttributeOption.ProductChoiceAttributeId);
            return View(choiceAttributeOption);
        }

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


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ChoiceAttributeOption choiceAttributeOption = DataManager.ChoiceAttributeOptions.GetById(id);
            var avalibleAttrOptions =   DataManager.AvalibleChoiceAttributeOptions.SearchFor(t => t.ChoiceAttributeOptionId == id)
                .ToList();
            foreach (var m in avalibleAttrOptions)
            {
                DataManager.AvalibleChoiceAttributeOptions.Delete(m);
            }
            DataManager.ChoiceAttributeOptions.Delete(choiceAttributeOption);           
            TempData["message"] = string.Format("Опция \"{0}\" была удалена", choiceAttributeOption.Name);

            return RedirectToAction("Index");
        }

        #endregion

        #region ctor

        public ChoiceAttributeOptionsController(DataManager dataManager) : base(dataManager)
        {
        }

        #endregion

    }
}
