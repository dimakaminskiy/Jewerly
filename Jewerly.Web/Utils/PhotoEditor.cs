using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace WebUaJewelry.Utils
{
    public class PhotoEditor
    {

        private static readonly ImageSize PhotoGallery = new ImageSize {Width = 640, Height = 800};
        private static readonly ImageSize PhotoGalleryPreView = new ImageSize {Width = 200, Height = 250};

        public bool IsImage(string ext)
        {
            var extensions = new string[] {".jpg", ".png", ".gif", ".jpeg", ".JPG", ".JPEG"}; // допустимые форматы.
            return extensions.Any(p => p == ext);
        }

        public static bool CheckSize(WebImage img)
        {
            return img.Width >= PhotoGallery.Width && img.Height >= PhotoGallery.Height;
        }


        public string SaveJevelPhot(int top, int left, int height, int width, string path)
        {
            //var pathToWaterMark = UrlToLocal("~\\Content\\images\\Watermart.png");

            //return HttpContext.Server.MapPath(url);
            FileInfo f = new FileInfo(path);
            var img = new WebImage(path); // наше изображение
            img.Resize(width, height); // уменьшим его 
            /********************************************************************************/
            int nHeight = img.Height - top - PhotoGallery.Height; // подготовим ширину и высоту
            int nWidth = img.Width - left - PhotoGallery.Width;
            if (nHeight < 0) nHeight = 0;
            /****************** Обрежим изображение, по выбранным кординатам ***************************************************/
            img.Crop(top, left, nHeight, nWidth);
            try
            {
             //   img.AddImageWatermark(PathToWaterMark, 600, 200, "Center", "Middle", 20);

             //   img.Save(JewelPHotoFolder + @"\big\" + f.Name);
                img.Resize(PhotoGalleryPreView.Width, PhotoGalleryPreView.Height);


             //   img.Save(JewelPHotoFolder + @"\small\" + f.Name);
                return f.Name;
            }
            catch (Exception)
            {
                throw new ArgumentException("Произошла ошибка  при попытке сохранения изображени.");
            }

        }



        public string SaveJevelPhotWithWatermark(string pathToGallery,int top, int left, int height, int width,
            string pathToImage, string pathToWatermark, int widthWatermark, int heightWatermark)
        {
            FileInfo f = new FileInfo(pathToImage);
            var img = new WebImage(pathToImage); // наше изображение
            img.Resize(width, height); // уменьшим его 
            /********************************************************************************/
            int nHeight = img.Height - top - PhotoGallery.Height; // подготовим ширину и высоту
            int nWidth = img.Width - left - PhotoGallery.Width;
            if (nHeight < 0) nHeight = 0;
            /****************** Обрежим изображение, по выбранным кординатам ***************************************************/
            img.Crop(top, left, nHeight, nWidth);
            try
            {
                img.AddImageWatermark(pathToWatermark, widthWatermark, heightWatermark, "Center", "Middle", 20);

                img.Save(pathToGallery + @"\image\" + f.Name);
                img.Resize(PhotoGalleryPreView.Width, PhotoGalleryPreView.Height);
                img.Save(pathToGallery + @"\preview\" + f.Name);
                return f.Name;
            }
            catch (Exception)
            {
                throw new ArgumentException("Произошла ошибка  при попытке сохранения изображени.");
            }

        }

        public string SaveTempFile(string tempFolder,  HttpPostedFileBase file)
        {
            try
            {
                if (Directory.Exists(tempFolder) == false)
                {
                    Directory.CreateDirectory(tempFolder);
                }
            }
            catch (Exception)
            {
                throw new Exception("Ошибка загрузки изображения");
            }
            string ext = Path.GetExtension(file.FileName);

            if (IsImage(ext))
            {
                string newName = Guid.NewGuid().ToString("D") + ext;

                var img = new WebImage(file.InputStream);
                if (CheckSize(img))
                {
                    try
                    {
                        img = ReSizePreview(img);
                        img.Save(tempFolder + "\\" + newName);
                        CleanUpTempFolder(tempFolder,1);
                        return newName;
                    }
                    catch (Exception)
                    {
                        throw new ArgumentException("Произошла ошибка при попытке загрузки изображени.");
                    }
                }
                throw new ArgumentException("Малый размер изображения.");
            }
            throw new ArgumentException("Файл недопустимого формата.");
        }


        private WebImage ReSizePreview(WebImage img)
        {
            int ImageHeight = 800;
            int ImageWidth = 1200;

            double ratio;


            if (img.Width > img.Height && img.Width > ImageWidth)
            {
                ratio = (double) img.Height/(double) img.Width;

                int nWidth = (int) (ImageHeight/ratio);

                //  img.Resize(ImageWidth, (int)(img.Height * ratio));

                img.Resize(nWidth, ImageHeight);
            }

            return img;
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

    }

    public class ImageSize
    {
        public int Width { get; set; }
        public int Height { get; set; }

    }
}