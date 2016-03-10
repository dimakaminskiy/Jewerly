using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Jewerly.Domain;
using Jewerly.Domain.Entities;

namespace Jewerly.Web.Areas.Admin.Controllers
{
    public class OrdersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/Orders
        public ActionResult Index()
        {
            var orders = db.Orders.Include(o => o.Country).Include(o => o.MethodOfDelivery).Include(o => o.MethodOfPayment).Include(o => o.OrderStatus);
            return View(orders.ToList());
        }

        // GET: Admin/Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Admin/Orders/Create
        public ActionResult Create()
        {
            ViewBag.CountryId = new SelectList(db.Countries, "Id", "Name");
            ViewBag.MethodOfDeliveryId = new SelectList(db.MethodOfDeliveries, "Id", "Name");
            ViewBag.MethodOfPaymentId = new SelectList(db.MethodOfPayments, "Id", "Name");
            ViewBag.OrderStatusId = new SelectList(db.OrderStatuses, "Id", "Name");
            return View();
        }

        // POST: Admin/Orders/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,OrderDate,FirstName,LastName,MiddleName,Phone,Email,Total,CountryId,City,TextInfo,OrderStatusId,MethodOfPaymentId,MethodOfDeliveryId")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CountryId = new SelectList(db.Countries, "Id", "Name", order.CountryId);
            ViewBag.MethodOfDeliveryId = new SelectList(db.MethodOfDeliveries, "Id", "Name", order.MethodOfDeliveryId);
            ViewBag.MethodOfPaymentId = new SelectList(db.MethodOfPayments, "Id", "Name", order.MethodOfPaymentId);
            ViewBag.OrderStatusId = new SelectList(db.OrderStatuses, "Id", "Name", order.OrderStatusId);
            return View(order);
        }

        // GET: Admin/Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.CountryId = new SelectList(db.Countries, "Id", "Name", order.CountryId);
            ViewBag.MethodOfDeliveryId = new SelectList(db.MethodOfDeliveries, "Id", "Name", order.MethodOfDeliveryId);
            ViewBag.MethodOfPaymentId = new SelectList(db.MethodOfPayments, "Id", "Name", order.MethodOfPaymentId);
            ViewBag.OrderStatusId = new SelectList(db.OrderStatuses, "Id", "Name", order.OrderStatusId);
            return View(order);
        }

        // POST: Admin/Orders/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,OrderDate,FirstName,LastName,MiddleName,Phone,Email,Total,CountryId,City,TextInfo,OrderStatusId,MethodOfPaymentId,MethodOfDeliveryId")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CountryId = new SelectList(db.Countries, "Id", "Name", order.CountryId);
            ViewBag.MethodOfDeliveryId = new SelectList(db.MethodOfDeliveries, "Id", "Name", order.MethodOfDeliveryId);
            ViewBag.MethodOfPaymentId = new SelectList(db.MethodOfPayments, "Id", "Name", order.MethodOfPaymentId);
            ViewBag.OrderStatusId = new SelectList(db.OrderStatuses, "Id", "Name", order.OrderStatusId);
            return View(order);
        }

        // GET: Admin/Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Admin/Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
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
