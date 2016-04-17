using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Linq;
using Jewerly.Domain;
using Jewerly.Web.Models;
using Jewerly.Web.Models.Store;
using Ninject.Infrastructure.Language;

namespace Jewerly.Web.Utils
{
    public class StoreHelper
    {
        public DataManager DataManager { get; set; }

        public StoreHelper(DataManager dataManager)
        {
            DataManager = dataManager;
        }

        public MainPageViewModal GetMainPageViewModal()
        {
            return new MainPageViewModal()
            {
                HomePageCategories = GetListShowOnHomePageCategories(),
                MenuCategories = new MenuCategories(null, GetListMenuCategories())
            };
        }
        public List<Category> GetListShowOnHomePageCategories()
        {
            var result =
                DataManager.Categories.SearchFor(t => t.Published && t.ShowOnHomePage).OrderBy(t => t.Name).ToList();
            return result;
        }
        
        public List<CategoryModel> GetListMenuCategories()
        {
            List<CategoryModel> categories = new List<CategoryModel>();
            var topcategories =
                DataManager.Categories.SearchFor(t => t.Published && t.ParentCategoryId == null)
                    .OrderBy(t => t.Name)
                    .ToList();

            foreach (var top in topcategories)
            {
                var child =
                    DataManager.Categories.SearchFor(
                        t => t.ParentCategoryId == top.Id && t.Products.Count(g => g.Published) > 0).OrderBy(t => t.Name)
                    .ToList();


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

        public IQueryable<Product> GetProductByCategoryId(int catId)
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

        public IQueryable<Product> GetProductBySearch(string text)
        {
            return DataManager.Products.SearchFor(t => t.ProductId.ToString().Contains(text));
        }

        public IQueryable<Product> GetProducts(IQueryable<Product> queryableSet, string sort)
        {
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
                Default:
                    queryableSet = queryableSet.OrderBy(t => t.ProductId);
                    break;
            }

            return queryableSet;
        }

        public static NameValueCollection ParseQueryString(string query)
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
                    string key = parts[0].Trim(new char[] { '?', ' ' });
                    string val = parts[1].Trim();

                    queryParameters.Add(key, val);
                }
            }



            return queryParameters;
        }

        public IQueryable<Product> FilterProducts(IQueryable<Product> queryableSet, int attrId, int optionId)
        {
            return (from p in queryableSet
                    from m in p.MappingProductSpecificationAttributeToProducts
                    where
                        p.ProductId == m.ProductId && m.ProductSpecificationAttributeId == attrId &&
                        m.SpecificationAttributeOptionId == optionId
                    select p);

        }

        public List<QueryFilter> GetQueryFilters(string query)
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

        public List<ProductFilter> GetProductFilters(IQueryable<Product> queryableSet)
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



    }
}