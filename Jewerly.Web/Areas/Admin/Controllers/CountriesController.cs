using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Jewerly.Domain;
using Jewerly.Domain.Entities;
using Jewerly.Web.Controllers;

namespace Jewerly.Web.Areas.Admin.Controllers
{
    public class CountriesController : BaseController
    {
        #region Action

        public ActionResult Index()
        {
            return View(DataManager.Countries.GetAll().ToList());
        }
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Country country)
        {
            if (ModelState.IsValid)
            {
                DataManager.Countries.Insert(country);
                TempData["message"] = string.Format("Страна \"{0}\" была создана", country.Name);
                return RedirectToAction("Index");
            }

            return View(country);
        }

        // GET: Admin/Countries/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Country country = DataManager.Countries.GetById(id.Value);
            if (country == null)
            {
                return HttpNotFound();
            }
            return View(country);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Country country)
        {
            if (ModelState.IsValid)
            {
                DataManager.Countries.Edit(country);
                TempData["message"] = string.Format("Страна \"{0}\" была отредактирована", country.Name);
                return RedirectToAction("Index");
            }
            return View(country);
        }

        // GET: Admin/Countries/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Country country = DataManager.Countries.GetById(id.Value);
            if (country == null)
            {
                return HttpNotFound();
            }
            return View(country);
        }

        // POST: Admin/Countries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Country country = DataManager.Countries.GetById(id);
            using (var db = new ApplicationDbContext())
            {
                if (db.Users.Count(t => t.CountryId == id) != 0)
                {
                    TempData["error"] = "Произошла ошибка при удалении страны. Обнаружены пользователи с этой страны.";
                    return RedirectToAction("Delete", new { id = id });
                }
            }
         
            DataManager.Countries.Delete(country);
            TempData["message"] = string.Format("Страна \"{0}\" была удалена", country.Name);
            return RedirectToAction("Index");
        }

        
        #endregion

        #region ctor

        public CountriesController(DataManager dataManager) : base(dataManager)
        {
        }

        #endregion

    }
}
