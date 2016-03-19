using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Jewerly.Domain;
using Jewerly.Domain.Entities;
using Jewerly.Web.Controllers;
using Jewerly.Web.Models;
using Jewerly.Web.Utils;

namespace Jewerly.Web.Areas.Default.Controllers
{
    public class ShoppingCartController : BaseController
    {
        public ActionResult Index()
        {
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
            //return View(new ShoppingCartViewModel { CartItems = new List<Cart>() });
            return Json(new
            {});
        }

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
                    redirect = @Url.Action("Details", "Store", new { id = productId, name = product.SeoName})
                });
             }
            int currencyId = GetCurrentCurrency();
            var currency = DataManager.Currencies.SearchFor(t => t.CurrencyId == currencyId).Single();

           var cart = ShoppingCart.GetCart(this, DataManager);
            cart .AddProductToCart(productId,1);
           var cartItems=  cart.GetCarts();
            List<CartModel> carts = new List<CartModel>();
            foreach (var item in cartItems)
            {
                var price = product.Price*currency.Rate;

                if (product.Markup != null)
                {
                    if (User.Identity.IsAuthenticated)
                    {
                        price = price + ((price/100)*product.Markup.Trade);
                    }
                    else
                    {
                        price = price + ((price / 100) * product.Markup.Retail);
                    }
                }
                
                
                if (product.Discount != null)
                {
                    price = price - ((price/100)*product.Discount.Value);
                }
                carts.Add(new CartModel
                {
                    Id = item.Id,
                    Name = item.Product.Name,
                    SeoName = item.Product.SeoName,
                    Picture = item.Product.Picture.Preview(),
                    ProductId = item.ProductId,
                    Quantity = item.Count,
                    UnitPrice = price,
                    Currency = currency.CurrencyCode
                });
            }
            var model = new ShoppingCartMiniModel(carts);
            var helper = new HtmlHelper(new ViewContext(), new ViewPage());
            return Json(new
            {
                cartcountitems = model.Items.Count,
                carttotalprice = model.TotalPrice +" "+currency.CurrencyCode,
                cartitems =  helper.ShoppingCartMini(model),
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