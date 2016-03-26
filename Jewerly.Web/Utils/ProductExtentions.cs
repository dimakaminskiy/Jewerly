using System.Collections.Generic;
using System.Linq;
using Jewerly.Domain;
using Jewerly.Web.Models;

namespace Jewerly.Web.Utils
{

    public static class ProductExtentions
    {
        public static ProductViewModel ToProductViewModel(this Product product, Currency currency, bool trade)
        {

            var price = product.Price*currency.Rate;

            if (product.Markup != null)
            {
                if (trade)
                {
                    price = price + ((price / 100) * product.Markup.Trade);
                }
                else
                {
                    price = price + ((price / 100) * product.Markup.Retail);
                }
            }



            if (product.Discount != null)
            {
                price = price - ((price/100)*product.Discount.Value);
            }

            var model = new ProductViewModel();
            model.Name = product.Name;
            model.SeoName = product.SeoName;
            model.Id = product.ProductId;
            model.Picture = product.Picture;
            model.Price = price;
            model.Currency = currency.Name;
            model.Discount = product.DiscountId == null ? 0 : product.Discount.Value;
            return model;
        }

        public static ProductDetailModel ToProductDetailModel(this Product product, Currency currency,bool trade)
        {
            var model = new ProductDetailModel();


            var price = product.Price * currency.Rate;

            if (product.Markup != null)
            {
                if (trade)
                {
                    price = price + ((price / 100) * product.Markup.Trade);
                }
                else
                {
                    price = price + ((price / 100) * product.Markup.Retail);
                }
            }
            var oldprice = price;

            if (product.Discount != null)
            {
                price = price - ((price / 100) * product.Discount.Value);
            }

            model.Id = product.ProductId;
            model.Name = product.Name;
           // model.SeoName = product.SeoName;
            model.Picture = product.Picture;
            model.Price = price;
            model.OldPrice = oldprice;
            model.Currency = currency.CurrencyCode;
            model.FullDescription = product.FullDescription;
            model.Discount = product.DiscountId == null ? 0 : product.Discount.Value;
            //model.SpecificationAttributes = new Dictionary<string, string>();

            var spec = new List<ProductPerformance>();
            foreach (var m in product.MappingProductSpecificationAttributeToProducts.OrderBy(t=>t.ProductSpecificationAttribute.DisplayOrder).ThenBy(t=>t.ProductSpecificationAttribute.Name))
            {
            //    model.SpecificationAttributes.Add(m.ProductSpecificationAttribute.Name,m.SpecificationAttributeOption.Name);
              spec.Add( new ProductPerformance
              {
                  Name = m.ProductSpecificationAttribute.Name,
                  Value = m.SpecificationAttributeOption.Name
              });
            }

            model.SpecificationAttributes = spec;

            var choiceAttr= new List<ProductChoiceAttribute>();
            foreach (var mapAttr in product.MappingProductChoiceAttributeToProducts.OrderBy(t=>t.ProductChoiceAttribute.DisplayOrder).ThenBy(t=>t.ProductChoiceAttribute.Name))
            {
               choiceAttr.Add(mapAttr.ProductChoiceAttribute);
            }

            model.ChoiceAttributes = choiceAttr;
            return model;
        }
    }

    public static class PictureExtentions
    {
        public static string Image(this Picture pic)
        {

            return "/Content/images/gallery/image/" + pic.Path;
        }
        public static string Preview(this Picture pic)
        {

            return "/Content/images/gallery/preview/" + pic.Path;
        }
    }
}