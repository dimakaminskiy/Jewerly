using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Jewerly.Domain;
using Jewerly.Web.Utils;

namespace Jewerly.Web.Models
{
    public class StoreViewModel
    {
        public PageableProducts Products { get; set; }
        public ProductsOrderByModel ProductsOrderOption { get; set; }

        private CategoriesMenuViewModel _menu;
        public CategoriesMenuViewModel Menu
        {
            get { return _menu; }
            set
            {
                _menu = value;
                bool flag = false;

                if (_menu != null && _menu.MenuCategories != null && _menu.MenuCategories.Count > 0 && _menu.CurentCategoryId!=null)
                {
                    foreach (var c in _menu.MenuCategories)
                    {
                        if (c.Id == _menu.CurentCategoryId.Value)
                        {
                            CurentCategory = c;
                            break;
                        }

                        foreach (var s in c .SubCategories)
                        {
                            if (s.Id == _menu.CurentCategoryId)
                            {
                                CurentCategory = s;
                                flag = true;
                            }
                        }
                        if (flag) break;
                    }
                }

            } }

        private CategoryModel CurentCategory { get; set; }
        //{
        //    get
        //    {
        //        if (Menu.CurentCategoryId == null) return null;
        //        var cat = Menu.MenuCategories.SingleOrDefault(t => t.Id == Menu.CurentCategoryId.Value);
        //        if (cat == null) return null;
        //        return cat;
        //    }
        //}

        public string CurentCategorySeoName
        {
            get
            {
                if (CurentCategory == null) return "";
                return CurentCategory.SeoName;
            }
        }

        public string CurentCategoryId
        {
            get
            {
                if (CurentCategory == null) return "";
                return CurentCategory.Id.ToString();
            }
        }

        public string CurentSort
        {
            get { return ProductsOrderOption.Sort; }
        }



    }

    public class CategoriesMenuViewModel
    {
        public int? CurentCategoryId { get; set; }
        public List<CategoryModel> MenuCategories { get; set; }

        public int? TopCategoryId
        {
            get
            {
                if (CurentCategoryId == null) return null;
                int?  pcat = null;
                foreach (var topcat in MenuCategories)
                {
                    if (topcat.Id == CurentCategoryId)
                    {
                        pcat = topcat.Id;
                        break;
                    }
                    foreach (var subCategory in topcat.SubCategories)
                    {
                        if (subCategory.Id == CurentCategoryId)
                        {
                            pcat = subCategory.ParentCategoryId;
                            break;
                        }
                    }
                  }
                return pcat;
            }
        }

        public string Name
        {
            get
            {
                if (CurentCategoryId == null) return null;
                var c = MenuCategories.SingleOrDefault(t => t.Id == CurentCategoryId);
                if (c == null) return null;
                return c.SeoName;
            }


        }


    }

    public class MainPageViewModal
    {
        public List<Category> HomePageCategories { get; set; }
        public CategoriesMenuViewModel Menu { get; set; }

    }

    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SeoName { get; set; }
        public Picture Picture { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
        public int  Discount { get; set; }

    }

    public class ProductDetailViewModel
    {
        public CategoriesMenuViewModel Menu { get; set; }
        public ProductDetailModel Product    { get; set; }
    }



    public class ProductDetailModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Picture Picture { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
        public string FullDescription { get; set; }
        public int Discount { get; set; }
        public Dictionary<string, string> SpecificationAttributes { get; set; }
        public List<ProductChoiceAttribute> ChoiceAttributes { get; set; } 

    }


    public class CategoryModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SeoName { get; set; }
        public int? ParentCategoryId { get; set; }
        public List<CategoryModel> SubCategories { get; set;} 
    }





    public class ProductsOrderByModel
    {
        public string Sort { get; set; }

        public string SortByDefult
        {
            get { return "novelty"; }
        }

        public SelectList SortOptions { get; set; }

        public ProductsOrderByModel(Dictionary<string, string> dic, string sort)
        {
           Sort = sort;
           SortOptions= new SelectList(dic, "Key", "Value",Sort?? SortByDefult);
        }
    }

}