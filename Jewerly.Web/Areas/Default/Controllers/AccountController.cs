using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Jewerly.Domain;
using Jewerly.Web.Areas.Default.Models;
using Jewerly.Web.Controllers;
using Jewerly.Web.Models;
using Jewerly.Web.Utils;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace Jewerly.Web.Areas.Default.Controllers
{
    public class AccountController : BaseController
    {

        #region Ctor

        public AccountController(DataManager dataManager)
            : base(dataManager)
        {

        }

        #endregion

        #region Fields

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ApplicationSignInManager SignInManager
        {
            get { return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>(); }
            private set { _signInManager = value; }
        }

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        private IAuthenticationManager AuthenticationManager
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }

        #endregion

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private string GetRegisterMessge(string link, string code)
        {
            // var settings = new FashionStones.Utils.EmailSettings();
            var tb = new TagBuilder("a");
            tb.MergeAttribute("href", link);
            tb.SetInnerText(link);
            tb.ToString(TagRenderMode.SelfClosing);

            var callBack = new TagBuilder("a");
            callBack.MergeAttribute("href", code);
            callBack.SetInnerText("ссылке");
            callBack.ToString(TagRenderMode.SelfClosing);


            return string.Format("Регистрация на сайте {0}" + "{2}" +
                           "Здравствуйте! {2}" +
                            "Поздравляем, Вы зарегистрировались на сайте {0} {2}" +
                            "Для завершения регистрации, перейдите, пожалуйста, по этой {1}.{2}" +
                             "С уважением, команда {3}", link, callBack, "<br/>", tb);
        }

        #endregion

        #region Actions

        #region Index

        public ActionResult Index()
        {
            return View();
        }

        #endregion

        #region Login

        [AjaxOnly]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return PartialView("_LoginPartial");
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]

        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_LoginPartial", model);
            }

            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", @"Неверное имя пользователя или пароль");
                return PartialView("_LoginPartial", model);
            }
            if (user.EmailConfirmed == false)
            {
                ModelState.AddModelError("", @"Пожалуйста, подтвердите Ваш E-mail");
                return PartialView("_LoginPartial", model);
            }

            var roles = UserManager.GetRoles(user.Id);
            if (roles.Contains("Registered"))
            {
                ModelState.AddModelError("", @"Менеджер еще не подтвердил Вашу заявку");
                return PartialView("_LoginPartial", model);
            }
            if (roles.Contains("Banned"))
            {
                ModelState.AddModelError("", @"Ваш аккаунт заблокирован");
                return PartialView("_LoginPartial", model);
            }

            var result =
                await
                    SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe,
                        shouldLockout: false);

            switch (result)
            {
                case SignInStatus.Success:
                    string url = Url.Action("Index", "Store");
                    SetCookie("Currency", user.CurrencyId.ToString());
                    return Json(new {success = true, url = url});
                default:
                    ModelState.AddModelError("", @"Неверное имя пользователя или пароль");
                    return PartialView("_LoginPartial", model);
            }

        }

        #endregion

        #region Register

        [AllowAnonymous]
        [AjaxOnly]
        public ActionResult Register()
        {
            var countries = DataManager.Countries.GetAll().OrderBy(t => t.Name).ToList();
            if (countries.Any())
            {
                ViewBag.CountryId = new SelectList(countries, "Id", "Name", countries.First().Id);
            }
            else
            {
                ViewBag.CountryId = new SelectList(countries, "Id", "Name");
            }

            var model = new RegisterViewModel
            {
                AgreeWithConditions = true,
                City = "Odessa",
                ConfirmPassword = "123123",
                Password = "123123",
                Email = "rt@ua.fm",
                FirstName = "dima",
                MiddleName = "dima",
                LastName = "dima",
                KindOfActivity = "123"
            };



            return PartialView("_RegisterPartial",model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {

                if (UserManager.Users.Any(t => t.Email == model.Email))
                {
                    ModelState.AddModelError("", @"Пользователь с таким email уже зарегистрирован");
                    ViewBag.CountryId = new SelectList(DataManager.Countries.GetAll().OrderBy(t=>t.Name), "Id", "Name", model.CountryId);
                    return PartialView("_RegisterPartial", model);
                }
                if (UserManager.Users.Any(t => t.PhoneNumber == model.Phone))
                {
                    ModelState.AddModelError("", @"Пользователь с таким номером телефоном уже зарегистрирован");
                    ViewBag.CountryId = new SelectList(DataManager.Countries.GetAll().OrderBy(t => t.Name), "Id", "Name", model.CountryId);
                    return PartialView("_RegisterPartial", model);
                }

                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    MiddleName = model.MiddleName,
                    City = model.City,
                    CountryId = model.CountryId,
                    PhoneNumber = model.Phone,
                    KindOfActivity = model.KindOfActivity,
                    CurrencyId = DefaultCurrency

                };


                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await UserManager.AddToRoleAsync(user.Id, "Registered");

                    EmailSettings settings = new EmailSettings();

                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    await UserManager.SendEmailAsync(user.Id, "Регистрация " + settings.Link, GetRegisterMessge(settings.Link, callbackUrl));

                    ViewBag.Name = model.FirstName + " " + model.LastName;
                    ViewBag.Email = model.Email;
                    return PartialView("_MustConfirmEmail");

                }

                AddErrors(result);
            }
            ViewBag.CountryId = new SelectList(DataManager.Countries.GetAll().OrderBy(t => t.Name).ToList(), "Id", "Name");
            return PartialView("_RegisterPartial", model);
        }

        #endregion

        #region LogOff

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Store");
        }

        #endregion

        #region ForgotPassword

        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("", @"Пользователь с таким e-mail не зарегистрирован");
                    return PartialView(model);
                }



                EmailSettings settings = new EmailSettings();
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new {userId = user.Id, code = code},
                    protocol: Request.Url.Scheme);
                await UserManager.SendEmailAsync(user.Id, settings.Link+" Восстановление пароля",
                    "Здравствуйте! <br/>Вы отправили запрос на восстановление пароля от аккаунта " + user.Email +
                    " .<br/>" +
                    "Для того чтобы задать новый пароль, перейдите по  <a href=\"" + callbackUrl + "\">ссылке</a><br/>" +
                    "С уважением, команда <a href=\"" + settings.Link + "\">" + settings.Link + "</a>");
                return PartialView("ForgotPasswordConfirmation");
           }

            // If we got this far, something failed, redisplay form
            return PartialView(model);
        }


        #endregion

        #region ConfirmEmail

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        #endregion

        #region ResetPassword

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return PartialView();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        #endregion



      }

    #endregion

}