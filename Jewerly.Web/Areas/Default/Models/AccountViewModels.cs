using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Jewerly.Web.Areas.Default.Models
{

        public class ForgotViewModel
        {
            public string Email { get; set; }
        }

        public class LoginViewModel
        {
            [EmailAddress]
            public string Email { get; set; }
            [DataType(DataType.Password)]
            public string Password { get; set; }
            public bool RememberMe { get; set; }
        }

        public class RegisterViewModel
        {
            public RegisterViewModel()
            {
                AgreeWithConditions = true;
            }
            [Required]
            public string LastName { get; set; }
            [Required]
            public string FirstName { get; set; }
              [Required]
            public string MiddleName { get; set; }
              [Required]
            public int CountryId { get; set; }
              [Required]
            public string City { get; set; }
              [Required]
            public string Phone { get; set; }
            //    [RegularExpression(@"^([a-z0-9_-]+\.)*[a-z0-9_-]+@[a-z0-9_-]+(\.[a-z0-9_-]+)*\.[a-z]{2,6}$", ErrorMessageResourceType = typeof(GlobalResource), ErrorMessageResourceName = "ErrorMessageRegularExpressionEmail")]
            public string Email { get; set; }
              [Required]
            public string Password { get; set; }
              [Required]
            public string ConfirmPassword { get; set; }
              [Required]
            public string KindOfActivity { get; set; }
            [Required]
            public bool AgreeWithConditions { get; set; }

        }

        public class ResetPasswordViewModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            public string Code { get; set; }
        }

        public class ForgotPasswordViewModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }
        }

        public class ChangePasswordViewModel
        {
            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Current password")]
            public string OldPassword { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "New password")]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm new password")]
            [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }
    
}