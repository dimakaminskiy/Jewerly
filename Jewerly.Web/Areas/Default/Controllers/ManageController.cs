using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml.Schema;
using Jewerly.Domain;
using Jewerly.Web.Areas.Default.Models;
using Jewerly.Web.Controllers;
using Jewerly.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Jewerly.Web.Areas.Default.Controllers
{
    public class ManageController : BaseController
    {
        #region Ctor

        public ManageController(DataManager dataManager)
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

        #endregion

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        #endregion


        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    TempData["message"] = string.Format("\"{0}\" ваш пароль был изменён", user.FirstName+" "+user.LastName);
                }
                return RedirectToAction("Index");
            }
            AddErrors(result);
            return View(model);
        }


        public ActionResult UserInfo()
        {
           var u =  UserManager.FindById(User.Identity.GetUserId());
            var model = new UserInfo
            {
                CurrencyId = u.CurrencyId,
                LastName = u.LastName,
                FirstName = u.FirstName,
                MiddleName = u.MiddleName,
                CountryId = u.CountryId,
                City = u.City,
                Phone = u.PhoneNumber,
                KindOfActivity = u.KindOfActivity
            };
            
            
            
            return View(model);
        }

        [HttpPost]
        public ActionResult UserInfo(UserInfo model)
        {
            if (ModelState.IsValid)
            {
                var u = UserManager.FindById(User.Identity.GetUserId());
                u.City = model.City;
                u.CountryId = model.CurrencyId;
                u.CurrencyId = model.CurrencyId;
                u.FirstName = model.FirstName;
                u.LastName = model.LastName;
                u.MiddleName = model.MiddleName;
                u.PhoneNumber = model.Phone;
                
                return View(model);
            }




            return View();
        }

        public ActionResult OrderHistory()
        {
            return View();
        }
    }
}