using System;
using System.Collections.Generic;
using Jewerly.Domain;
using Jewerly.Web.Utils;

namespace Jewerly.Web.Models
{
    public class StoreViewModel
    {
        public PageableProducts Products { get; set; }
        public ProductSortModel ProductSortModel { get; set; }
        public MenuCategories MenuCategories { get; set; }
        public CurrencyModel Currencies { get; set; }
        public string CurrentCategorySeoName
        {
            get
            {
                if (MenuCategories.CurrentCategory == null) return "";
                return MenuCategories.CurrentCategory.SeoName;
            }
        }
        public string CurrentCategoryId
        {
            get
            {
                if (MenuCategories.CurrentCategory == null) return "";
                return MenuCategories.CurrentCategory.Id.ToString();
            }
        }
        public string CurrentSort
        {
            get { return ProductSortModel.Sort; }
        }
        public List<ProductFilter> Filters { get; set; }
        public ShoppingCartMiniModel ShoppingCartMiniModel { get; set; }

     }


    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FullName
        {
            get { return Id + " " + Name; }
        }
        public string SeoName { get; set; }
        public Picture Picture { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
        public int  Discount { get; set; }

        public string PriceString
        {
            get { return Price.ToString("F2"); }
        }


    }


   










   



}