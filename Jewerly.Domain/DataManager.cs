﻿using System.Collections.Generic;
using System.Data.Entity;
using Jewerly.Domain.Entities;
using Jewerly.Domain.Repository;

namespace Jewerly.Domain
{
   public  class DataManager
    {
       public IGenericRepository<Category> Categories { get; private set; }
       public IGenericRepository<Currency> Currencies { get; private set; }
       public IGenericRepository<Picture> Pictures { get; private set; }
       public IGenericRepository<CategoryPicture> CategoryPictures { get; private set; }
       public IGenericRepository<Product> Products { get; private set; }
       public IGenericRepository<ProductSpecificationAttribute> ProductSpecificationAttributes { get;  private set; }
       public IGenericRepository<SpecificationAttributeOption> SpecificationAttributeOptions { get; private set; }
       public IGenericRepository<MappingProductSpecificationAttributeToProduct> MappingProductSpecificationAttributeToProducts { get; private set; }
       public IGenericRepository<MappingProductChoiceAttributeToProduct> MappingProductChoiceAttributeToProducts { get; private set; }
       public IGenericRepository<AvalibleChoiceAttributeOption> AvalibleChoiceAttributeOptions { get; private set; }
       public IGenericRepository<ProductChoiceAttribute> ProductChoiceAttributes { get; private set; }
       public IGenericRepository<ChoiceAttributeOption> ChoiceAttributeOptions { get; private set; }
       public IGenericRepository<Discount> Discounts { get; private set; }
       public IGenericRepository<Markup> Markups { get;  private set; }
       public IGenericRepository<Review> Reviews { get; set; }
       public IGenericRepository<Country> Countries { get; set; }
       public IGenericRepository<OrderStatus> OrderStatuses { get; set; }
       public IGenericRepository<Order> Orders { get; set; }
       public IGenericRepository<OrderDetail> OrderDetails { get; set; }
       public IGenericRepository<Cart> Carts { get; set; }
       public IGenericRepository<SliderPicture> SliderPictures { get; set; }
       public IGenericRepository<MethodOfPayment> MethodOfPayments { get; set; }
       public IGenericRepository<MethodOfDelivery> MethodOfDeliveries { get; set; }


       public DataManager
           (
            IGenericRepository<Category> categories
           ,IGenericRepository<Currency> currencies
           ,IGenericRepository<Picture> pictures
           ,IGenericRepository<CategoryPicture> categoryPictures 
           ,IGenericRepository<Product> products
           ,IGenericRepository<ProductSpecificationAttribute> productSpecificationAttributes
           ,IGenericRepository<SpecificationAttributeOption> specificationAttributeOptions
           ,IGenericRepository<MappingProductSpecificationAttributeToProduct> mappingProductSpecificationAttributeToProducts
           ,IGenericRepository<MappingProductChoiceAttributeToProduct> mappingProductChoiceAttributeToProducts
           ,IGenericRepository<AvalibleChoiceAttributeOption> avalibleChoiceAttributeOption
           ,IGenericRepository<ProductChoiceAttribute> productChoiceAttributes
           ,IGenericRepository<ChoiceAttributeOption> choiceAttributeOptions
           ,IGenericRepository<Discount> discounts
           ,IGenericRepository<Markup> markups
           ,IGenericRepository<Review> reviews
           ,IGenericRepository<Country> countries
           ,IGenericRepository<OrderStatus> orderStatuses
           ,IGenericRepository<Order> orders
           ,IGenericRepository<OrderDetail> orderDetails
           ,IGenericRepository<Cart> carts
           ,IGenericRepository<SliderPicture> sliderPictures
           ,IGenericRepository<MethodOfPayment> methodOfPayments
           ,IGenericRepository<MethodOfDelivery> methodOfDeliveries 
           
           )
       {
           Categories = categories;
           Currencies = currencies;
           Pictures = pictures;
           CategoryPictures = categoryPictures;
           Products = products;
           ProductSpecificationAttributes = productSpecificationAttributes;
           SpecificationAttributeOptions = specificationAttributeOptions;
           MappingProductSpecificationAttributeToProducts = mappingProductSpecificationAttributeToProducts;
           MappingProductChoiceAttributeToProducts = mappingProductChoiceAttributeToProducts;
           AvalibleChoiceAttributeOptions = avalibleChoiceAttributeOption;
           ProductChoiceAttributes = productChoiceAttributes;
           ChoiceAttributeOptions = choiceAttributeOptions;
           Discounts = discounts;
           Markups = markups;
           Reviews = reviews;
           Countries = countries;
           OrderStatuses = orderStatuses;
           Orders = orders;
           OrderDetails = orderDetails;
           Carts = carts;
           SliderPictures = sliderPictures;
           MethodOfPayments = methodOfPayments;
           MethodOfDeliveries = methodOfDeliveries;
       }
    }
}
