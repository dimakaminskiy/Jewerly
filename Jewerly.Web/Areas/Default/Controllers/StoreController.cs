using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Jewerly.Domain;
using Jewerly.Web.Controllers;
using Jewerly.Web.Models;
using Jewerly.Web.Utils;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
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

        private List<CategoryModel> GetListMenuCategories()
        {
            List<CategoryModel> result = new List<CategoryModel>();

            foreach (var c in DataManager.Categories.SearchFor(t => t.ParentCategoryId == null && t.Published)
                .OrderBy(t => t.Name).Select(t => new CategoryModel()
                {
                    Id = t.Id,
                    Name = t.Name,
                    SeoName = t.SeoName,
                    ParentCategoryId = t.ParentCategoryId
                }).ToList())
            {
                result.Add(c);
            }


            foreach (var c in result)
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

            return result;
        }

        #endregion





 
        
        private IQueryable<Product> GetProductByCategoryId(int catId)
        {
            var category = DataManager.Categories.GetById(catId);
            if (category.ParentCategoryId == null)
            {
                var childcategories = DataManager.Categories.SearchFor(t => t.ParentCategoryId == category.Id && t.Published).Select(t=>t.Id).ToEnumerable();
                var products = DataManager.Products.SearchFor(t => childcategories.Any(g => g == t.CategoryId));
                return products;
            }
            else
            {
               return DataManager.Products.SearchFor(t => t.CategoryId == catId && t.Published);
            }

  
        }
        public IQueryable<Product> GetProducts(int categoryId, string sort)
        {
            IQueryable<Product> queryableSet = GetProductByCategoryId(categoryId);
                //DataManager.Products.SearchFor(t => t.CategoryId == categoryId && t.Published);
            switch (sort)
            {
                case "novelty" :
                  queryableSet=  queryableSet.OrderBy(t => t.ProductId); break;
                case "expensive" :
                  queryableSet=  queryableSet.OrderByDescending(t => t.Price); break;
                case "cheap" :
                     queryableSet=  queryableSet.OrderBy(t => t.Price); break;
              Default :
                         queryableSet=  queryableSet.OrderBy(t => t.ProductId); break;
             }
            
            return queryableSet;
        }
      
        void InitializeRoles()
        {
            var roleManager = new RoleManager<Microsoft.AspNet.Identity.EntityFramework.IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));


            if (!roleManager.RoleExists("Registered"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Registered";
                roleManager.Create(role);
             }
            if (!roleManager.RoleExists("Member"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Member";
                roleManager.Create(role);

            }
            if (!roleManager.RoleExists("Administrator"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Administrator";
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("Banned"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Banned";
                roleManager.Create(role);

            }


        }

        public CurrencyModel GetCurrencyModel()
        {
            var list = DataManager.Currencies.GetAll().OrderBy(t => t.DisplayOrder).ToList();
            int id = GetCurrentCurrency();
            return  new CurrencyModel( list,id);
        }

        
        public ActionResult Index(int? page = 0,int id = 0, string name = "", string sort = "")
        {
            var gg = Request.Form;
            if (Request.HttpMethod == "POST")
            {
                var currencyParam =  Request.Form["CurrencyId"];
                if (currencyParam != null && DataManager.Currencies.Count(t => t.Published && t.CurrencyId.ToString() == currencyParam) > 0)
                {
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
           model.MenuCategories = new MenuCategories(id, GetListMenuCategories());
           model.Currencies=GetCurrencyModel();
           var products = GetProducts(id, string.IsNullOrEmpty(sort) ? sortOptions.SortByDefult : sort);
           var productsViewModel = new PageableProducts(products,
               model.Currencies.CurrentCurrency, (page== null||page==0)? 1: page.Value, itemPerPage);
           model.Products = productsViewModel;
           model.ProductSortModel = sortOptions;
           return View(model);
        }

        public ActionResult Details(int id, string name)
        {
           var product = DataManager.Products.GetById(id);
           var model = new ProductDetailViewModel();
           var c= DataManager.Currencies.GetById(GetCurrentCurrency());
           model.Product = product.ToProductDetailModel(c);
           model.MenuCategories = new MenuCategories(product.CategoryId,GetListMenuCategories());
           return View(model);
        }


        public StoreController(DataManager dataManager) : base(dataManager)
        {
        }

        
    }
}