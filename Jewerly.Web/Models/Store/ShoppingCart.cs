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
        public void  SetProductCountByCartId(int cartId, int count)
        {
            Cart cartItem = null; 
            if (count == 0 )
            {
               cartItem =  DataManager.Carts.SearchFor(t => t.Id == cartId && t.CartId == ShoppingCartId).SingleOrDefault();
                if (cartItem != null)
                {
                    DataManager.Carts.Delete(cartItem);
                }
            }
            else
            {
                cartItem = DataManager.Carts.SearchFor(t => t.Id == cartId && t.CartId == ShoppingCartId).SingleOrDefault();
                if (cartItem != null)
                {
                    cartItem.Count = count;
                    DataManager.Carts.Edit(cartItem);
                }

            }


     
        }

        public int GetCartsCount()
        {
            return DataManager.Carts.Count(t => t.CartId == ShoppingCartId);
        }

        public int CreateOrder(OrderModel model, Currency currency,bool trade)
        {
            //var c = new OrderDetail();

            var order = new Order
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                MiddleName = model.MiddleName,
                Email = model.Email,
                Phone = model.Phone,
                CountryId = model.CountryId,
                City = model.City,
                CurrencyId = currency.CurrencyId,
                OrderDate = DateTime.Now,
                OrderStatusId = 1,
                MethodOfDeliveryId = model.MethodOfDeliveryId,
                MethodOfPaymentId = model.MethodOfPaymentId,
                TextInfo = model.TextInfo,
              };

            List<OrderDetail> list = new List<OrderDetail>();
            var cartItems = GetCarts();
            foreach (var item in cartItems)
            {
                var price = item.Product.Price * currency.Rate;

                if (item.Product.Markup != null)
                {
                    if (trade)
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

                var orderDetail = new OrderDetail
                {
                    ProductId = item.ProductId,
                    Quantity = item.Count,
                    UnitPrice = price
                 };
                list.Add(orderDetail);
            }
            order.Total = list.Sum(t => t.UnitPrice*t.Quantity);

            DataManager.Orders.Insert(order);

            foreach (var detail in list)
            {
                detail.OrderId = order.Id;
                DataManager.OrderDetails.Insert(detail);
            }
            return order.Id;
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
        public ShoppingCartMiniModel()
        {
            
        }
        public List<CartModel> Items { get; set; }
        public string TotalPrice { get; set; }
        public string Currency { get; set; }
        public int Count { get; set; }

    }












}