using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Jewerly.Domain;
using Jewerly.Web.Controllers;
using Jewerly.Web.extensions;
using Jewerly.Web.Models;
using Jewerly.Web.Models.Store;
using Jewerly.Web.Utils;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Ninject.Infrastructure.Language;

namespace Jewerly.Web.Areas.Default.Controllers
{
    public class StoreController : BaseController
    {
        [NonAction]
        private CurrencyModel GetCurrencyModel()
        {
            var list = DataManager.Currencies.GetAll().OrderBy(t => t.DisplayOrder).ToList();
            int id = GetCurrentCurrency();
            return new CurrencyModel(list, id);
        }
        
        public ActionResult Index(int? page = 0,int id = 0, string name = "", string sort = "")
        {
            if (Request.HttpMethod == "POST")
            {
                var currencyParam =  Request.Form["CurrencyId"];
                if (currencyParam != null && DataManager.Currencies.Count(t => t.Published && t.CurrencyId.ToString() == currencyParam) > 0)
                {
                    if (User.Identity.IsAuthenticated)
                    {
                       var manager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                        var userId = User.Identity.GetUserId();
                        var u = manager.FindById(userId);

                        if (DataManager.Currencies.SearchFor(t => t.CurrencyId.ToString() == currencyParam).Any())
                        {
                            u.CurrencyId = int.Parse(currencyParam);
                            manager.Update(u);
                        }

                    }
                   
                    SetCookie("Currency", currencyParam);
                    return RedirectToAction("Index", new { page = (page == 0) ? null : page, id, name, sort });
                }
            }

            
            if (id == 0)
            {
                if (string.IsNullOrEmpty(name))
                {
                    return View("Home", StoreHelper.GetMainPageViewModal());
                }
                var routeValues = Request.RequestContext.RouteData.Values;
                routeValues.Remove("name");
                routeValues.Remove("id");
                return RedirectToAction("Index");
            }
            if (DataManager.Categories.Count(t=>t.Published && t.Id==id)==0)
            {
                var routeValues = Request.RequestContext.RouteData.Values;
                routeValues.Remove("name");
                routeValues.Remove("id");
                return RedirectToAction("Index");
            }


           var model = new StoreViewModel();
           var itemPerPage = 12;
           var sortOptions = new ProductSortModel(sort);          
           model.MenuCategories = new MenuCategories(id, StoreHelper.GetListMenuCategories());
           model.Currencies=GetCurrencyModel();

           IQueryable<Product> queryableSet = StoreHelper.GetProductByCategoryId(id);
            List<QueryFilter> queryFilters;
            if (!string.IsNullOrEmpty(Request.Url.Query))
            {
               //ViewData["query"] = Request.Url.Query.Trim();
               queryFilters = StoreHelper.GetQueryFilters(Request.Url.Query);
            }
            else
            {
                queryFilters =  new List<QueryFilter>();
            }

            var filters = StoreHelper.GetProductFilters(queryableSet);
            if (queryFilters != null && queryFilters.Any())
            {
                for (int i = 0; i < queryFilters.Count; i++)
                {
                    var f = filters.SingleOrDefault(t => t.Id == queryFilters[i].AttributeId);
                    if (f != null)
                    {
                        f.CurrentOptionId = queryFilters[i].AttributeOptionId;
                        queryableSet = StoreHelper.FilterProducts(queryableSet, f.Id, f.CurrentOptionId);
                    }
                }
            }
            model.Filters = filters;
            ViewBag.queryFilters = queryFilters;
            var products = StoreHelper.GetProducts(queryableSet, string.IsNullOrEmpty(sort) ? sortOptions.SortByDefult : sort);
            var productsViewModel = new PageableProducts(products,
            model.Currencies.CurrentCurrency, (page == null || page == 0) ? 1 : page.Value,User.Identity.IsAuthenticated ,itemPerPage);

           model.Products = productsViewModel;
           model.ProductSortModel = sortOptions;
           return View(model);
        }

        public ActionResult Details(int id, string name)
        {
           var product = DataManager.Products.GetById(id);
            if (product == null) return HttpNotFound();
           var model = new ProductDetailViewModel();
           var c= DataManager.Currencies.GetById(GetCurrentCurrency());
           model.Product = product.ToProductDetailModel(c,User.Identity.IsAuthenticated);
           model.MenuCategories = new MenuCategories(product.CategoryId, StoreHelper.GetListMenuCategories());
           return View(model);
        }

        public ActionResult Search( int? page=0, string text="", string sort="")
        {

           if (Request.HttpMethod.ToString() == "POST")
            {

                var currencyParam = Request.Form["CurrencyId"];
                if (currencyParam != null && DataManager.Currencies.Count(t => t.Published && t.CurrencyId.ToString() == currencyParam) > 0)
                {
                    SetCookie("Currency", currencyParam);
                    return RedirectToAction("Search", new {text=text });
                }

                return RedirectToAction("Search", new
                {
                    text = text,
                    sort = sort,
                    page = page,
               });
            }
           if (string.IsNullOrEmpty(text)) return RedirectToAction("Index");
 
            if (!page.HasValue) page = 1;
            if (string.IsNullOrEmpty(sort)) sort = "novelty";


            var model = new StoreViewModel();
            var itemPerPage = 12;
            var sortOptions = new ProductSortModel(sort);
            model.MenuCategories = new MenuCategories(null, StoreHelper.GetListMenuCategories());
            model.Currencies = GetCurrencyModel();

            IQueryable<Product> queryableSet = StoreHelper.GetProductBySearch(text);
            model.Filters =  new List<ProductFilter>();
            var products = StoreHelper.GetProducts(queryableSet, string.IsNullOrEmpty(sort) ? sortOptions.SortByDefult : sort);
            var productsViewModel = new PageableProducts(products,
            model.Currencies.CurrentCurrency, (page == null || page == 0) ? 1 : page.Value, User.Identity.IsAuthenticated, itemPerPage);
            model.Products = productsViewModel;
            model.ProductSortModel = sortOptions;

            ViewBag.text = text;
            return View(model);
        }

        #region Ctor

        public StoreController(DataManager dataManager) : base(dataManager)
        {
            StoreHelper = new StoreHelper(dataManager);
        }

        private StoreHelper StoreHelper { get; set; }
        #endregion


        
    }
}