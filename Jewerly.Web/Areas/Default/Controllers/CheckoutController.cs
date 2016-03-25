using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Jewerly.Domain;
using Jewerly.Domain.Entities;
using Jewerly.Web.Controllers;
using Jewerly.Web.Models;
using Jewerly.Web.Utils;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace Jewerly.Web.Areas.Default.Controllers
{
    public class CheckoutController : BaseController
    {
        public ActionResult AddressAndPayment()
        {
            var cart = ShoppingCart.GetCart(HttpContext, DataManager);
            if (cart.GetCartsCount() == 0) return RedirectToAction("Index", "ShoppingCart");

            var mPayId = DataManager.MethodOfPayments.GetAll().OrderBy(x => x.Id).First().Id;
            var mDelId = DataManager.MethodOfDeliveries.GetAll().OrderBy(x => x.Id).First().Id;
            //int currencyId = GetCurrentCurrency();

            //var currency = DataManager.Currencies.SearchFor(t => t.CurrencyId == currencyId).Single();
            //var cartItems = cart.GetCarts();
           
           

            //var model = new Order()
            //{
            //    MethodOfDeliveryId = mDelId,
            //    MethodOfPaymentId = mPayId,
            //};

            var model = new OrderModel()
            {
                MethodOfDeliveryId = mDelId,
                MethodOfPaymentId = mPayId,
            };

            if (User.Identity.IsAuthenticated)
            {
                var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = userManager.FindById(User.Identity.GetUserId());

                model.FirstName = user.FirstName;
                model.LastName = user.LastName;
                model.MiddleName = user.MiddleName;
                model.Phone = user.PhoneNumber;
                model.City = user.City;
                model.Email = user.Email;
                model.CountryId = user.CountryId;
            }
            else
            {
               model.CountryId = DataManager.Countries.GetAll().First().Id;
            }
            
            ViewBag.CountryId =
               new SelectList(DataManager.Countries.GetAll()
                   .OrderBy(x => x.Id), "Id", "Name", model.CountryId);
            ViewBag.MethodOfDeliveryId =
                new SelectList(DataManager.MethodOfDeliveries.GetAll()
                    .OrderBy(x => x.Id), "Id", "Name", model.MethodOfDeliveryId);
            ViewBag.MethodOfPaymentId = new SelectList(DataManager.MethodOfPayments.GetAll()
                    .OrderBy(x => x.Id), "Id", "Name", model.MethodOfPaymentId);
            return View(model);
        }

        [HttpPost]
        public ActionResult AddressAndPayment(OrderModel model)
        {
            if (ModelState.IsValid)
            {
              //  if (model.Total.Value < 120)
              //  {
              //      ModelState.AddModelError("", "Минимальная сумма заказа 120грн");
              //      ViewBag.MethodOfDeliveryId =
              //new SelectList(DataManager.MethodOfDeliveries.GetAll()
              //    .OrderBy(x => x.Name), "Id", "Name", model.MethodOfDeliveryId);
              //      ViewBag.MethodOfPaymentId = new SelectList(DataManager.MethodOfPayments.GetAll()
              //              .OrderBy(x => x.Name), "Id", "Name", model.MethodOfPaymentId);
              //      return View(model);


              //  }
                int currencyId = GetCurrentCurrency();
                var currency = DataManager.Currencies.SearchFor(t => t.CurrencyId == currencyId).Single();
                var cart = ShoppingCart.GetCart(HttpContext, DataManager);
                if (cart.GetCartsCount() == 0) return RedirectToAction("Index", "ShoppingCart");
                var baseurl = FullyQualifiedApplicationPath(HttpContext);
                var orderId = cart.CreateOrder(model,currency,User.Identity.IsAuthenticated,User.Identity.GetUserId());


                var order = DataManager.Orders.SearchFor(t => t.Id == orderId).Single();

                EmailSettings settings = new EmailSettings();
                SmtpClient smtp = new SmtpClient();
                smtp.Host = settings.ServerName;
                smtp.Port = settings.ServerPort;
                smtp.EnableSsl = settings.UseSsl;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(settings.MailFromAddress, settings.password);


                ViewBag.baseurl = baseurl;
                using (var msg = new MailMessage(settings.MailFromAddress, settings.MailFromAddress))
                {
                    string message = RenderViewToString(ControllerContext, "MailView", order);
                    msg.Subject = "Новый заказ";//message;
                    msg.IsBodyHtml = true;
                    msg.Body = message;
                    try
                    {
                        smtp.Send(msg);
                    }
                    catch (Exception e)
                    {
                        Response.Write(e.Message);
                        ViewBag.Message = e.Message;
                        return RedirectToAction("NotFound", "Error");
                    }
                }
                return RedirectToAction("Complete", "Checkout",
                    new { id = orderId });
            }
            ViewBag.MethodOfDeliveryId =
                new SelectList(DataManager.MethodOfDeliveries.GetAll()
                    .OrderBy(x => x.Name), "Id", "Name", model.MethodOfDeliveryId);
            ViewBag.MethodOfPaymentId = new SelectList(DataManager.MethodOfPayments.GetAll()
                    .OrderBy(x => x.Name), "Id", "Name", model.MethodOfPaymentId);
            ViewBag.CountryId =
             new SelectList(DataManager.Countries.GetAll()
                 .OrderBy(x => x.Id), "Id", "Name", model.CountryId);
            return View(model);
        }






















        public ActionResult Complete(int id)
        {
            if (DataManager.Orders.SearchFor(t => t.Id == id).Any())
            {
                return View(id);
            }
            return HttpNotFound();
        }
        public CheckoutController(DataManager dataManager) : base(dataManager)
        {
           
        }
    }
}