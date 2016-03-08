using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Jewerly.Domain
{
    public class Picture
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Выберите изображение")]
        [Display(Name = "Изображение")]
        public string Path { get; set; }
        [Display(Name = "Название")]
        [Required]
        public string Caption { get; set; }
    }

    public class CategoryPicture
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Выберите изображение")]
        [Display(Name = "Изображение")]
        public string Path { get; set; }
        [Display(Name = "Название")]
        [Required]
        public string Caption { get; set; }
        [Display(Name = "Текст к атрибуту Alt")]
        public string AltAttribute { get; set; }
        [Display(Name = "Текст к атрибуту Title")]
        public string TitleAttribute { get; set; }

    }

}