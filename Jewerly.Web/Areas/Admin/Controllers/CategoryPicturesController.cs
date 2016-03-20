using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Jewerly.Domain;
using Jewerly.Web.Controllers;

namespace Jewerly.Web.Areas.Admin.Controllers
{
    public class CategoryPicturesController : BaseController
    {
        #region Helpers

        private int PhotoGalleryWidth = 500;
        private int PhotoGalleryHeight = 320;
        private string TempFolder { get; set; }
        private string UrlToLocal(string url)
        {
            return HttpContext.Server.MapPath(url);
        }

        private string SavaPreImage(HttpPostedFileBase file)
        {
            string ext = Path.GetExtension(file.FileName);
            if (IsImage(ext))
            {
                 var img = new WebImage(file.InputStream);
                if (CheckSize(img))
                {
                    ReSizePreview(img);
                    string newName = Guid.NewGuid().ToString("D") + ext;
                    
                    try
                    {
                        img.Save(UrlToLocal(TempFolder) + "\\" + newName);
                        CleanUpTempFolder(UrlToLocal(TempFolder), 1);
                        return "/Content/images/Temp" + "/" + newName;
                    }
                    catch (Exception)
                    {
                        throw new ArgumentException("Произошла ошибка при попытке загрузки изображени.");
                    }
                }
                else
                {
                    throw new ArgumentException("Малый размер изображения.");
                }
            }
            else
            {
                throw  new ArgumentException("Файл не является изображением.");
            }
                

        }
        private void CleanUpTempFolder(string tempFolder,int hoursOld)
        {
            DateTime fileCreationTime;
            DateTime currentUtcNow = DateTime.UtcNow;
            try
            {
                var serverPath = tempFolder;
                if (Directory.Exists(serverPath))
                {
                    string[] fileEntries = Directory.GetFiles(serverPath);
                    foreach (var fileEntry in fileEntries)
                    {
                        fileCreationTime = System.IO.File.GetCreationTimeUtc(fileEntry);
                        var res = currentUtcNow - fileCreationTime;
                        if (res.TotalHours > hoursOld)
                        {
                            System.IO.File.Delete(fileEntry);
                        }
                    }
                }
            }
            catch
            {
            }
        }
        private void ReSizePreview(WebImage img)
        {
            double ratio;
            if (img.Width > img.Height && img.Width > PhotoGalleryWidth)
            {
                ratio = (double)img.Height / (double)img.Width;
                int nWidth = (int)(PhotoGalleryHeight / ratio);
                img.Resize(nWidth, PhotoGalleryHeight);
            }          
        }
        private bool IsImage(string ext)
        {
            var extensions = new string[] { ".jpg", ".png", ".gif", ".jpeg", ".JPG", ".JPEG" }; // допустимые форматы.
            return extensions.Any(p => p == ext);
        }
        private  bool CheckSize(WebImage img)
        {
            return img.Width >= PhotoGalleryWidth && img.Height >= PhotoGalleryHeight;
        }

        #endregion
        
        #region Ctor

        public CategoryPicturesController(DataManager dataManager)
            : base(dataManager)
        {
            TempFolder = "~/Content/images/Temp";
        }

        #endregion

        #region Actions


        // GET: Admin/CategoryPictures
        public ActionResult Index()
        {
            return View(DataManager.CategoryPictures.GetAll().ToList());
        }

       // GET: Admin/CategoryPictures/Create
        public ActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Path,Caption,AltAttribute,TitleAttribute")] CategoryPicture categoryPicture)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    categoryPicture.Caption = categoryPicture.Caption.Trim();
                    var oldPath = UrlToLocal(categoryPicture.Path);                   
                    string imgName = Path.GetFileName(oldPath);
                    var newPath = UrlToLocal("~\\Content\\images\\category\\" + imgName);
                    System.IO.File.Move(oldPath, newPath);

                    categoryPicture.Path = "/Content/images/category/" + imgName;
                    DataManager.CategoryPictures.Insert(categoryPicture);
                    TempData["message"] = string.Format("Изображение \"{0}\" было сохранено", categoryPicture.Caption);
                    return RedirectToAction("Index");
                }
                catch (Exception )
                {
                    ModelState.AddModelError(string.Empty, "Произошла ошибка при попытке загрузки изображени.");
                }
                        
            }

            return View(categoryPicture);
        }

       
 

        [HttpPost]
        public JsonResult UploadImage(HttpPostedFileWrapper qqfile)
        {
          try
            {
                var url = SavaPreImage(qqfile);
                return
                    Json(
                        new
                        {
                            success = true,
                            errorMessage = string.Empty,
                            url = url
                        });
            }
            catch (Exception e)
            {
                return Json(new { success = false, errorMessage = e.Message });
            }
        }

      
        // GET: Admin/CategoryPictures/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoryPicture categoryPicture = DataManager.CategoryPictures.GetById(id.Value);
            if (categoryPicture == null)
            {
                return HttpNotFound();
            }
            return View(categoryPicture);
        }

        // POST: Admin/CategoryPictures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CategoryPicture categoryPicture = DataManager.CategoryPictures.GetById(id);
            DataManager.CategoryPictures. Delete(categoryPicture);


            TempData["message"] = string.Format("Изображение категории \"{0}\" было удалено", categoryPicture.Caption);
            return RedirectToAction("Index");
        }

        #endregion

    }
}
