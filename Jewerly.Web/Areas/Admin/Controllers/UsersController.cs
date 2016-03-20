using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Jewerly.Domain;
using Jewerly.Web.extensions;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Jewerly.Web.Areas.Admin.Controllers
{
    public class UsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        private IQueryable<ApplicationUser> GetUsersByRoleName(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
                return db.Users;

            var role = db.Roles.FirstOrDefault(t => t.Name == roleName);
            if (role == null) return null;

            var users = (from u in db.Users
                where u.Roles.Any(r => r.RoleId == role.Id)
                select u);

            return users;
        }
        private string GetRoleByUserId(string id)
        {
            return UserManager.GetRoles(id).First();
        } 

        private SelectList GetSelectListRoles(string roleName)
        {
            SelectList list;
            if (string.IsNullOrEmpty(roleName))
            {
                list =
                    new SelectList(db.Roles.OrderBy(t => t.Name), "Name", "Name")
                        .PreAppend("-----------", "", true);
            }
            else
            {
                list =
                    new SelectList(db.Roles.OrderBy(t => t.Name), "Name", "Name", roleName)
                        .PreAppend("-----------", "", false);
            }
            return list;
        }


        void Test()
        {
            testAddUsersMember(1, 2, "Administrator");
            testAddUsersMember(3, 20, "Banned");
            testAddUsersMember(21, 50, "Member");
            testAddUsersMember(51, 70, "Registered");

        }



        private  void testAddUsersMember(int start, int stop,string roleName)
        {


            for (int i = start; i < stop; i++)
            {
                var user = new ApplicationUser
                {
                    UserName = "test" + (i + 1),
                    Email = "test" + (i + 1) + "@ua.fm",
                    FirstName = "test" + (i + 1),
                    LastName = "test" + (i + 1),
                    MiddleName = "test" + (i + 1),
                    City = "test" + (i + 1),
                    CountryId = 1,
                    PhoneNumber = "123456789",
                    KindOfActivity = "test",
                    CurrencyId = 1

                };
                var result =  UserManager.Create(user, "123123");
                UserManager.AddToRole(user.Id, roleName);
            }

        }


        public ActionResult Index(string userName,string roleName)
        {

            var g = Request["userName"];

           // Test();
            var queryableUsers = GetUsersByRoleName(roleName);
            List<ApplicationUser> users;

            if (queryableUsers == null) users = new List<ApplicationUser>();
            else
            {
                if (!string.IsNullOrEmpty(userName))
                    users = queryableUsers.Where(t => t.LastName.Contains(userName)).OrderBy(t => t.LastName).ToList();
                else
                users = queryableUsers.OrderBy(t => t.LastName).ToList();
            }

            ViewBag.roleName = GetSelectListRoles(roleName);
            ViewBag.role = roleName;
            ViewBag.userName = userName;
            return View(users);
        }

        // GET: Admin/Users/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }

            var role = GetRoleByUserId(applicationUser.Id);
            ViewBag.RoleName = new SelectList(db.Roles.OrderBy(t => t.Name), "Name", "Name", role);
            ViewBag.Role = role;
            
            ViewBag.CountryId = new SelectList(db.Countries, "Id", "Name", applicationUser.CountryId);
            ViewBag.CurrencyId = new SelectList(db.Currencies, "CurrencyId", "Name", applicationUser.CurrencyId);
            return View(applicationUser);
        }

        // POST: Admin/Users/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,LastName,FirstName,MiddleName,CountryId,CurrencyId,City," +
                                                 "KindOfActivity,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber," +
                                                 "PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled," +
                                                 "AccessFailedCount,UserName")] ApplicationUser applicationUser, string roleName)
        {
            if (ModelState.IsValid)
            {
                db.Entry(applicationUser).State = EntityState.Modified;
                db.SaveChanges();
                var oldRole = GetRoleByUserId(applicationUser.Id);
                if (oldRole != roleName)
                {
                    UserManager.RemoveFromRole(applicationUser.Id, oldRole);
                    UserManager.AddToRole(applicationUser.Id, roleName);
                }
                return RedirectToAction("Index");
            }
            var role = GetRoleByUserId(applicationUser.Id);
            ViewBag.RoleName = new SelectList(db.Roles.OrderBy(t => t.Name), "Name", "Name", role);
            ViewBag.Role = role;

            ViewBag.CountryId = new SelectList(db.Countries, "Id", "Name", applicationUser.CountryId);
            ViewBag.CurrencyId = new SelectList(db.Currencies, "CurrencyId", "Name", applicationUser.CurrencyId);
            return View(applicationUser);
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
