using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Jewerly.Domain;
using Jewerly.Web.Areas.Default.Models;
using Jewerly.Web.Controllers;
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


        public ActionResult Myinfo()
        {
          //  var model = new UserInfo();
            return View();
        }
        public ActionResult OrderHistory()
        {
            return View();
        }
    }
}