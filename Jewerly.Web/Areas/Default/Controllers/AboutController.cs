using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Jewerly.Domain;
using Jewerly.Web.Controllers;
using Jewerly.Web.Models;

namespace Jewerly.Web.Areas.Default.Controllers
{
    public class AboutController : BaseController
    {
        private List<CategoryModel> GetListMenuCategories()
        {
            List<CategoryModel> result = new List<CategoryModel>();

            foreach (var c in DataManager.Categories.SearchFor(t => t.ParentCategoryId == null && t.Published)
                    .OrderBy(t => t.Name).Select(t => new CategoryModel()
                    {
                        Id = t.Id,
                        Name = t.Name,
                        SeoName = t.SeoName
                    }).ToList())
            {
                result.Add(c);
            }


            foreach (var c in result)
            {
                List<CategoryModel> subcategories = new List<CategoryModel>();
                foreach (var s in DataManager.Categories.SearchFor(t => t.ParentCategoryId == c.Id && t.Published)
                    .OrderBy(t => t.Name).Select(t => new CategoryModel()
                    {
                        Id = t.Id,
                        Name = t.Name,
                        SeoName = t.SeoName
                    }).ToList())
                {
                    subcategories.Add(s);
                }
                c.SubCategories = subcategories;
            }

            return result;
        }




        public ActionResult Delivery()
        {
            return View();
        }

        public ActionResult Index()
        {
            var model = new MenuCategories(null,GetListMenuCategories());
  
            return View(model);
        }






        public ActionResult Contacts()
        {
            return View();
        }

        public AboutController(DataManager dataManager) : base(dataManager)
        {
        }
    }
}