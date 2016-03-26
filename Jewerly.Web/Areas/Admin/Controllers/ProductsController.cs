using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Jewerly.Domain;
using Jewerly.Web.Controllers;
using Jewerly.Web.Models;
using Jewerly.Web.Utils;
using Jewerly.Web.extensions;


namespace Jewerly.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ProductsController : BaseController
    {

        #region Ctor

        public ProductsController(DataManager dataManager)
            : base(dataManager)
        {
        }

        #endregion

        #region Helpers

        private SelectList GetSelectListPictures(int id)
        {
            SelectList list;
            var selectedPic = DataManager.Products.GetAll().Select(t => t.PictureId).ToArray();
            if (id == 0)
            {
                list =
                    new SelectList(DataManager.Pictures.SearchFor(t=>selectedPic.All(g=>g!=t.Id)).OrderBy(x => x.Caption),
                        "Id", "Caption").PreAppend( "-----------", "", true);
            }
            else
            {
                list =
                   new SelectList(DataManager.Pictures.GetAll().OrderBy(x => x.Caption), "Id", "Caption", id).PreAppend("-----------", "", false);
            }
            return list;
        }
        private SelectList GetSelectListDiscount(int? id)
        {
            SelectList list;

            if (id == null)
            {
                list =
                    new  SelectList(DataManager.Discounts.GetAll().OrderBy(x => x.Name), "Id", "Name").PreAppend("-----------", "", true);
            }
            else
            {
                list =
                   new SelectList(DataManager.Discounts.GetAll().OrderBy(x => x.Name), "Id", "Name", id).PreAppend( "-----------", "", false);
            }
            return list;
        }
        private SelectList GetSelectListCategories(int id)
        {
            SelectList list;
            var categories = DataManager.Categories.GetAll().OrderBy(x => x.Name);
            if (id == 0)
            {
                list =
                 new SelectList(categories, "Id", "Name", categories.First().Id).PreAppend( "-----------", "", true);
            }
            else
            {
                list =
                  new SelectList(categories, "Id", "Name", id).PreAppend("-----------", "", false);
            }
            return list;
        }
        #endregion

        #region Product Actions

        public ActionResult Index(string searchString, string categoryId, int page = 1)
        {
            IQueryable<Product> products = DataManager.Products.GetAll();
            if (!string.IsNullOrEmpty(categoryId)  && DataManager.Categories.Count(t=>t.Id.ToString()==categoryId)!=0)
            {
                products = products.Where(t => t.CategoryId.ToString() == categoryId);
            }
            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(t => t.ProductId.ToString().Contains(searchString));
            }
            products = products.OrderBy(t => t.ProductId);
            var count = products.Count();
            int countItemOnpage = 5;

            products = products.Skip((page - 1)*countItemOnpage)
                .Take(countItemOnpage);
            
            if (!string.IsNullOrEmpty(categoryId) && categoryId != 0.ToString())
            {
                ViewBag.CategoryName =
                    DataManager.Categories.SearchFor(t => t.Id.ToString() == categoryId).Single().Name;
            }


            if (!string.IsNullOrEmpty(categoryId))
            {
                int h = int.Parse(categoryId);
                var cat = DataManager.Categories.SearchFor(t => t.Id == h).SingleOrDefault();
                if (cat != null)
                {
                    ViewBag.CategoryName = cat.Name;
                }
            }
            if (!string.IsNullOrEmpty(searchString))
            {
                ViewBag.SearchString = searchString;
            }


    
            ViewBag.PageNo = page;
            ViewBag.CategoryId = categoryId;
            ViewBag.CountPage = (int)decimal.Remainder(count, countItemOnpage) == 0 ? count / countItemOnpage : count / countItemOnpage + 1;
            ViewBag.Category = GetSelectListCategories(string.IsNullOrEmpty(categoryId)?0:int.Parse(categoryId));
            return View(products.ToList());
        }

        public ActionResult Create()
        {
            var product = new Product();
            product.DiscountId = null;

            ViewBag.PictureId = GetSelectListPictures(product.PictureId);

            ViewBag.DiscountId = GetSelectListDiscount(product.DiscountId);
            ViewBag.MarkupId = new SelectList(DataManager.Markups.GetAll(), "Id", "Name");
            ViewBag.CategoryId = GetSelectListCategories(product.CategoryId);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductId,Name,SeoName,ShortDescription,FullDescription,Price,Published,PictureId,MarkupId,DiscountId,CategoryId")] Product product)
        {
            if (ModelState.IsValid)
            {
                product.Name = product.Name.Trim();
                product.SeoName = string.IsNullOrEmpty(product.SeoName)
                    ? product.Name.ToTranslit()
                    : product.SeoName.Trim();

                DataManager.Products.Insert(product);

                TempData["message"] = string.Format("Продукт \"{0}\" был создан", product.Name);
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = GetSelectListCategories(product.CategoryId);
            ViewBag.DiscountId = GetSelectListDiscount(product.DiscountId);
            ViewBag.MarkupId = new SelectList(DataManager.Markups.GetAll(), "Id", "Name",product.MarkupId);
            ViewBag.PictureId = GetSelectListPictures(product.PictureId);
            return View(product);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = DataManager.Products.GetById(id.Value);
            
            if (product == null)
            {
                return HttpNotFound();
            }

            ViewBag.CategoryId = GetSelectListCategories(product.CategoryId);
            ViewBag.DiscountId = GetSelectListDiscount(product.DiscountId);
            ViewBag.MarkupId = new SelectList(DataManager.Markups.GetAll(), "Id", "Name", product.MarkupId);
            ViewBag.prodId = id;
            ViewBag.PictureId = GetSelectListPictures(product.PictureId);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductId,Name,SeoName,ShortDescription,FullDescription,Price,Published,PictureId,MarkupId,DiscountId,CategoryId")] Product product)
        {
            if (ModelState.IsValid)
            {

                product.Name = product.Name.Trim();
                product.SeoName = string.IsNullOrEmpty(product.SeoName)
                    ? product.Name.ToTranslit()
                    : product.SeoName.Trim();

                DataManager.Products.Edit(product);

                TempData["message"] = string.Format("Изменения в продукте \"{0}\" были сохранены", product.Name);
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = GetSelectListCategories(product.CategoryId);
            ViewBag.DiscountId = GetSelectListDiscount(product.DiscountId);
            ViewBag.MarkupId = new SelectList(DataManager.Markups.GetAll(), "Id", "Name", product.MarkupId);
            ViewBag.PictureId = GetSelectListPictures(product.PictureId);
            return View(product);
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = DataManager.Products.GetById(id.Value);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var product = DataManager.Products.GetById(id);

            if (DataManager.OrderDetails.SearchFor(t => t.ProductId == id).Count() != 0)
            {
                TempData["error"] = string.Format("Есть записи о  \"{0}\" в заказах пользователей. Удаление не возможно", product.Name);
                return RedirectToAction("Delete",new {id=id});
            }

            if (DataManager.Carts.SearchFor(t => t.ProductId == id).Count() != 0)
            {
                TempData["error"] = string.Format("Есть записи о  \"{0}\" в корзинах пользователей. Удаление не возможно", product.Name);
                return RedirectToAction("Delete", new { id = id });
            }
            
            DataManager.Products.Delete(product);

            TempData["message"] = string.Format("Продукт \"{0}\" был удален", product.Name);
            return RedirectToAction("Index");
        }

        #endregion

        #region Product Actions
        public ActionResult ProductSpecificationAttributes(int prodId)
        {
            var attributes = DataManager.ProductSpecificationAttributes.GetAll().ToList();
            List<ProductSpecificationAttribiteViewModel> model = new List<ProductSpecificationAttribiteViewModel>();

            foreach (var attr in attributes)
            {
                var options = DataManager.SpecificationAttributeOptions.SearchFor(
                    t => t.ProductSpecificationAttributeId == attr.ProductSpecificationAttributeId)
                    .OrderBy(t => t.DisplayOrder)
                    .ThenBy(t => t.Name)
                    .Select(p => new {Id = p.SpecificationAttributeOptionId, Name = p.Name}).ToList();

                if (options.Any())
                {
                    model.Add(new ProductSpecificationAttribiteViewModel()
                    {
                        AttributeId = attr.ProductSpecificationAttributeId,
                        Name = attr.Name,
                        Options = new SelectList(options, "Id", "Name", options.First().Id)
                    });
                }
                else
                {
                    model.Add(new ProductSpecificationAttribiteViewModel()
                    {
                        AttributeId = attr.ProductSpecificationAttributeId,
                        Name = attr.Name,
                        Options = new SelectList(options, "Id", "Name")
                    });
                }


            }

            ViewBag.prodId = prodId;
            return PartialView(model);

        }
        public ActionResult ProductSpecificationAttributeOptions(int prodSpecAttrId)
        {

            var result = DataManager.SpecificationAttributeOptions
                .SearchFor(t => t.ProductSpecificationAttributeId == prodSpecAttrId).OrderBy(t => t.DisplayOrder)
                .ThenBy(t => t.Name).ToList().Select(p => new
                {
                    Id = p.SpecificationAttributeOptionId,
                    Name = p.Name,
                });

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult AddAttributeToProduct(int prodId, int attrId, int attrOptionId)
        {
            Product product = DataManager.Products.GetById(prodId);
            ProductSpecificationAttribute attribute = DataManager.ProductSpecificationAttributes.GetById(attrId);
            SpecificationAttributeOption option = DataManager.SpecificationAttributeOptions.GetById(attrOptionId);


            var g =
                DataManager.SpecificationAttributeOptions.GetAll()
                    .Select(t => new {Id = t.ProductSpecificationAttributeId, Name = t.Name})
                    .ToList();

            if (product == null || attribute == null || option == null)
            {
                return Json(new
                {
                    success = false,
                    errorMessage = "Ошибка добавления атрибута",
                });
            }

            DataManager.MappingProductSpecificationAttributeToProducts.Insert(new MappingProductSpecificationAttributeToProduct
                ()
            {
                ProductId = prodId,
                ProductSpecificationAttributeId = attrId,
                SpecificationAttributeOptionId = attrOptionId
            });

            TempData["message"] = string.Format("Атрибут \"{0}\" -> \"{1}\" был добавлен к товару \"{2}\"",
                attribute.Name, option.Name, product.Name);

            return Json(new
            {
                success = true,
                errorMessage = string.Empty,
                url = Url.Action("Edit", new {id = prodId})
            });

        }
        [HttpPost]
        public ActionResult DeleteAttributeFromProduct(int id)
        {
            var attr = DataManager.MappingProductSpecificationAttributeToProducts.GetById(id);
            var name = attr.SpecificationAttributeOption.Name;
            DataManager.MappingProductSpecificationAttributeToProducts.Delete(attr);

            TempData["message"] = string.Format("Атрибут \"{0}\" был удален", name);

            return Json(new
            {
                success = true,
                errorMessage = string.Empty,
                url = Url.Action("Edit", new {id = attr.ProductId})
            });
        }
        public ActionResult ProductChoiceAttributes(int prodId)
        {
            var attributes = DataManager.ProductChoiceAttributes.GetAll()
                .Where(t => t.ChoiceAttributeOptions.Any()).OrderBy(t => t.DisplayOrder).ThenBy(t => t.Name).ToList();

            var model = new ProductChoiceAttributesViewModel();
            model.ProductId = prodId;


            if (attributes.Any())
            {
                var fAttr = attributes.First();
                model.ProductChoiceAttributeId = fAttr.ProductChoiceAttributeId;
                var options =
                    DataManager.ChoiceAttributeOptions.SearchFor(
                        t => t.ProductChoiceAttributeId == fAttr.ProductChoiceAttributeId).
                        Select(p => new ChoiceAttributeOptionViewModel()
                        {
                            ProductChoiceAttributeId = p.ProductChoiceAttributeId,
                            ChoiceAttributeOptionId = p.ChoiceAttributeOptionId,
                            Name = p.Name,
                            DisplayOrder = p.DisplayOrder
                        }).ToList();

                model.ProductChoiceAttributes = attributes;
                model.ChoiceAttributeOptions = options;
             }
            else
            {
                 model.ProductChoiceAttributes= new List<ProductChoiceAttribute>();
                 model.ChoiceAttributeOptions = new List<ChoiceAttributeOptionViewModel>();
            }




            return PartialView(model);

        }
        public ActionResult ProductChoiceAttributeOptions(int prodAttrId)
        {
            var result = DataManager.ChoiceAttributeOptions
              .SearchFor(t => t.ProductChoiceAttributeId == prodAttrId).OrderBy(t => t.DisplayOrder)
              .ThenBy(t => t.Name).ToList().Select(p => new
              {
                  Id = p.ChoiceAttributeOptionId,
                  Name = p.Name,
              });

            return Json(
                       new
                       {
                           success = true,
                           result = result,
                       }, JsonRequestBehavior.AllowGet);


           
        }
        public ActionResult ProductNameByCategoryId(int Id)
        {
            var result = DataManager.Categories.GetById(Id);
            return Json(
                       new
                       {
                           success = true,
                           name = result.ProductName,
                       }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ProductPicturebyId(int id)
        {
            var picture = DataManager.Pictures.GetById(id);
            //if (picture == null)
            //{
                
            //}

            //string pic = result.Preview();
            return Json(
                       new
                       {
                           success = true,
                           name = (picture==null)? "/Content/img/NoImage.gif" : picture.Preview(),
                       }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult DeleteChoiceAttributeFromProduct(int id)
        {
            var attr = DataManager.MappingProductChoiceAttributeToProducts.GetById(id);
            var name = attr.ProductChoiceAttribute.Name;
          var attrOptions = DataManager.AvalibleChoiceAttributeOptions.SearchFor(t => t.MappingProductChoiceAttributeToProductId == id)
                .ToList();

            foreach (var attrOption in attrOptions)
            {
                DataManager.AvalibleChoiceAttributeOptions.Delete(attrOption);
            }

            DataManager.MappingProductChoiceAttributeToProducts.Delete(attr);

            TempData["message"] = string.Format("Атрибут \"{0}\" был удален", name);

            return Json(new
            {
                success = true,
                errorMessage = string.Empty,
                url = Url.Action("Edit", new { id = attr.ProductId })
            });

        }
        [HttpPost]
        public ActionResult AddChoiceAttributesToProduct(ProductChoiceAttributesViewModel model)
        {
            var avalibleAttrOptions = model.ChoiceAttributeOptions.Where(t => t.Available).ToList();
            var attrid = model.ProductChoiceAttributeId;
            var prodId = model.ProductId;
            var prod = DataManager.Products.GetById(prodId);
            var mapAttrToProd = new MappingProductChoiceAttributeToProduct();
            mapAttrToProd.ProductId = prodId;
            mapAttrToProd.ProductChoiceAttributeId = attrid;
            DataManager.MappingProductChoiceAttributeToProducts.Insert(mapAttrToProd);

            foreach (var option in avalibleAttrOptions)
            {
                var t = new AvalibleChoiceAttributeOption()
                {
                    MappingProductChoiceAttributeToProductId = mapAttrToProd.MappingProductChoiceAttributeToProductId,
                    ChoiceAttributeOptionId = option.ChoiceAttributeOptionId
                };
                DataManager.AvalibleChoiceAttributeOptions.Insert(t);
            }

            TempData["message"] = string.Format("Атрибуты были добавлены к товару \"{0}\"", prod.Name);

            return Json(new
            {
                success = true,
                errorMessage = string.Empty,
                url = Url.Action("Edit", new { id = prodId})
            });
          }

        #endregion
    }
}
