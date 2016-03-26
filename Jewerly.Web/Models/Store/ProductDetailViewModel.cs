using System.Collections.Generic;
using Jewerly.Domain;
using Microsoft.Owin.Security;

namespace Jewerly.Web.Models
{
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
            get { return Id + " " + Name; }
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

        public List<ProductPerformance> SpecificationAttributes { get; set; }
        public List<ProductChoiceAttribute> ChoiceAttributes { get; set; }

    }

    public class ProductPerformance
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

}