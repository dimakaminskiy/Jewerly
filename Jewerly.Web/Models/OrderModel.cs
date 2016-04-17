using System.ComponentModel.DataAnnotations;

namespace Jewerly.Web.Models
{
    public class OrderModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите Имя")]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Введите Фамилию")]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Введите Отчество")]
        [Display(Name = "Отчество")]
        public string MiddleName { get; set; }

        [Required(ErrorMessage = "Введите Телефон")]
        [Display(Name = "Телефон")]
        [RegularExpression(@"^(\+)?(\d{3,5})?\d{7,10}$", ErrorMessage = "Неверный номер теленфона")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Введите E-mail")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }        
       
        [Display(Name = "Страна")]
        public int CountryId { get; set; }
        [Required(ErrorMessage = "Введите Город")]
        [Display(Name = "Город")]
        public string City { get; set; }
        
        [Display(Name = "Способ оплаты")]
        public int MethodOfPaymentId { get; set; }
        
        [Display(Name = "Метод доставки")]
        public int MethodOfDeliveryId { get; set; }

        [Display(Name = "Комментарии")]
        public string TextInfo { get; set; }

       
       
     }
}