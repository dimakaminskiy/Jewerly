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
        //    var cart = ShoppingCart.GetCart(this, DataManager);
        //    try
        //    {
        //        var viewModel = new ShoppingCartViewModel
        //        {
        //            CartItems = cart.GetCartItems(),
        //            CartTotal = cart.GetTotal()
        //        };
        //        return System.Web.UI.WebControls.View(viewModel);
        //    }
        //    catch (Exception)
        //    {
        //        cart.EmptyCart();


        //    }
        //    return System.Web.UI.WebControls.View(new ShoppingCartViewModel { CartItems = new List<Cart>() });
        //}

        public ShoppingCartController(DataManager dataManager) : base(dataManager)
        {
        }
    }
}