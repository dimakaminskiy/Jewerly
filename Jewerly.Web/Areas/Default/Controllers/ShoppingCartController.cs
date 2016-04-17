using System;
using System.Collections.Generic;
using System.Linq;
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
            int currencyId = GetCurrentCurrency();
            var currency = DataManager.Currencies.SearchFor(t => t.CurrencyId == currencyId).Single();
            var cart = ShoppingCart.GetCart(this, DataManager);
            var cartItems = cart.GetCarts();
            var model = GetShoppingCartMiniModel(cartItems, currency);

            return View(model);
        }
        [HttpPost]
        public ActionResult ChangeCart(ShoppingCartMiniModel shoppingCartMiniModel)
        {
            int currencyId = GetCurrentCurrency();
            var currency = DataManager.Currencies.SearchFor(t => t.CurrencyId == currencyId).Single();
            var cart = ShoppingCart.GetCart(this, DataManager);
            
            foreach (var item in shoppingCartMiniModel.Items)
            {
                cart.SetProductCountByCartId(item.Id,item.Quantity);
            }
            var cartItems = cart.GetCarts();
            var model = GetShoppingCartMiniModel(cartItems, currency);

            var data = RenderViewToString(ControllerContext, "_ShoppingCart",model);
            var mini = RenderViewToString(ControllerContext, "_ShoppingCartItemsPartialView", model);

            return Json(new
            { success = true,
              data=data,
              mini=mini,
              cartcountitems = model.Count,
              carttotalprice = model.TotalPrice + " " + currency.CurrencyCode,
            });
            
        }
        [NonAction]
        public ShoppingCartMiniModel GetShoppingCartMiniModel(IEnumerable<Cart> cartItems, Currency currency)
        {
            List<CartModel> carts = new List<CartModel>();
            foreach (var item in cartItems)
            {
                var price = item.Product.Price * currency.Rate;

                if (item.Product.Markup != null)
                {
                    if (User.Identity.IsAuthenticated)
                    {
                        price = price + ((price / 100) * item.Product.Markup.Trade);
                    }
                    else
                    {
                        price = price + ((price / 100) * item.Product.Markup.Retail);
                    }
                }
                if (item.Product.Discount != null)
                {
                    price = price - ((price / 100) * item.Product.Discount.Value);
                }
                var temp =new CartModel
                {
                    Id = item.Id,
                    Name = item.Product.Name,
                    SeoName = item.Product.SeoName,
                    Picture = item.Product.Picture.Preview(),
                    ProductId = item.ProductId,
                    Quantity = item.Count,
                    UnitPrice = price,
                    ChoiceAttributesInString = item.ChoiceAttributesInString,
                    Currency = currency.CurrencyCode
                };
                temp.TotalPrice = temp.UnitPrice*temp.Quantity;
                carts.Add(temp);
            }
            var model = new ShoppingCartMiniModel(carts);
            model.Currency = currency.CurrencyCode;
            return model;
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
                        message = "Возникла ошибка. Пожалуйста, обновите страницу и попробуйте еще раз."
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
           cart .AddProductToCart(productId,1,"");
           var cartItems=  cart.GetCarts();

            var model = GetShoppingCartMiniModel(cartItems, currency);


            var html = RenderViewToString(ControllerContext, "_ShoppingCartItemsPartialView", model);

          //  var helper = new HtmlHelper(new ViewContext(), new ViewPage());
            return Json(new
            {
                cartcountitems = model.Count,
                carttotalprice = model.TotalPrice +" "+currency.CurrencyCode,
                cartitems = html,
                // helper.ShoppingCartMini(model),
                message = "Товар добавлен в корзину",
                success = true 
            });

        }
        [HttpPost]
        public ActionResult AddProductToCart_Details(int productId, int count, int? attrId, int? attrOptionId)
        {
            ProductChoiceAttribute attribute = null;
            ChoiceAttributeOption option = null;
            var product = DataManager.Products.GetById(productId);
            if (product == null)
            {
                if (product == null)
                    //no product found
                    return Json(new
                    {
                        success = false,
                        message = "Возникла ошибка. Пожалуйста, обновите страницу и попробуйте еще раз."
                    });
            }
             if (product.Published == false)
            {
                 // не опубликован
                return Json(new
                {
                    success = false,
                    message = "Возникла ошибка. Пожалуйста, обновите страницу и попробуйте еще раз."
                });
            }
            if (product.MappingProductChoiceAttributeToProducts.Any())
            {
                //нет атрибутов
                if (attrId.HasValue == false || attrOptionId.HasValue == false)
                {
                    return Json(new
                {
                    success = false,
                    message = "Возникла ошибка. Пожалуйста, обновите страницу и попробуйте еще раз."
                });
                }
                var map = product.MappingProductChoiceAttributeToProducts.SingleOrDefault(t => t.ProductChoiceAttributeId == attrId.Value);
                if (map != null)
                {
                 
                     attribute = map.ProductChoiceAttribute;
                     option =
                        attribute.ChoiceAttributeOptions.SingleOrDefault(
                            t => t.ChoiceAttributeOptionId == attrOptionId.Value);

                    if (option == null)
                    {
                        return Json(new
                        {
                            success = false,
                            message = "Возникла ошибка. Пожалуйста, обновите страницу и попробуйте еще раз."
                        });
                    }
                }
                else
                {
                    return Json(new
                    {
                        success = false,
                        message = "Возникла ошибка. Пожалуйста, обновите страницу и попробуйте еще раз."
                    });
                }
             }
            if (count <= 0) count = 1;


            int currencyId = GetCurrentCurrency();
            var currency = DataManager.Currencies.SearchFor(t => t.CurrencyId == currencyId).Single();

            var cart = ShoppingCart.GetCart(this, DataManager);
            var attrAtring = (attribute == null) ? "" : attribute.Name + " " + option.Name;
            cart.AddProductToCart(productId, count, attrAtring);
            var cartItems = cart.GetCarts();

            var model = GetShoppingCartMiniModel(cartItems, currency);
            var html = RenderViewToString(ControllerContext, "_ShoppingCartItemsPartialView", model);
            //  var helper = new HtmlHelper(new ViewContext(), new ViewPage());
            return Json(new 
            {
                cartcountitems = model.Count,
                carttotalprice = model.TotalPrice + " " + currency.CurrencyCode,
                cartitems = html,
                message = "Товар добавлен в корзину",
                success = true
            });
          }
        [HttpPost]
        public ActionResult CleanCart()
        {
            var cart = ShoppingCart.GetCart(HttpContext, DataManager);
            cart.EmptyCart();


            int currencyId = GetCurrentCurrency();
            var currency = DataManager.Currencies.SearchFor(t => t.CurrencyId == currencyId).Single();

            var cartItems = cart.GetCarts();
            var model = GetShoppingCartMiniModel(cartItems, currency);

            var data = RenderViewToString(ControllerContext, "_ShoppingCart", model);
            var mini = RenderViewToString(ControllerContext, "_ShoppingCartItemsPartialView", model);

            return Json(new
            {
                success = true,
                data = data,
                mini = mini,
                cartcountitems = model.Count,
                carttotalprice = model.TotalPrice + " " + currency.CurrencyCode,
            });
  

          
        }
        public ShoppingCartController(DataManager dataManager) : base(dataManager)
        {
        }

        public ActionResult MiniShoppingCart()
        {
            var cart = ShoppingCart.GetCart(this, DataManager);
            var cartItems = cart.GetCarts();
            int currencyId = GetCurrentCurrency();
            var currency = DataManager.Currencies.SearchFor(t => t.CurrencyId == currencyId).Single();
            var model = GetShoppingCartMiniModel(cartItems, currency);
            //List<CartModel> carts = new List<CartModel>();
            //foreach (var item in cartItems)
            //{
            //    var price = item.Product.Price * currency.Rate;

            //    if (item.Product.Markup != null)
            //    {
            //        if (User.Identity.IsAuthenticated)
            //        {
            //            price = price + ((price / 100) * item.Product.Markup.Trade);
            //        }
            //        else
            //        {
            //            price = price + ((price / 100) * item.Product.Markup.Retail);
            //        }
            //    }


            //    if (item.Product.Discount != null)
            //    {
            //        price = price - ((price / 100) * item.Product.Discount.Value);
            //    }
            //    carts.Add(new CartModel
            //    {
            //        Id = item.Id,
            //        Name = item.Product.Name,
            //        SeoName = item.Product.SeoName,
            //        Picture = item.Product.Picture.Preview(),
            //        ProductId = item.ProductId,
            //        Quantity = item.Count,
            //        UnitPrice = price,
            //        ChoiceAttributesInString = item.ChoiceAttributesInString,
            //        Currency = currency.CurrencyCode
            //    });
            //}
            //var model = new ShoppingCartMiniModel(carts);
            //model.Currency = currency.CurrencyCode;
            return PartialView("_ShoppingCartPartialView",model);
            
        }

    }
}