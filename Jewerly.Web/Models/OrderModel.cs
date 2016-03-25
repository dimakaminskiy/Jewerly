using System.ComponentModel.DataAnnotations;

namespace Jewerly.Web.Models
{
    public class OrderModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }
        
        [Required]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }
        
        [Required]
        [Display(Name = "Отчество")]
        public string MiddleName { get; set; }
        
        [Required]
        [Display(Name = "Телефон")]
        [RegularExpression(@"^(\+)?(\d{3,5})?\d{7,10}$", ErrorMessage = "Неверный номер теленфона")]
        public string Phone { get; set; }
        
        [Required]
        [Display(Name = "E-mail")]
        public string Email { get; set; }        
       
        [Display(Name = "Страна")]
        public int CountryId { get; set; }
        [Required]
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