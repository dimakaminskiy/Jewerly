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
        //public  void Insert





        //public (int productId, int count, bool trade = false)
        //{
        //    Cart cart = GetCartItem(productId);
        //    if (cart == null) // такого товара в корзине нет ... печалька
        //    {
        //        return AddNewCart(productId, count, trade);
        //    }
        //    return SetProductCount(cart, cart.Count + count);
        //}


        private Cart GetCartById(int id)
        {
            return DataManager.Carts.SearchFor(t => t.Id == id).FirstOrDefault();
        }

       




    }


    public class ShoppingCartModel
    {
        public IList<ShoppingCartItemModel> Items { get; set; }

        public string GetTotal { get; set; }
    }




    public partial class ShoppingCartItemModel 
    {
        public ShoppingCartItemModel()
        {
            Picture = new PictureModel();          
        }
       
        public PictureModel Picture { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductSeName { get; set; }
        public string AttributeInfo { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public string SubTotal { get; set; }
      


    }

    public partial class PictureModel 
    {
        public string ImageUrl { get; set; }

        public string FullSizeImageUrl { get; set; }

        public string Title { get; set; }

        public string AlternateText { get; set; }
    }
}