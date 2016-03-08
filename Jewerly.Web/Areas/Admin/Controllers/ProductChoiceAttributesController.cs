using System.Linq;
using System.Net;
using System.Web.Mvc;
using Jewerly.Domain;
using Jewerly.Web.Controllers;

namespace Jewerly.Web.Areas.Admin.Controllers
{
    public class ProductChoiceAttributesController : BaseController
    {
       

        // GET: Admin/ProductChoiceAttributes
        public ActionResult Index()
        {
            return View(DataManager.ProductChoiceAttributes.GetAll().ToList());
        }

        // GET: Admin/ProductChoiceAttributes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductChoiceAttribute productChoiceAttribute = DataManager.ProductChoiceAttributes.GetById(id.Value);
            if (productChoiceAttribute == null)
            {
                return HttpNotFound();
            }
            return View(productChoiceAttribute);
        }

        // GET: Admin/ProductChoiceAttributes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/ProductChoiceAttributes/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductChoiceAttributeId,Name,DisplayOrder")] ProductChoiceAttribute productChoiceAttribute)
        {
            if (ModelState.IsValid)
            {
             
                DataManager.ProductChoiceAttributes.Insert(productChoiceAttribute);

                return RedirectToAction("Index");
            }

            return View(productChoiceAttribute);
        }

        // GET: Admin/ProductChoiceAttributes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductChoiceAttribute productChoiceAttribute = DataManager.ProductChoiceAttributes.GetById(id.Value);
            if (productChoiceAttribute == null)
            {
                return HttpNotFound();
            }
            return View(productChoiceAttribute);
        }

        // POST: Admin/ProductChoiceAttributes/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductChoiceAttributeId,Name,DisplayOrder")] ProductChoiceAttribute productChoiceAttribute)
        {
            if (ModelState.IsValid)
            {
             
                DataManager.ProductChoiceAttributes.Edit(productChoiceAttribute);

                return RedirectToAction("Index");
            }
            return View(productChoiceAttribute);
        }

        // GET: Admin/ProductChoiceAttributes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductChoiceAttribute productChoiceAttribute = DataManager.ProductChoiceAttributes.GetById(id.Value);
            if (productChoiceAttribute == null)
            {
                return HttpNotFound();
            }
            return View(productChoiceAttribute);
        }

        // POST: Admin/ProductChoiceAttributes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductChoiceAttribute productChoiceAttribute = DataManager.ProductChoiceAttributes.GetById(id);
            
            DataManager.ProductChoiceAttributes.Delete(productChoiceAttribute);
            return RedirectToAction("Index");
        }

      

        public ProductChoiceAttributesController(DataManager dataManager) : base(dataManager)
        {
        }
    }
}
