using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Jewerly.Domain;
using Jewerly.Domain.Entities;
using Jewerly.Web.Controllers;
using Jewerly.Web.Models;

namespace Jewerly.Web.Areas.Default.Controllers
{
    public class ShoppingCartController : BaseController
    {
        //public ActionResult Index()
        //{
        //    //var cart = ShoppingCart.GetCart(this, DataManager);
        //    //try
        //    //{
        //    //    var viewModel = new ShoppingCartViewModel
        //    //    {
        //    //        CartItems = cart.GetCartItems(),
        //    //        CartTotal = cart.GetTotal()
        //    //    };
        //    //    return System.Web.UI.WebControls.View(viewModel);
        //    //}
        //    //catch (Exception)
        //    //{
        //    //    cart.EmptyCart();


        //    //}
        //    //return View(new ShoppingCartViewModel { CartItems = new List<Cart>() });
        //}

        [HttpPost]
        public ActionResult AddProductToCart(int productId)
        {
            var product = DataManager.Products.GetById(productId);
            if (product == null)
            {
                if (product == null)
                    //no product found
                    return Json(new
                    {
                        success = false,
                        message = "Продукт не найден. Пожалуйста, обновите страницу и попробуйте еще раз."
                    });
             }
            

            if (product.MappingProductChoiceAttributeToProducts.Any())
            {
                return Json(new
                {
                    redirect = @Url.Action("Details", "Store", new { id = productId, name =product.SeoName})
                });
             }



            return Json(new
            {
                carttotalitems = 10,
                carttotalprice = "135 USD",
                cartitems = "<div>Мама мия</div>",
                message = "Товар добавлен в корзину",
                success = true 
            });

        }






        [HttpPost]
        public ActionResult AddToCard(int id, int count)
        {
            try
            {
                var cart = ShoppingCart.GetCart(this, DataManager);
                
                
                
                
                
                return Json(new { success = true, item = id, itemCount = 10});
            }
            catch (Exception e)
            {
                return Json(new { success = false, errorMessage = e.Message });
            }

        }


        public ShoppingCartController(DataManager dataManager) : base(dataManager)
        {
        }
    }
}