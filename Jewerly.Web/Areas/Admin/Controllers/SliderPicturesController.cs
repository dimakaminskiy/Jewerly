﻿using System;
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
    [Authorize(Roles = "Administrator")]
    public class SliderPicturesController : BaseController
    {

        #region Helpers

        private int PhotoGalleryWidth = 850;
        private int PhotoGalleryHeight = 480;
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
                throw new ArgumentException("Файл не является изображением.");
            }


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

        private void ReSizePreview(WebImage img)
        {
            double ratio;
            if (img.Width > img.Height && img.Width > PhotoGalleryWidth)
            {
                ratio = (double) img.Height/(double) img.Width;
                int nWidth = (int) (PhotoGalleryHeight/ratio);
                img.Resize(nWidth, PhotoGalleryHeight);
            }
        }

        private bool IsImage(string ext)
        {
            var extensions = new string[] {".jpg", ".png", ".gif", ".jpeg", ".JPG", ".JPEG"}; // допустимые форматы.
            return extensions.Any(p => p == ext);
        }

        private bool CheckSize(WebImage img)
        {
            return img.Width >= PhotoGalleryWidth && img.Height >= PhotoGalleryHeight;
        }

        #endregion

        #region Actions

        public ActionResult Index(int page = 1)
        {
            IQueryable<SliderPicture> pictures = DataManager.SliderPictures.GetAll().OrderBy(t => t.Caption);

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
                return Json(new {success = false, errorMessage = e.Message});
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Path,Caption,Text")] SliderPicture sliderPicture)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    sliderPicture.Caption = sliderPicture.Caption.Trim();
                    var oldPath = UrlToLocal(sliderPicture.Path);
                    string imgName = Path.GetFileName(oldPath);
                    var newPath = UrlToLocal("~/Content/images/slider/" + imgName);

                    var dir = UrlToLocal("~/Content/images/slider");
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                    System.IO.File.Move(oldPath, newPath);

                    sliderPicture.Path = "/Content/images/slider/" + imgName;
                    DataManager.SliderPictures.Insert(sliderPicture);
                    TempData["message"] = string.Format("Изображение \"{0}\" было сохранено", sliderPicture.Caption);
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    ModelState.AddModelError(string.Empty, "Произошла ошибка при попытке загрузки изображени.");
                }
                return RedirectToAction("Index");
            }

            return View(sliderPicture);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SliderPicture sliderPicture = DataManager.SliderPictures.GetById(id.Value);
            if (sliderPicture == null)
            {
                return HttpNotFound();
            }
            return View(sliderPicture);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Path,Caption,Text")] SliderPicture sliderPicture)
        {
            if (ModelState.IsValid)
            {
                DataManager.SliderPictures.Edit(sliderPicture);
                return RedirectToAction("Index");
            }
            return View(sliderPicture);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SliderPicture sliderPicture = DataManager.SliderPictures.GetById(id.Value);
            if (sliderPicture == null)
            {
                return HttpNotFound();
            }
            return View(sliderPicture);
        }

        // POST: Admin/SliderPictures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SliderPicture sliderPicture = DataManager.SliderPictures.GetById(id);
            try
            {
                DataManager.SliderPictures.Delete(sliderPicture);
                var path = UrlToLocal(sliderPicture.Path);
                System.IO.File.Delete(path);
                TempData["message"] = string.Format("Изображение  \"{0}\" было удалено", sliderPicture.Caption);
            }
            catch (Exception)
            {
                TempData["error"] = "При удалении возникла ошибка";
                return RedirectToAction("Delete", new { id = id });
            }
            return RedirectToAction("Index");
        }

        #endregion

        #region ctor

        public SliderPicturesController(DataManager dataManager) : base(dataManager)
        {
            TempFolder = "~/Content/images/Temp";
        }

        #endregion

    }
}
