using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Jewerly.Domain;
using Jewerly.Web.Controllers;
using Jewerly.Web.Utils;
using Microsoft.Owin.Security;

namespace Jewerly.Web.Areas.Admin.Controllers
{
    public class PicturesController : BaseController
    {

        #region Helpers

        private int PictureWidth = 640;
        private int PictureHeight = 800;

        private int TrumbPictureWidth = 200;
        private int TrumbPictureHeight = 250;

        private int PrePictureWidth = 1200;
        private int PrePictureHeight = 800;

        private string PathToPHotoFolder { get; set; }
        private string PathToWaterMark { get; set; }
        private string TempFolder { get; set; }
        private string UrlToLocal(string url)
        {
            return HttpContext.Server.MapPath(url);
        }

        private void ReSizePreview(WebImage img)
        {
            double ratio;
            if (img.Width > img.Height && img.Width > PrePictureWidth)
            {
                ratio = (double) img.Height/(double) img.Width;
                int nWidth = (int) (PrePictureHeight/ratio);
                img.Resize(nWidth, PrePictureHeight);
            }
        }

        private bool IsImage(string ext)
        {
            var extensions = new string[] {".jpg", ".png", ".gif", ".jpeg", ".JPG", ".JPEG"}; // допустимые форматы.
            return extensions.Any(p => p == ext);
        }

        private bool CheckSize(WebImage img)
        {
            return img.Width >= PrePictureWidth && img.Height >= PrePictureHeight;
        }

        private void CleanUpTempFolder(string tempFolder, int hoursOld)
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
                throw new ArgumentException("Файл не является изображением.");
            }


        }

        private WebImage CropImage(int top, int left, int height, int width, string pathToImage)
        {
            var img = new WebImage(pathToImage); // наше изображение
            img.Resize(width, height); // уменьшим его 
            /********************************************************************************/
            int nHeight = img.Height - top - PictureHeight; // подготовим ширину и высоту
            int nWidth = img.Width - left - PictureWidth;
            if (nHeight < 0) nHeight = 0;
            /****************** Обрежим изображение, по выбранным кординатам ***************************************************/
            img.Crop(top, left, nHeight, nWidth);
            return img;
        }

        private void AddWatermark(WebImage img, string pathToWatermark, int widthWatermark, int heightWatermark)
        {
            img.AddImageWatermark(pathToWatermark, widthWatermark, heightWatermark, "Center", "Middle", 20);
        }





        #endregion

        #region Ctor

        public PicturesController(DataManager dataManager)
            : base(dataManager)
        {
            TempFolder = "~/Content/images/Temp";
            PathToPHotoFolder = "~/Content/images/gallery";
            PathToWaterMark = "~/Content/img/WMwhite.png";
        }

        #endregion

        #region Actions

        public ActionResult Index(int page = 1)
        {

            IQueryable<Picture> pictures = DataManager.Pictures.GetAll().OrderBy(t => t.Caption);

            var count = pictures.Count();
            int countItemOnpage = 8;

            pictures = pictures.Skip((page - 1) * countItemOnpage)
                .Take(countItemOnpage);
            
            ViewBag.PageNo = page;
            ViewBag.CountPage = (int)decimal.Remainder(count, countItemOnpage) == 0
                ? count / countItemOnpage
                : count / countItemOnpage + 1;
            return View(pictures.ToList());
        }
        public ActionResult Create()
        {
            return View();
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Picture picture = DataManager.Pictures.GetById(id.Value);
            if (picture == null)
            {
                return HttpNotFound();
            }
            return View(picture);
        }

        // POST: Admin/Pictures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Picture picture = DataManager.Pictures.GetById(id);

            if (DataManager.Products.SearchFor(t => t.PictureId == id).Count() != 0)
            {
                TempData["error"] = "Произошла ошибка при удалении. Обнаружены товары с этим изображением.";
                return RedirectToAction("Delete", new { id = id });
            }
            try
            {
              System.IO.File.Delete(UrlToLocal(picture.Preview()));
              System.IO.File.Delete(UrlToLocal(picture.Image()));
              DataManager.Pictures.Delete(picture);
            }
            catch (Exception)
            {
                TempData["error"] = "Произошла ошибка при удалении.";
                return RedirectToAction("Delete", new { id = id }); 
            }
            TempData["message"] = string.Format("Изображение  \"{0}\" было удалено", picture.Caption);
            return RedirectToAction("Index");
        }

        #endregion

        [HttpPost]
        public JsonResult UploadPreImage(HttpPostedFileWrapper qqfile)
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

        [HttpPost]
        public ActionResult Save(string t, string l, string h, string w, string url, string caption)
        {
            if ((!string.IsNullOrEmpty(url)) && (!string.IsNullOrEmpty(caption)))
            {

                url = url.Trim();
                caption = caption.Trim();
               
                
                int top = Convert.ToInt32(t.Replace("-", "").Replace("px", ""));
                int left = Convert.ToInt32(l.Replace("-", "").Replace("px", ""));
                int height = Convert.ToInt32(h.Replace("-", "").Replace("px", ""));
                int width = Convert.ToInt32(w.Replace("-", "").Replace("px", ""));

                try 
                {
                    var img= CropImage(top, left, height, width, UrlToLocal(url));
                    AddWatermark(img,UrlToLocal(PathToWaterMark), 600, 200);

                    var imgName = Path.GetFileName(img.FileName);

                    img.Save(UrlToLocal(PathToPHotoFolder) + @"\image\" + imgName);
                    img.Resize(TrumbPictureWidth, TrumbPictureHeight);
                    img.Save(UrlToLocal(PathToPHotoFolder) + @"\preview\" + imgName);

                    System.IO.File.Delete(UrlToLocal(url));

                    var pic = new Picture
                    {
                        Path = imgName,
                        Caption = caption,
                       
                    };
                    
                    DataManager.Pictures.Insert(pic);
                    TempData["message"] = string.Format("Изображение \"{0}\" было сохранено", caption);
                    return Json(new { success = true, errorMessage = string.Empty });
                }
                catch (Exception e)
                {
                    return Json(new { success = false, errorMessage = e.Message });
                }
            }
            return Json(new { success = false, errorMessage = "Произошла ошибка при попытке сохранения изображения." });



        }


    }
}
