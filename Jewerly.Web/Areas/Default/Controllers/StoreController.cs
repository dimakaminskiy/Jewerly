using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Jewerly.Domain;
using Jewerly.Web.Controllers;
using Jewerly.Web.Models;
using Jewerly.Web.Utils;
using Ninject.Infrastructure.Language;

namespace Jewerly.Web.Areas.Default.Controllers
{
    public class StoreController : BaseController
    {
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
                foreach (var s in  DataManager.Categories.SearchFor(t => t.ParentCategoryId == c.Id && t.Published)
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
        private List<Category> GetListShowOnHomePageCategories()
        {
          var result = DataManager.Categories.SearchFor(t => t.Published && t.ShowOnHomePage).OrderBy(t => t.Name).ToList();
          return result;
        }


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
        private Dictionary<string, string> GetSortOptions()
        {
             var result = new Dictionary<string,string>();

             result.Add("cheap","от дешевых к дорогим");
             result.Add("expensive","от дорогих к дешевым");
             result.Add("novelty","новинки");
            return result;
        } 
        public ActionResult Index(int page=0,int id = 0, string name = "", string sort = "")
        {

            if (string.IsNullOrEmpty(name) && id==0)
            {

                var mainPageViewModal = new MainPageViewModal
                {
                  Menu   =  new CategoriesMenuViewModel()
                  {
                      CurentCategoryId = null,
                      MenuCategories =  GetListMenuCategories(),
                  },
                    HomePageCategories = GetListShowOnHomePageCategories()
                };
                return View("Home", mainPageViewModal);
            }
            var model = new StoreViewModel();
            var dic = GetSortOptions();
            var itemPerPage = 12; 

            if (!string.IsNullOrEmpty(sort) && !dic.ContainsKey(sort))
                sort = "";
            var orderOptions = new ProductsOrderByModel(dic, sort);

            if (page == 0)
            {
                page = 1;
            }

            model.Menu = new CategoriesMenuViewModel()
            {
                CurentCategoryId = id,
                MenuCategories = GetListMenuCategories()
            };

            var products = GetProducts(id, string.IsNullOrEmpty(sort) ? orderOptions.SortByDefult : sort);
            var productsViewModel = new PageableProducts(products, page, itemPerPage);
            model.Products = productsViewModel;
            
            model.ProductsOrderOption = orderOptions;
           return View(model);
        }

        public ActionResult Details(int id, string name)
        {
            var product = DataManager.Products.GetById(id);
            var model = new ProductDetailViewModel();
            model.Product = product.ToProductDetailModel();
            model.Menu = new CategoriesMenuViewModel()
            {
                CurentCategoryId = product.CategoryId,
                MenuCategories = GetListMenuCategories()
            };
            return View(model);
        }


        public StoreController(DataManager dataManager) : base(dataManager)
        {
        }

        
    }
}