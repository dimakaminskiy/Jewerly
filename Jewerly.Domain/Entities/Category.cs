using System.ComponentModel.DataAnnotations;

namespace Jewerly.Domain
{
    public class Category
    {
        public int Id { get; set; }
        [Display(Name = "Название")]
        [Required]
        public string Name { get; set; }
        [Display(Name = "Название продукта в категории")]
        public string ProductName { get; set; }
        
        [Display(Name = "Название SEO")]
        public string SeoName { get; set; }
        [Display(Name = "Опубликована?")]
        public bool Published { get; set; }
        [Display(Name = "Показывать на главной?")]
        public bool ShowOnHomePage { get; set; }
        [Display(Name = "Родительская категория")]
        public int? ParentCategoryId { get; set; }
        [Display(Name = "Изображение")]
        public int? CategoryPictureId { get; set; }
        public virtual CategoryPicture CategoryPicture { get; set; }

    }
}