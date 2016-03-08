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
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

       public AccountController(DataManager dataManager)
           : base(dataManager)
       {
        
       }

        // GET: Default/Account
        public ActionResult Index()
        {
            return View();
        }
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
                    return Json(new { success = true, url = url });
                default:
                    ModelState.AddModelError("", @"Неверное имя пользователя или пароль");
                    return PartialView("_LoginPartial", model);
            }

        }
        [AllowAnonymous]
        [AjaxOnly]
        public ActionResult Register()
        {
            var countries = DataManager.Countries.GetAll().OrderBy(t => t.Name).ToList();
            if (countries.Any())
            {
                ViewBag.CountryId =  new SelectList(countries, "Id", "Name",countries.First().Id);  
            }
            else
            {
                ViewBag.CountryId = new SelectList(countries, "Id", "Name");   
            }
            
            return PartialView("_RegisterPartial");
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
                    //   ViewBag.CountryId = new SelectList(db.Coutries.GetAll().ToList(), "Id", "Name", model.CountryId);
                    return PartialView("_RegisterPartial", model);
                }
                if (UserManager.Users.Any(t => t.PhoneNumber == model.Phone))
                {
                    ModelState.AddModelError("", @"Пользователь с таким номером телефоном уже зарегистрирован");
                    // ViewBag.CountryId = new SelectList(DataManager.Coutries.GetAll().ToList(), "Id", "Name", model.CountryId);
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
                    KindOfActivity = model.KindOfActivity

                };


                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await UserManager.AddToRoleAsync(user.Id, "Registered");

                    EmailSettings settings = new EmailSettings();

                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    //await UserManager.SendEmailAsync(user.Id, "Регистрация " + settings.Link, GetRegisterMessge(settings.Link, callbackUrl));
                    //return PartialView("_MustConfirmEmail", model);


                }

                AddErrors(result);
            }

            return PartialView("_RegisterPartial", model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Store");
        }

    }
}