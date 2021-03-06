﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Jewerly.Web.Areas.Default.Models
{

        public class ForgotViewModel
        {
            [Display(Name = "E-mail")]
            public string Email { get; set; }
        }

        public class LoginViewModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "E-mail")]
            public string Email { get; set; }
              [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Пароль")]
            public string Password { get; set; }

            [Display(Name = "Запомнить меня")]
            public bool RememberMe { get; set; }
        }

        public class RegisterViewModel
        {
            public RegisterViewModel()
            {
                AgreeWithConditions = true;
            }
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
            [Display(Name = "Страна")]
            public int CountryId { get; set; }
            [Required]
            [Display(Name = "Город")]
            public string City { get; set; }
            [Required]
            [Display(Name = "Телефон")]
          //  [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Entered phone format is not valid.")]
          //  [RegularExpression(@"^[0-9]{7,12}$", ErrorMessage = "Введите номер теленфона")]
            [RegularExpression(@"^(\+)?(\d{3,5})?\d{7,10}$", ErrorMessage = "Неверный номер теленфона")]

            public string Phone { get; set; }
            //    [RegularExpression(@"^([a-z0-9_-]+\.)*[a-z0-9_-]+@[a-z0-9_-]+(\.[a-z0-9_-]+)*\.[a-z]{2,6}$", ErrorMessageResourceType = typeof(GlobalResource), ErrorMessageResourceName = "ErrorMessageRegularExpressionEmail")]
            [Display(Name = "E-mail")]
            public string Email { get; set; }
              [Required]
              [DataType(DataType.Password)]
            [Display(Name = "Пароль")]
            public string Password { get; set; }
              [Required]
              [DataType(DataType.Password)]
              [Display(Name = "Подтверждение пароля")]
              public string ConfirmPassword { get; set; }
              [Required]
              [Display(Name = "Вид деятяльности")]
              public string KindOfActivity { get; set; }
            [Required]
            [Display(Name = "Согласен с условиями регистрации")]
            public bool AgreeWithConditions { get; set; }

        }

        public class ResetPasswordViewModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "E-mail")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "{0} Минимальная длина пароля {2} символов.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Пароль")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Повторите пароль")]
            [Compare("Password", ErrorMessage = "Введенные пароли не совпадают.")]
            public string ConfirmPassword { get; set; }

            public string Code { get; set; }
        }

        public class ForgotPasswordViewModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "E-mail")]
            public string Email { get; set; }
        }

        public class ChangePasswordViewModel
        {
            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Текущий пароль")]
            public string OldPassword { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "{0} должен быть минимум {2} символов длиной.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Новый пароль")]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Повторите новый пароль")]
            [Compare("NewPassword", ErrorMessage = "Введенные пароли не совпадают.")]
            public string ConfirmPassword { get; set; }
        }
    
}