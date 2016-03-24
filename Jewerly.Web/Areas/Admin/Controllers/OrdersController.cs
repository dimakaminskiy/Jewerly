using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Jewerly.Domain;
using Jewerly.Domain.Entities;
using Jewerly.Web.Controllers;
using Microsoft.AspNet.Identity;

namespace Jewerly.Web.Areas.Admin.Controllers
{
    public class OrdersController : BaseController
    {

        // GET: Admin/Orders
        public ActionResult Index(int? orderStatusId, int page = 1)
        {


            //foreach (var order in DataManager.Orders.GetAll().ToList())
            //{
            //    order.OrderStatusId = 2;
            //    DataManager.Orders.Edit(order);
            //}





            IQueryable<Order> orders = DataManager.Orders.GetAll();

            if (orderStatusId.HasValue)
            {
                orders = orders.Where(t => t.OrderStatusId == orderStatusId.Value);
            }
            else
            {
                orderStatusId = 1;
                orders = orders.Where(t => t.OrderStatusId==1);
            }
            orders = orders.OrderBy(t => t.Id).Include(o => o.Country).Include(o => o.MethodOfDelivery).Include(o => o.MethodOfPayment).Include(o => o.OrderStatus);
            var count = orders.Count();
            int countItemOnpage = 1;

            orders = orders.Skip((page - 1) * countItemOnpage)
                .Take(countItemOnpage);

            var orderStutuses = DataManager.OrderStatuses.GetAll().ToList();

            ViewBag.PageNo = page;
            ViewBag.CountPage = (int)decimal.Remainder(count, countItemOnpage) == 0 ? count / countItemOnpage : count / countItemOnpage + 1;
            ViewBag.OrderStatusId = orderStatusId;
            ViewBag.OrderStatuses = new SelectList(orderStutuses, "Id", "Name", 1);




            ViewBag.StutusName = orderStutuses.Single(t => t.Id == orderStatusId).Name;




            //          var orders = DataManager.Orders.GetAll().Include(o => o.Country).Include(o => o.MethodOfDelivery).Include(o => o.MethodOfPayment).Include(o => o.OrderStatus);
                    return View(orders.ToList());
        }


     

        // GET: Admin/Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = DataManager.Orders.GetById(id.Value);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.CountryId = new SelectList(DataManager.Countries.GetAll(), "Id", "Name", order.CountryId);
            ViewBag.MethodOfDeliveryId = new SelectList(DataManager.MethodOfDeliveries.GetAll(), "Id", "Name", order.MethodOfDeliveryId);
            ViewBag.MethodOfPaymentId = new SelectList(DataManager.MethodOfPayments.GetAll(), "Id", "Name", order.MethodOfPaymentId);
            ViewBag.OrderStatusId = new SelectList(DataManager.OrderStatuses.GetAll(), "Id", "Name", order.OrderStatusId);
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
             DataManager.Orders.Edit(order);
                return RedirectToAction("Index");
            }

            var list =
                DataManager.OrderDetails.SearchFor(t => t.OrderId == order.Id)
                    .Include(t => t.ProductId)
                    .Include(t => t.Product.Picture);

            order.OrderDetails = list.ToList();

            ViewBag.CountryId = new SelectList(DataManager.Countries.GetAll(), "Id", "Name", order.CountryId);
            ViewBag.MethodOfDeliveryId = new SelectList(DataManager.MethodOfDeliveries.GetAll(), "Id", "Name", order.MethodOfDeliveryId);
            ViewBag.MethodOfPaymentId = new SelectList(DataManager.MethodOfPayments.GetAll(), "Id", "Name", order.MethodOfPaymentId);
            ViewBag.OrderStatusId = new SelectList(DataManager.OrderStatuses.GetAll(), "Id", "Name", order.OrderStatusId);
            return View(order);
        }

        // GET: Admin/Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = DataManager.Orders.GetById(id.Value);
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
            Order order = DataManager.Orders.GetById(id);
            var orderDetails = DataManager.OrderDetails.SearchFor(t => t.OrderId == order.Id);
            foreach (var detail in orderDetails)
            {
                DataManager.OrderDetails.Delete(detail);
            }
            DataManager.Orders.Delete(order);

            return RedirectToAction("Index");
        }

        

        public OrdersController(DataManager dataManager) : base(dataManager)
        {
        }
    }
}
