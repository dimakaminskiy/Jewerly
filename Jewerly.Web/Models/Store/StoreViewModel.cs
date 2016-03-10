using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
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

        public string CurentCategorySeoName
        {
            get
            {
                if (MenuCategories.CurrentCategory == null) return "";
                return MenuCategories.CurrentCategory.SeoName;
            }
        }

        public string CurentCategoryId
        {
            get
            {
                if (MenuCategories.CurrentCategory == null) return "";
                return MenuCategories.CurrentCategory.Id.ToString();
            }
        }

        public string CurentSort
        {
            get { return ProductSortModel.Sort; }
        }

    }

    public class CurrencyModel

    {
        public CurrencyModel(List<Currency> currencies, int currentCurrencyId)
        {
            Currencies = currencies;
            CurrentCurrencyId = currentCurrencyId;
            _currentCurrency = Currencies.SingleOrDefault(t => t.CurrencyId == CurrentCurrencyId);
        }

        public List<Currency> Currencies { get; set; }
        public int CurrentCurrencyId { get; set; }
        private readonly Currency _currentCurrency;
        public Currency CurrentCurrency
        {
            get { return _currentCurrency; } 
        }

        public SelectList GetSelectList()
        {
            return new SelectList(Currencies, "Name", "CurrencyId", _currentCurrency.CurrencyId);
        }

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

    public class ProductDetailViewModel
    {
        public MenuCategories MenuCategories { get; set; }
        public ProductDetailModel Product    { get; set; }
    }



    public class ProductDetailModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string FullName
        {
            get { return Id +" "+ Name; }
        }

        public Picture Picture { get; set; }
        public decimal Price { get; set; }
        public decimal OldPrice { get; set; }
        public string Currency { get; set; }
        public string FullDescription { get; set; }
        public int Discount { get; set; }
        
        public string PriceString
        {
            get { return Price.ToString("F2"); }
        }
        public string OldPriceString
        {
            get { return OldPrice.ToString("F2"); }
        }

        public Dictionary<string, string> SpecificationAttributes { get; set; }
        public List<ProductChoiceAttribute> ChoiceAttributes { get; set; } 

    }



}