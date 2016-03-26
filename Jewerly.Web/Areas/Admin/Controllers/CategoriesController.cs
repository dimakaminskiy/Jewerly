using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Jewerly.Domain;
using Jewerly.Web.Controllers;
using Jewerly.Web.extensions;

namespace Jewerly.Web.Areas.Admin.Controllers
{
   [Authorize(Roles = "Administrator")]
    public class CategoriesController : BaseController
    {
        #region ctor

        public CategoriesController(DataManager dataManager)
            : base(dataManager)
        {
        }

        #endregion

        #region Helpers

        private SelectList GetSelectListCategoryPictures(int? id)
        {
            SelectList list;

            if (id == null)
            {
                list =
                    new SelectList(DataManager.CategoryPictures.GetAll().OrderBy(x => x.Caption), "Id", "Caption")
                        .PreAppend("-----------", "", true);
            }
            else
            {
                list =
                    new SelectList(DataManager.CategoryPictures.GetAll().OrderBy(x => x.Caption), "Id", "Caption", id)
                        .PreAppend("-----------", "", false);
            }
            return list;
        }

        private SelectList GetSelectListCategory(int? id)
        {
            SelectList list;

            if (id == null)
            {
                list =
                    new SelectList(DataManager.Categories.GetAll().OrderBy(x => x.Name), "Id", "Name")
                        .PreAppend("-----------", "", true);
            }
            else
            {
                list =
                    new SelectList(DataManager.Categories.GetAll().OrderBy(x => x.Name), "Id", "Name", id)
                        .PreAppend("-----------", "", false);
            }
            return list;
        }


        #endregion

        #region Actions

        public ActionResult Index()
        {
            List<Category> result = new List<Category>();


            var topCategories =
                DataManager.Categories.SearchFor(t => t.ParentCategoryId == null).OrderBy(t => t.Name).ToList();

            foreach (Category category in topCategories)
            {
                result.Add(category);
                var childrenCategories =
                    DataManager.Categories.SearchFor(t => t.ParentCategoryId == category.Id).OrderBy(t => t.Name);
                result.AddRange(childrenCategories);
            }

            return View(result);
        }

        public ActionResult Create()
        {
            var model = new Category();
            model.Published = true;
            model.ShowOnHomePage = true;
            ViewBag.CategoryPictureId = GetSelectListCategoryPictures(model.CategoryPictureId);
            ViewBag.ParentCategoryId = GetSelectListCategory(model.ParentCategoryId);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            [Bind(Include = "Id,Name,ProductName,SeoName,Published,ShowOnHomePage,ParentCategoryId,CategoryPictureId")] Category
                category)
        {
            if (ModelState.IsValid)
            {
                category.Name = category.Name.Trim();
                category.SeoName = string.IsNullOrEmpty(category.SeoName)
                    ? category.Name.ToTranslit()
                    : category.SeoName.Trim();
                DataManager.Categories.Insert(category);

                TempData["message"] = string.Format("Категория \"{0}\" была создана", category.Name);
                return RedirectToAction("Index");
            }
            ViewBag.ParentCategoryId = GetSelectListCategory(category.ParentCategoryId);
            ViewBag.CategoryPictureId = GetSelectListCategoryPictures(category.CategoryPictureId);
            return View(category);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = DataManager.Categories.GetById(id.Value);
            if (category == null)
            {
                return HttpNotFound();
            }
            ViewBag.ParentCategoryId = GetSelectListCategory(category.ParentCategoryId);
            ViewBag.CategoryPictureId = GetSelectListCategoryPictures(category.CategoryPictureId);
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            [Bind(Include = "Id,Name,ProductName,SeoName,Published,ShowOnHomePage,ParentCategoryId,CategoryPictureId")] Category
                category)
        {
            if (ModelState.IsValid)
            {
                category.Name = category.Name.Trim();
                if (!string.IsNullOrEmpty(category.SeoName))
                {
                    category.SeoName = category.Name.ToTranslit();
                }
                DataManager.Categories.Edit(category);

                TempData["message"] = string.Format("Изменения в категории \"{0}\" были сохранены", category.Name);
                return RedirectToAction("Index");
            }
            ViewBag.ParentCategoryId = GetSelectListCategory(category.ParentCategoryId);
            ViewBag.CategoryPictureId = GetSelectListCategoryPictures(category.CategoryPictureId);
            return View(category);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = DataManager.Categories.GetById(id.Value);
            if (category == null)
            {
                return HttpNotFound();
            }


            var error = Request.Params["msg"];
            if (!string.IsNullOrEmpty(error))
            {
                ModelState.AddModelError("", error);
            }


            return View(category);
        }

        // POST: Admin/Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = DataManager.Categories.GetById(id);

            if (DataManager.Products.SearchFor(t => t.CategoryId == id).Count() != 0)
            {
                TempData["error"] = string.Format("В категории  \"{0}\" обнаружены товары. Удаление невозможно.", category.Name);
                return RedirectToAction("Delete", new { id = id });
            }
            if (DataManager.Categories.SearchFor(t => t.ParentCategoryId == id).Count() != 0)
            {
                TempData["error"] = string.Format("В категории  \"{0}\" обнаружены подкатегории. Удаление невозможно.", category.Name);
                return RedirectToAction("Delete", new { id = id });
            }

            DataManager.Categories.Delete(category);
            TempData["message"] = string.Format("Категории \"{0}\" была удалена", category.Name);
            return RedirectToAction("Index");
        }

        #endregion

    }
}
