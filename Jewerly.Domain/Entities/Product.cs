using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Jewerly.Domain
{
    public class Product
    {
        public int ProductId { get; set; }
        [Display(Name = "Название")]
        public string Name { get; set; }
        [Display(Name = "Название SEO")]
        public string SeoName { get; set; }
        [Display(Name = "Короткое описание")]
        public string ShortDescription { get; set; }
        [Display(Name = "Полное описание")]
        public string FullDescription { get; set; }
        [Required]
        [Display(Name = "Цена")]
        public decimal Price { get; set; }
        [Display(Name = "Опубликована?")]
        public bool Published { get; set; }
        [Display(Name = "Изображение")]
        public int PictureId { get; set; }
        [Required]
        [Display(Name = "Наценка")]
        public int MarkupId { get; set; }
        [Display(Name = "Скидка")]
        public int? DiscountId { get; set; }


        [Required]
        [Display(Name = "Категория")]
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }
        public virtual Markup Markup { get; set; }
        public virtual Discount Discount { get; set; }

        public virtual Picture Picture { get; set; }

        public ICollection<MappingProductSpecificationAttributeToProduct> MappingProductSpecificationAttributeToProducts { get; set; }
        public ICollection<MappingProductChoiceAttributeToProduct> MappingProductChoiceAttributeToProducts { get; set; }

    }


    public class MappingProductSpecificationAttributeToProduct
    {
        public int MappingProductSpecificationAttributeToProductId { get; set; }
        public int ProductId { get; set; }
        public int ProductSpecificationAttributeId { get; set; }
        public int SpecificationAttributeOptionId { get; set; }

        public virtual Product Product  { get; set; }
        public virtual ProductSpecificationAttribute ProductSpecificationAttribute { get; set; }
        public virtual SpecificationAttributeOption SpecificationAttributeOption { get; set; }
    }

    public class ProductSpecificationAttribute
    {
        public int ProductSpecificationAttributeId { get; set; }
        [Required]
        [Display(Name = "Название")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Название SEO")]
        public string SeoName { get; set; }
        [Display(Name = "Разрешить фильтр")]
        public bool AllowFiltering { get; set; }
        [Display(Name = "Порядок фильтров")]
        public int DisplayOrder { get; set; }
        public ICollection<SpecificationAttributeOption> SpecificationAttributeOptions { get; set; }

    }

    public class SpecificationAttributeOption
    {
        public int SpecificationAttributeOptionId { get; set; }
        [Display(Name = "Название Атрибута")]
        public int ProductSpecificationAttributeId { get; set; }
        [Required]
        [Display(Name = "Значение")]
        public string Name { get; set; }
       
        [Display(Name = "Порядок фильтров")]
        public int DisplayOrder { get; set; }
        public virtual ProductSpecificationAttribute ProductSpecificationAttribute { get; set; }
    }






    public class MappingProductChoiceAttributeToProduct
    {
        public int MappingProductChoiceAttributeToProductId { get; set; }
        public int ProductId { get; set; }
        public int ProductChoiceAttributeId { get; set; }
        public virtual Product Product { get; set; }
        public virtual ProductChoiceAttribute ProductChoiceAttribute { get; set; }
        public ICollection<AvalibleChoiceAttributeOption> AllowSpecificationAttributeOptionToProduct { get; set; }
    }


    public class AvalibleChoiceAttributeOption
    {
        public int AvalibleChoiceAttributeOptionId { get; set; }
        public int MappingProductChoiceAttributeToProductId { get; set; }
        public int ChoiceAttributeOptionId { get; set; }

        public virtual MappingProductChoiceAttributeToProduct MappingProductChoiceAttributeToProduct { get; set; }
        public virtual ChoiceAttributeOption ChoiceAttributeOption { get; set; }
      
    }






    public class ProductChoiceAttribute
    {
        public int ProductChoiceAttributeId { get; set; }
        [Display(Name = "Название Атрибута")]
        public string Name { get; set; }
        [Display(Name = "Порядок фильтров")]
        public int DisplayOrder { get; set; }
        public ICollection<ChoiceAttributeOption> ChoiceAttributeOptions { get; set; }
    }

    public class ChoiceAttributeOption
    {
        public int ChoiceAttributeOptionId { get; set; }
         [Display(Name = "Название Атрибута")]
        public int ProductChoiceAttributeId { get; set; }
        [Display(Name = "Значение")]
        public string Name { get; set; }
         [Display(Name = "Порядок фильтров")]
        public int DisplayOrder { get; set; }

        public virtual ProductChoiceAttribute ProductChoiceAttribute { get; set; }
    }

}