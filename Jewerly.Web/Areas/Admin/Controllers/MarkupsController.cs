using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Jewerly.Domain;

namespace Jewerly.Web.Areas.Admin.Controllers
{
    public class MarkupsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/Markups
        public ActionResult Index()
        {
            return View(db.Markups.ToList());
        }

        // GET: Admin/Markups/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Markup markup = db.Markups.Find(id);
            if (markup == null)
            {
                return HttpNotFound();
            }
            return View(markup);
        }

        // GET: Admin/Markups/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Markups/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Retail,Trade")] Markup markup)
        {
            if (ModelState.IsValid)
            {
                db.Markups.Add(markup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(markup);
        }

        // GET: Admin/Markups/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Markup markup = db.Markups.Find(id);
            if (markup == null)
            {
                return HttpNotFound();
            }
            return View(markup);
        }

        // POST: Admin/Markups/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Retail,Trade")] Markup markup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(markup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(markup);
        }

        // GET: Admin/Markups/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Markup markup = db.Markups.Find(id);
            if (markup == null)
            {
                return HttpNotFound();
            }
            return View(markup);
        }

        // POST: Admin/Markups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Markup markup = db.Markups.Find(id);
            db.Markups.Remove(markup);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
