using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Jewerly.Domain;
using Jewerly.Domain.Entities;

namespace Jewerly.Web.Models
{
    public class ShoppingCart
    {
        private ShoppingCart(DataManager dataManager)
        {
            DataManager = dataManager;
        }

        string ShoppingCartId { get; set; }
        public const string CartSessionKey = "CartId";
        private DataManager DataManager { get; set; }
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
       
        private Cart GetCartItem(int productId)
        {
            Cart cartItem = null;
            cartItem =
                DataManager.Carts.SearchFor(x => x.CartId == ShoppingCartId)
                    .SingleOrDefault(t => t.ProductId == productId);
            return cartItem;
        }
        private Cart GetCartById(int id)
        {
            return DataManager.Carts.SearchFor(t => t.Id == id).FirstOrDefault();
        }

        public void AddProductTocart(int productId, int count, string attr)
        {
            

        }

        private List<Cart> GetCartsByProductId(int id)
        {
            var carts = GetCartItems();
            var products = carts.Where(t => t.ProductId == id).ToList();
            return products;
        }

//        private List<ProductChoiceAttribute>  
















        private Cart AddNewCart(int productId, int count, bool trade = false)
        {
            //if (!DataManager.Products.SearchFor(x => x.ProductId == productId).Any())
            //{
            //    throw new Exception("Товар не найден");
            //}
            //var product = DataManager.Products.SearchFor(x => x.ProductId == productId).Single();
            //int markup = 0;
            //double price = 0;
            //if (product.MarkupId != null && DataManager.Markups.SearchFor(t => t.Id == product.MarkupId).Any())
            //{
            //    if (trade)
            //    {
            //        markup = DataManager.Markups.SearchFor(t => t.Id == product.MarkupId).First().TradeMarkup;
            //    }
            //    else
            //    {
            //        markup = DataManager.Markups.SearchFor(t => t.Id == product.MarkupId).First().RetailMarkup;
            //    }
            //}
            //if (!trade)
            //{
            //    price = (product.ShoppingPrice + (product.ShoppingPrice * markup / 100));
            //}
            //else
            //{
            //    price = product.ShoppingPrice + (product.ShoppingPrice * markup / 100);
            //}

            //var cart = new Cart()
            //{
            //    ProductId = productId,
            //    Count = count,
            //    Price = price,
            //    CartId = ShoppingCartId,
            //    DateCreated = DateTime.Now
            //};
            //DataManager.Carts.Insert(cart);
            //return cart;
            return null;
        }
        //public Cart AddToCart(int productId, int count, string attr)
        //{
        //    Cart cart = GetCartItem(productId);
        //    if (cart == null) // такого товара в корзине нет ... печалька
        //    {
        //        return AddNewCart(productId, count, trade);
        //    }
        //    return SetProductCount(cart, cart.Count + count);
        //}
        public Cart SetProductCount(int id, int count)
        {
            var cart = GetCartById(id);
            return SetProductCount(cart, count);
        }
        public Cart SetProductCount(Cart cart, int count)
        {

            if (cart == null) throw new ArgumentException("Нет такого товара в корзине");
            if (count <= 0) throw new ArgumentException("Кол-во товара всегда больше 0");
            cart.Count = count;
            DataManager.Carts.Edit(cart);
            return cart;
        }
        public void RemoveCart(int id) // удаление с заказа
        {
            var cart = GetCartById(id);
            if (cart == null) throw new ArgumentException("Нет такого товара в корзине");
            DataManager.Carts.Delete(cart);
        }
        public IEnumerable<Cart> GetCartItems()
        {
            return DataManager.Carts.SearchFor(
                cart => cart.CartId == ShoppingCartId).ToList();
        }
        public int GetCountItems()
        {
            var sz = DataManager.Carts.SearchFor(
                cart => cart.CartId == ShoppingCartId).ToList().Select(t => t.Count).Sum().ToString();
            return int.Parse(sz.ToString());
        }
        public double GetTotal()
        {
            //var list = DataManager.Carts.SearchFor(i => i.CartId == ShoppingCartId).Include(t => t.Product).ToList();
            //return (from cart in list select cart.Count * cart.Price).Sum();
            return 0;
        }
        public void EmptyCart()
        {
            var cartItems = DataManager.Carts.SearchFor(
                cart => cart.CartId == ShoppingCartId).ToList();
            foreach (var cartItem in cartItems)
            {
                DataManager.Carts.Delete(cartItem);
            }
        }

        public int CreateOrder(Order order)
        {
            var c = new OrderDetail();

            order.OrderDate = DateTime.Now;
            order.OrderStatusId = 1; // новый заказ => статус не обработано
            order.Total = GetTotal(); // общую сумму пересчитаем!!!
            DataManager.Orders.Insert(order);  //  запишем заказ в базу данных
            //----------------------------------------------------------------//
            // в таблицу OrderDetail запишем подробности нашего заказа
            var cartItems = GetCartItems();
            foreach (var orderDetail in cartItems.Select(item => new OrderDetail
            {
                OrderId = order.Id,
                UnitPrice = item.Price,
                Quantity = item.Count,
                ProductId = item.ProductId,

            }))
            {
                DataManager.OrderDetails.Insert(orderDetail);
            }
            //-----------------------------------------------------------------//

            EmptyCart(); // после оформления заказа необходимо очистить корзину !
            return order.Id;
        }


    }
}