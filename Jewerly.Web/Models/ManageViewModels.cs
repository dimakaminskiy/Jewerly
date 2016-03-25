using System.ComponentModel.DataAnnotations;
namespace Jewerly.Web.Models
{
    public class UserInfo
    {
        [Required]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Отчество")]
        public string MiddleName { get; set; }
        [Required]
        [Display(Name = "Телефон")]
        [RegularExpression(@"^(\+)?(\d{3,5})?\d{7,10}$", ErrorMessage = "Введите номер теленфона")]
        public string Phone { get; set; }
        [Required]
        [Display(Name = "Страна")]
        public int CountryId { get; set; }
        [Required]
        [Display(Name = "Валюта")]
        public int CurrencyId { get; set; }
        [Required]
        [Display(Name = "Город")]
        public string City { get; set; }
        [Required]
        [Display(Name = "Вид деятельности")]
        public string KindOfActivity { get; set; }
    }







    //    public class IndexViewModel
    //    {
    //        public bool HasPassword { get; set; }
    //        public IList<UserLoginInfo> Logins { get; set; }
    //        public string PhoneNumber { get; set; }
    //        public bool TwoFactor { get; set; }
    //        public bool BrowserRemembered { get; set; }
    //    }

    //    public class ManageLoginsViewModel
    //    {
    //        public IList<UserLoginInfo> CurrentLogins { get; set; }
    //        public IList<AuthenticationDescription> OtherLogins { get; set; }
    //    }

    //    public class FactorViewModel
    //    {
    //        public string Purpose { get; set; }
    //    }

    //    public class SetPasswordViewModel
    //    {
    //        [Required]
    //        [StringLength(100, ErrorMessage = "Значение {0} должно содержать символов не менее: {2}.", MinimumLength = 6)]
    //        [DataType(DataType.Password)]
    //        [Display(Name = "Новый пароль")]
    //        public string NewPassword { get; set; }

    //        [DataType(DataType.Password)]
    //        [Display(Name = "Подтверждение нового пароля")]
    //        [Compare("NewPassword", ErrorMessage = "Новый пароль и его подтверждение не совпадают.")]
    //        public string ConfirmPassword { get; set; }
    //    }

    //    public class ChangePasswordViewModel
    //    {
    //        [Required]
    //        [DataType(DataType.Password)]
    //        [Display(Name = "Текущий пароль")]
    //        public string OldPassword { get; set; }

    //        [Required]
    //        [StringLength(100, ErrorMessage = "Значение {0} должно содержать символов не менее: {2}.", MinimumLength = 6)]
    //        [DataType(DataType.Password)]
    //        [Display(Name = "Новый пароль")]
    //        public string NewPassword { get; set; }

    //        [DataType(DataType.Password)]
    //        [Display(Name = "Подтверждение нового пароля")]
    //        [Compare("NewPassword", ErrorMessage = "Новый пароль и его подтверждение не совпадают.")]
    //        public string ConfirmPassword { get; set; }
    //    }

    //    public class AddPhoneNumberViewModel
    //    {
    //        [Required]
    //        [Phone]
    //        [Display(Name = "Номер телефона")]
    //        public string Number { get; set; }
    //    }

    //    public class VerifyPhoneNumberViewModel
    //    {
    //        [Required]
    //        [Display(Name = "Код")]
    //        public string Code { get; set; }

    //        [Required]
    //        [Phone]
    //        [Display(Name = "Номер телефона")]
    //        public string PhoneNumber { get; set; }
    //    }

    //    public class ConfigureTwoFactorViewModel
    //    {
    //        public string SelectedProvider { get; set; }
    //        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
    //    }
}