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
    public class Products1Controller : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/Products1
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Category).Include(p => p.Discount).Include(p => p.Markup).Include(p => p.Picture);
            return View(products.ToList());
        }

        // GET: Admin/Products1/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Admin/Products1/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            ViewBag.DiscountId = new SelectList(db.Discounts, "Id", "Name");
            ViewBag.MarkupId = new SelectList(db.Markups, "Id", "Name");
            ViewBag.PictureId = new SelectList(db.Pictures, "Id", "Path");
            return View();
        }

        // POST: Admin/Products1/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductId,Name,SeoName,ShortDescription,FullDescription,Price,Published,PictureId,MarkupId,DiscountId,CategoryId")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", product.CategoryId);
            ViewBag.DiscountId = new SelectList(db.Discounts, "Id", "Name", product.DiscountId);
            ViewBag.MarkupId = new SelectList(db.Markups, "Id", "Name", product.MarkupId);
            ViewBag.PictureId = new SelectList(db.Pictures, "Id", "Path", product.PictureId);
            return View(product);
        }

        // GET: Admin/Products1/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", product.CategoryId);
            ViewBag.DiscountId = new SelectList(db.Discounts, "Id", "Name", product.DiscountId);
            ViewBag.MarkupId = new SelectList(db.Markups, "Id", "Name", product.MarkupId);
            ViewBag.PictureId = new SelectList(db.Pictures, "Id", "Path", product.PictureId);
            return View(product);
        }

        // POST: Admin/Products1/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductId,Name,SeoName,ShortDescription,FullDescription,Price,Published,PictureId,MarkupId,DiscountId,CategoryId")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", product.CategoryId);
            ViewBag.DiscountId = new SelectList(db.Discounts, "Id", "Name", product.DiscountId);
            ViewBag.MarkupId = new SelectList(db.Markups, "Id", "Name", product.MarkupId);
            ViewBag.PictureId = new SelectList(db.Pictures, "Id", "Path", product.PictureId);
            return View(product);
        }

        // GET: Admin/Products1/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Admin/Products1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
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
