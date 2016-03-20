using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Jewerly.Domain;
using Jewerly.Domain.Entities;
using Jewerly.Web.Utils;

namespace Jewerly.Web.Models
{
    public class ShoppingCart
    {
        string ShoppingCartId { get; set; }
        public const string CartSessionKey = "CartId";
        private DataManager DataManager { get; set; }

        #region Ctor

        public static ShoppingCart GetCart(HttpContextBase context, DataManager dataManager)
        {
            var cart = new ShoppingCart(dataManager);
            cart.ShoppingCartId = cart.GetCartId(context);
            return cart;
        }

        public static ShoppingCart GetCart(Controller controller, DataManager dataManager)
        {
            return GetCart(controller.HttpContext, dataManager);
        }

        private ShoppingCart(DataManager dataManager)
        {
            DataManager = dataManager;
        }

        #endregion


        #region Private

        private string GetCartId(HttpContextBase context)
        {
            if (context.Session[CartSessionKey] == null)
            {
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session[CartSessionKey] =
                        context.User.Identity.Name;
                }
                else
                {
                    // Generate a new random GUID using System.Guid class
                    Guid tempCartId = Guid.NewGuid();
                    // Send tempCartId back to client as a cookie
                    context.Session[CartSessionKey] = tempCartId.ToString();
                }

            }
            return context.Session[CartSessionKey].ToString();
        }

        #endregion
        public Cart GetCartItemByProductId(int productId)
        {
            Cart cartItem = null;
            cartItem =
                DataManager.Carts.SearchFor(x => x.CartId == ShoppingCartId)
                    .SingleOrDefault(t => t.ProductId == productId);
            return cartItem;
        }
        public List<Cart> GetCarts()
        {
           return DataManager.Carts.SearchFor(t => t.CartId == ShoppingCartId).ToList();
        }

        public void AddProductToCart(int productId, int count)
        {
            var cart = GetCartItemByProductId(productId);
            if (cart == null) // новый товар
            {
                cart = new Cart()
                {
                    ProductId = productId,
                    Count = count,
                    CartId = ShoppingCartId,
                    DateCreated = DateTime.Now
                };
                DataManager.Carts.Insert(cart);
            }
            else
            {
                SetProductCount(cart, cart.Count + count);
            }

        }

        public Cart SetProductCount(Cart cart, int count)
        {
            cart.Count = count;
            DataManager.Carts.Edit(cart);
            return cart;
        }
       


    }


    public class CartModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string SeoName { get; set; }
        public string Picture { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public string Currency { get; set; }
    }

    public class ShoppingCartMiniModel
    {
        public ShoppingCartMiniModel(List<CartModel> cartItems)
        {
           Items = cartItems;
            Count = cartItems.Sum(t => t.Quantity);
           TotalPrice = cartItems.Sum(t => t.UnitPrice*t.Quantity).ToString("F2");
        }
        public IList<CartModel> Items { get; set; }
        public string TotalPrice { get; set; }
        public string Currency { get; set; }
        public int Count { get; set; }

    }












}