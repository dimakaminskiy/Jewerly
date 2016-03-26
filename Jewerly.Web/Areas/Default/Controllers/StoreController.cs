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

        #region HomepageModel

        private MainPageViewModal GetMainPageViewModal()
        {
            return new MainPageViewModal()
            {
                HomePageCategories = GetListShowOnHomePageCategories(),
                MenuCategories = new MenuCategories(null, GetListMenuCategories())
            };
        }

        #endregion

        #region MenuCategoryModel

        private List<Category> GetListShowOnHomePageCategories()
        {
            var result =
                DataManager.Categories.SearchFor(t => t.Published && t.ShowOnHomePage).OrderBy(t => t.Name).ToList();
            return result;
        }



        private List<CategoryModel> GetListMenuCategories1()
        {
            List<CategoryModel> categories = new List<CategoryModel>();
            var topcategories =
                DataManager.Categories.SearchFor(t => t.Published && t.ParentCategoryId == null).ToList();

            foreach (var top in topcategories)
            {
                var child =
                    DataManager.Categories.SearchFor(
                        t => t.ParentCategoryId == top.Id && t.Products.Count(g => g.Published) > 0).ToList();

                if (child.Count == 0)
                {
                    if (top.Products.Count(g => g.Published) > 0)
                    {
                        var t = new CategoryModel
                        {
                            Id = top.Id,
                            Name = top.Name,
                            SeoName = top.SeoName,
                            SubCategories = new List<CategoryModel>()
                        };
                        categories.Add(t);
                    }
                }
                else
                {
                    var topcategory = new CategoryModel
                    {
                        Id = top.Id,
                        Name = top.Name,
                        SeoName = top.SeoName,

                    };


                    List<CategoryModel> subcategories = new List<CategoryModel>();
                    foreach (var c in child)
                    {
                        var s = new CategoryModel
                        {
                            Id = c.Id,
                            Name = c.Name,
                            SeoName = c.SeoName,
                            ParentCategoryId = c.ParentCategoryId
                        };
                        subcategories.Add(s);
                    }

                    topcategory.SubCategories = subcategories;
                    categories.Add(topcategory);
                }
            }
            return categories;
        }



















        private
            List<CategoryModel> GetListMenuCategories()
        {
            List<CategoryModel> categories = new List<CategoryModel>();

            foreach (var c in DataManager.Categories.SearchFor(t => t.ParentCategoryId == null && t.Published)
                .OrderBy(t => t.Name).Select(t => new CategoryModel()
                {
                    Id = t.Id,
                    Name = t.Name,
                    SeoName = t.SeoName,
                    ParentCategoryId = t.ParentCategoryId
                }).ToList())
            {
                categories.Add(c);
            }


            foreach (var c in categories)
            {
                List<CategoryModel> subcategories = new List<CategoryModel>();
                foreach (var s in DataManager.Categories.SearchFor(t => t.ParentCategoryId == c.Id && t.Published)
                    .OrderBy(t => t.Name).Select(t => new CategoryModel()
                    {
                        Id = t.Id,
                        Name = t.Name,
                        SeoName = t.SeoName,
                        ParentCategoryId = t.ParentCategoryId
                    }).ToList())
                {
                    subcategories.Add(s);
                }
                c.SubCategories = subcategories;
            }

            return categories;
        }

        #endregion

        #region Helpers

        private IQueryable<Product> GetProductByCategoryId(int catId)
        {
            var category = DataManager.Categories.GetById(catId);
            if (category.ParentCategoryId == null)
            {
                var childcategories =
                    DataManager.Categories.SearchFor(t => t.ParentCategoryId == category.Id && t.Published)
                        .Select(t => t.Id)
                        .ToEnumerable();
                var products =
                    DataManager.Products.SearchFor(
                        t => childcategories.Any(g => g == t.CategoryId) || t.CategoryId == catId);
                return products;
            }
            else
            {
                return DataManager.Products.SearchFor(t => t.CategoryId == catId && t.Published);
            }
        }

        private IQueryable<Product> GetProductBySearch(string text)
        {
            return DataManager.Products.SearchFor(t => t.ProductId.ToString().Contains(text));
        }

        private IQueryable<Product> GetProducts(IQueryable<Product> queryableSet, string sort)
        {
            //  IQueryable<Product> queryableSet = GetProductByCategoryId(categoryId);
            //DataManager.Products.SearchFor(t => t.CategoryId == categoryId && t.Published);
            switch (sort)
            {
                case "novelty":
                    queryableSet = queryableSet.OrderByDescending(t => t.ProductId);
                    break;
                case "expensive":
                    queryableSet = queryableSet.OrderByDescending(t => t.Price);
                    break;
                case "cheap":
                    queryableSet = queryableSet.OrderBy(t => t.Price);
                    break;
                    Default :
                    queryableSet = queryableSet.OrderBy(t => t.ProductId);
                    break;
            }

            return queryableSet;
        }

        private void InitializeRoles()
        {
            var roleManager =
                new RoleManager<IdentityRole>(
                    new RoleStore<IdentityRole>(new ApplicationDbContext()));


            if (!roleManager.RoleExists("Registered"))
            {
                var role = new IdentityRole();
                role.Name = "Registered";
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("Member"))
            {
                var role = new IdentityRole();
                role.Name = "Member";
                roleManager.Create(role);

            }
            if (!roleManager.RoleExists("Administrator"))
            {
                var role = new IdentityRole();
                role.Name = "Administrator";
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("Banned"))
            {
                var role = new IdentityRole();
                role.Name = "Banned";
                roleManager.Create(role);

            }


        }

        public CurrencyModel GetCurrencyModel()
        {
            var list = DataManager.Currencies.GetAll().OrderBy(t => t.DisplayOrder).ToList();
            int id = GetCurrentCurrency();
            return new CurrencyModel(list, id);
        }

        private static NameValueCollection ParseQueryString(string query)
        {
            NameValueCollection queryParameters = new NameValueCollection();
            if (query.Contains("?"))
            {
                query = query.Substring(query.IndexOf('?') + 1);
            }

            string[] querySegments = query.Split('&');
            foreach (string segment in querySegments)
            {
                string[] parts = segment.Split('=');
                if (parts.Length > 0)
                {
                    string key = parts[0].Trim(new char[] {'?', ' '});
                    string val = parts[1].Trim();

                    queryParameters.Add(key, val);
                }
            }



            return queryParameters;
        }

        private IQueryable<Product> FilterProducts(IQueryable<Product> queryableSet, int attrId, int optionId)
        {
            return (from p in queryableSet
                from m in p.MappingProductSpecificationAttributeToProducts
                where
                    p.ProductId == m.ProductId && m.ProductSpecificationAttributeId == attrId &&
                    m.SpecificationAttributeOptionId == optionId
                select p);

        }

        private List<QueryFilter> GetQueryFilters(string query)
        {
            if (!query.Contains("?"))
            {
                return null;
            }
            else
            {
                query = query.Substring(query.IndexOf('?') + 1);
            }
            string[] querySegments = query.Split('&');

            List<QueryFilter> filters = new List<QueryFilter>();

            foreach (string segment in querySegments)
            {
                string[] parts = segment.Split('=');
                if (parts.Length == 2)
                {
                    string attrString = parts[0].Trim();
                    string optionString = parts[1].Trim();

                    if (attrString.StartsWith("page"))
                    {
                        continue;
                    }

                    var Attr =
                        DataManager.ProductSpecificationAttributes.SearchFor(t => t.SeoName == attrString)
                            .Include(x => x.SpecificationAttributeOptions).SingleOrDefault();
                    if (Attr != null)
                    {
                        if (
                            Attr.SpecificationAttributeOptions.Any(
                                t => t.SpecificationAttributeOptionId.ToString() == optionString))
                        {
                            filters.Add(new QueryFilter()
                            {
                                AttributeName = attrString,
                                AttributeId = Attr.ProductSpecificationAttributeId,
                                AttributeOptionId = int.Parse(optionString)
                            });
                        }
                    }

                }
            }
            return filters;
        }

        private List<ProductFilter> GetProductFilters(IQueryable<Product> queryableSet)
        {
            var filters = new List<ProductFilter>();

            var attributtes = (from Product product in queryableSet
                               from mapp in product.MappingProductSpecificationAttributeToProducts.ToList()
                               where mapp.ProductSpecificationAttribute.AllowFiltering
                               orderby mapp.ProductSpecificationAttribute.DisplayOrder
                               select mapp.ProductSpecificationAttribute).Distinct().ToList();

            foreach (var attribute in attributtes)
            {
                var options =
                    DataManager.SpecificationAttributeOptions.SearchFor(
                        t => t.ProductSpecificationAttributeId == attribute.ProductSpecificationAttributeId)
                        .Select(g => new ProductFilterOption()
                        {
                            Id = g.SpecificationAttributeOptionId,
                            Name = g.Name,
                        }).ToList();


                var filter = new ProductFilter()
                {
                    Id = attribute.ProductSpecificationAttributeId,
                    Name = attribute.Name,
                    SeoName = attribute.SeoName,
                    Options = options,
                };

                filters.Add(filter);
            }


            return filters;

        }

            #endregion
        
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
                    return View("Home", GetMainPageViewModal());
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
           model.MenuCategories = new MenuCategories(id, GetListMenuCategories1());
           model.Currencies=GetCurrencyModel();



            IQueryable<Product> queryableSet = GetProductByCategoryId(id);
            List<QueryFilter> queryFilters;
            if (!string.IsNullOrEmpty(Request.Url.Query))
            {
               //ViewData["query"] = Request.Url.Query.Trim();
               queryFilters = GetQueryFilters(Request.Url.Query);
            }
            else
            {
                queryFilters =  new List<QueryFilter>();
            }

            var filters = GetProductFilters(queryableSet);
            if (queryFilters != null && queryFilters.Any())
            {
                for (int i = 0; i < queryFilters.Count; i++)
                {
                    var f = filters.SingleOrDefault(t => t.Id == queryFilters[i].AttributeId);
                    if (f != null)
                    {
                        f.CurrentOptionId = queryFilters[i].AttributeOptionId;
                       queryableSet= FilterProducts(queryableSet, f.Id, f.CurrentOptionId);
                    }
                }
            }
            model.Filters = filters;
            ViewBag.queryFilters = queryFilters;
          











            var products = GetProducts(queryableSet, string.IsNullOrEmpty(sort) ? sortOptions.SortByDefult : sort);

            var productsViewModel = new PageableProducts(products,
            model.Currencies.CurrentCurrency, (page == null || page == 0) ? 1 : page.Value,User.Identity.IsAuthenticated ,itemPerPage);



        
           model.Products = productsViewModel;
           model.ProductSortModel = sortOptions;

         












         


                     


         






           return View(model);
        }

        public ActionResult Details(int id, string name)
        {
           var product = DataManager.Products.GetById(id);
           var model = new ProductDetailViewModel();
           var c= DataManager.Currencies.GetById(GetCurrentCurrency());
           model.Product = product.ToProductDetailModel(c,User.Identity.IsAuthenticated);
           model.MenuCategories = new MenuCategories(product.CategoryId,GetListMenuCategories1());
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
            model.MenuCategories = new MenuCategories(null, GetListMenuCategories1());
            model.Currencies = GetCurrencyModel();

            IQueryable<Product> queryableSet = GetProductBySearch(text);
            model.Filters =  new List<ProductFilter>();
            var products = GetProducts(queryableSet, string.IsNullOrEmpty(sort) ? sortOptions.SortByDefult : sort);
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
        }

        #endregion


        
    }
}