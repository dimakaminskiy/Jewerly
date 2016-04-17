//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.IO;
//using System.Linq;
//using System.Web;
//using System.Web.Helpers;
//using System.Web.Mvc;
//using System.Xml;
//using Jewerly.Domain;
//using Jewerly.Web.Areas.Default.Models.grab;
//using Jewerly.Web.Controllers;
//using Jewerly.Web.extensions;

//namespace Jewerly.Web.Areas.Default.Controllers
//{
//    public class TestController : BaseController
//    {
//        private string UrlToLocal(string url)
//        {
//            return HttpContext.Server.MapPath(url);
//        }

//        public string PathToPHotoFolder = "~/Content/images/gallery";
//        private int TrumbPictureWidth = 200;
//        private int TrumbPictureHeight = 250;
//        private void AddWatermark(WebImage img, string pathToWatermark, int widthWatermark, int heightWatermark)
//        {
//            img.AddImageWatermark(pathToWatermark, widthWatermark, heightWatermark, "Center", "Middle", 20);
//        }


//        OtherRepository repository = new OtherRepository();
//        void xmlToData()
//        {
//            XmlDocument xDoc = new XmlDocument();
//            xDoc.Load(UrlToLocal("/Content/xml.xml"));
//            XmlElement xRoot = xDoc.DocumentElement;

//            XmlNodeList materials = xRoot.SelectNodes("//shop//material");
//            XmlNodeList stones = xRoot.SelectNodes("//shop//stone");
//            XmlNodeList categories = xRoot.SelectNodes("//shop//category");
//            XmlNodeList length = xRoot.SelectNodes("//shop//length");
//            XmlNodeList offers = xRoot.SelectNodes("//shop//offers//offer");

//            var tcount = offers.Count;

//            if (materials != null)
//                foreach (XmlNode n in materials)
//                {
//                    var name = n.SelectSingleNode("@name").Value;
//                    var id = n.SelectSingleNode("@id").Value;
//                    repository.Materials.Add(new OtherMaterial()
//                    {
//                        Id = int.Parse(id),
//                        Name = name,
//                    });

//                }

//            if (stones != null)
//            {
//                foreach (XmlNode n in stones)
//                {
//                    var name = n.SelectSingleNode("@name").Value;
//                    var id = n.SelectSingleNode("@id").Value;
//                    var t = new OtherStone
//                    {
//                        Id = int.Parse(id),
//                        Name = name,
//                    };
//                    repository.addStones(t);
//                }
//            }
//            if (length != null)
//            {
//                foreach (XmlNode n in length)
//                {
//                    var name = n.SelectSingleNode("@name").Value;
//                    var id = n.SelectSingleNode("@id").Value;
//                    repository.Lenghts.Add(new OtherLenght()
//                    {
//                        Id = int.Parse(id),
//                        Name = name,
//                    });

//                }
//            }

//            if (categories != null)
//            {
//                foreach (XmlNode n in categories)
//                {
//                    var name = n.InnerText;
//                    var id = n.SelectSingleNode("@id").Value;
//                    var parent = n.SelectSingleNode("@parentId");
//                    var parentId = "";

//                    if (parent != null)
//                    {
//                        parentId = parent.Value;
//                    }


//                    repository.Categories.Add(new OtherCategory()
//                    {
//                        Id = int.Parse(id),
//                        Name = name,
//                        ParentId = parentId

//                    });

//                }
//            }

//            //if (offers != null)
//            //{
//            //    foreach (XmlNode offer in offers)
//            //    {

//            //        var price = "";
//            //        var category = "";
//            //        var picture = "";

//            //        var product = new OtherProduct();

//            //        foreach (XmlNode o in offer.ChildNodes)
//            //        {
//            //            if (o.Name == "offer_properties")
//            //            {
//            //                foreach (XmlNode c in o.ChildNodes)
//            //                {
//            //                    var propName = c.SelectSingleNode("@name");
//            //                    if (propName != null)
//            //                    {
//            //                        if (propName.Value == "Длина")
//            //                        {
//            //                            product.LenghtId = c.SelectSingleNode("@value").Value;
//            //                        }
//            //                        else if (propName.Value == "Материал")
//            //                        {
//            //                            product.MaterialId = c.SelectSingleNode("@value").Value;
//            //                        }
//            //                        else if (propName.Value == "Покрытие")
//            //                        {
//            //                            product.CoverId = c.SelectSingleNode("@value").Value;
//            //                        }
//            //                        else if (propName.Value == "Камень")
//            //                        {
//            //                            product.StoneId = c.SelectSingleNode("@value").Value;
//            //                        }
//            //                        else if (propName.Value == "Диаметр")
//            //                        {
//            //                            product.DiametrId = c.SelectSingleNode("@value").Value;
//            //                        }
//            //                    }

//            //                }
//            //            }


//            //            if (o.Name == "price_opt")
//            //            {
//            //                product.Price = o.InnerText;
//            //            }
//            //            if (o.Name == "categoryID")
//            //            {
//            //                product.CategoryId = o.InnerText;
//            //            }
//            //            if (o.Name == "picture")
//            //            {
//            //                product.Picture = o.InnerText;
//            //            }

//            //            repository.Products.Add(product);
//            //        }

//            //    }
//            //}


//            var list = new List<OtherProduct>();
//            if (offers != null)
//            {
//                foreach (XmlNode offer in offers)
//                {
//                    var price = "";
//                    var category = "";
//                    var picture = "";
//                    var product = new OtherProduct();
//                    //product.Price = price;
//                    //product.CategoryId = category;
//                    //product.Picture = picture;
//                  //  list.Add(product);

//                    foreach (XmlNode o in offer.ChildNodes)
//                    {
//                        if (o.Name == "offer_properties")
//                        {
//                            foreach (XmlNode c in o.ChildNodes)
//                            {
//                                var propName = c.SelectSingleNode("@name");
//                                if (propName != null)
//                                {
//                                    if (propName.Value == "Длина")
//                                    {
//                                        product.LenghtId = c.SelectSingleNode("@value").Value;
//                                    }
//                                    else if (propName.Value == "Материал")
//                                    {
//                                        product.MaterialId = c.SelectSingleNode("@value").Value;
//                                    }
//                                    else if (propName.Value == "Покрытие")
//                                    {
//                                        product.CoverId = c.SelectSingleNode("@value").Value;
//                                    }
//                                    else if (propName.Value == "Камень")
//                                    {
//                                        product.StoneId = c.SelectSingleNode("@value").Value;
//                                    }
//                                    else if (propName.Value == "Диаметр")
//                                    {
//                                        product.DiametrId = c.SelectSingleNode("@value").Value;
//                                    }
//                                }

//                            }
//                        }

//                        if (o.Name == "price_opt")
//                        {
//                            product.Price = o.InnerText;
//                        }
//                        if (o.Name == "categoryID")
//                        {
//                            product.CategoryId = o.InnerText;
//                        }
//                        if (o.Name == "picture")
//                        {
//                            product.Picture = o.InnerText;
//                        }

//                    }

//                    repository.Products.Add(product);
//                }
//            }

//            var ttt = repository.Products.Count();

//        }


//        void xtractoldattt()
//        {

//            //var markups = DataManager.Markups.GetAll().ToList();
//            //var tttt = DataManager.Categories.GetAll().ToList();

//            //foreach (var m in markups)
//            //{
//            //    DataManager.Markups.Delete(m);
//            //}

//            //foreach (var category in tttt)
//            //{
//            //    var m = new Markup()
//            //    {
//            //        Name = category.Name,
//            //        Retail = 200,
//            //        Trade = 100
//            //    };
//            //    DataManager.Markups.Insert(m);
//            //}

//            //foreach (var diametr in repository.Diametrs)
//            //{
//            //    var option = new SpecificationAttributeOption
//            //    {
//            //        DisplayOrder = 1,
//            //        Name = diametr.Name,
//            //        ProductSpecificationAttributeId = attrDiametr.ProductSpecificationAttributeId
//            //    };
//            //    DataManager.SpecificationAttributeOptions.Insert(option);
//            //}

//            //foreach (var lenght in repository.Lenghts)
//            //{
//            //    var option = new SpecificationAttributeOption
//            //    {
//            //        DisplayOrder = 1,
//            //        Name = lenght.Name,
//            //        ProductSpecificationAttributeId = attrDlina.ProductSpecificationAttributeId
//            //    };
//            //    DataManager.SpecificationAttributeOptions.Insert(option);
//            //}

//            //foreach (var cover in repository.Covers)
//            //{
//            //    var option = new SpecificationAttributeOption
//            //    {
//            //        DisplayOrder = 1,
//            //        Name = cover.Name,
//            //        ProductSpecificationAttributeId = attrCover.ProductSpecificationAttributeId
//            //    };
//            //    DataManager.SpecificationAttributeOptions.Insert(option);
//            //}
//            //foreach (var stone in repository.Stones)
//            //{
//            //    var option = new SpecificationAttributeOption
//            //    {
//            //        DisplayOrder = 1,
//            //        Name = stone.Name,
//            //        ProductSpecificationAttributeId = attrStone.ProductSpecificationAttributeId
//            //    };
//            //    DataManager.SpecificationAttributeOptions.Insert(option);
//            //}

//            //foreach (var material in repository.Materials)
//            //{
//            //    var option = new SpecificationAttributeOption
//            //    {
//            //        DisplayOrder = 1,
//            //        Name = material.Name,
//            //        ProductSpecificationAttributeId = attrMaterial.ProductSpecificationAttributeId
//            //    };
//            //    DataManager.SpecificationAttributeOptions.Insert(option);
//            //}
//        }


//          List<ExportPrroduct> productToexport()
//        {
//            var extacter = new Extracter(repository);
//            List<ExportPrroduct> ext = new List<ExportPrroduct>();

//            foreach (var p in repository.Products)
//            {
//                var t = extacter.OtherProductToExportProduct(p);
//                ext.Add(t);
//            }
//              return ext;

//        }

//        // GET: Default/Test
//        public ActionResult Index()
//        {
//            //xmlToData();
//            //List<ExportPrroduct> ext = productToexport();

//            //var list = ext.Select(t => t.Picture);
//            //var k= "kkk";

//            //var attrDiametr =
//            //    DataManager.ProductSpecificationAttributes.SearchFor(t => t.Name == "Диаметр")
//            //        .Include(t => t.SpecificationAttributeOptions)
//            //        .First();
//            //var attrDlina = DataManager.ProductSpecificationAttributes.SearchFor(t => t.Name == "Длина").Include(t => t.SpecificationAttributeOptions).First();
//            //var attrCover = DataManager.ProductSpecificationAttributes.SearchFor(t => t.Name == "Покрытие").Include(t => t.SpecificationAttributeOptions).First();
//            //var attrStone = DataManager.ProductSpecificationAttributes.SearchFor(t => t.Name == "Камень").Include(t => t.SpecificationAttributeOptions).First();
//            //var attrMaterial = DataManager.ProductSpecificationAttributes.SearchFor(t => t.Name == "Материал").Include(t => t.SpecificationAttributeOptions).First();

//            //var pathToTempFolder = UrlToLocal("/Content/tmpImage");


//            //foreach (var e in ext)
//            //{
//            //    var catId = DataManager.Categories.SearchFor(t => t.Name == e.CategoryName).SingleOrDefault();
//            //    int? coverid = null;
//            //    int? dlinaId = null;
//            //    int? diametrId = null;
//            //    int? materialId = null;
//            //    int? stoneId = null;

//            //    decimal price = e.Price;

//            //    if (catId == null)
//            //    {
//            //        continue;
//            //    }

//            //    var markup = DataManager.Markups.SearchFor(t => t.Name == catId.Name).SingleOrDefault();
//            //    if (markup == null)
//            //    {
//            //        continue;
//            //    }

//            //    Picture picture = null;
//            //    var locpath = pathToTempFolder + "\\" + e.Picture;

//            //    if (System.IO.File.Exists(locpath))
//            //    {
//            //        // var f = System.IO.File.OpenRead(locpath);
//            //        var img = new WebImage(locpath);

//            //        int PictureWidth = 640;
//            //        int PictureHeight = 800;

//            //        double ratio;
//            //        if (img.Width > img.Height && img.Width > PictureWidth)
//            //        {
//            //            ratio = (double)img.Height / (double)img.Width;
//            //            int nWidth = (int)(PictureHeight / ratio);
//            //            img.Resize(nWidth, PictureHeight);
//            //        }

//            //        AddWatermark(img, UrlToLocal("~/Content/img/WMwhite.png"), 600, 200);

//            //        // string extention = Path.GetExtension(e.Picture);
//            //        //string newName = Guid.NewGuid().ToString("D") + extention;
//            //        //string imgName = newName;

//            //        img.Save(UrlToLocal(PathToPHotoFolder) + @"\image\" + e.Picture);
//            //        img.Resize(TrumbPictureWidth, TrumbPictureHeight);
//            //        img.Save(UrlToLocal(PathToPHotoFolder) + @"\preview\" + e.Picture);
//            //        //f.Close();
//            //        picture = new Picture
//            //        {
//            //            Path = e.Picture,
//            //            Caption = e.Picture

//            //        };
//            //    }

//            //    if (picture == null)
//            //    {
//            //        continue;
//            //    }


//            //    if (!string.IsNullOrEmpty(e.CoverName))
//            //    {

//            //        var o = attrCover.SpecificationAttributeOptions.SingleOrDefault(t => t.Name == e.CoverName);
//            //        if (o != null)
//            //        {
//            //            coverid = o.SpecificationAttributeOptionId;
//            //        }
//            //    }
//            //    if (!string.IsNullOrEmpty(e.DiametrName))
//            //    {

//            //        var o = attrDiametr.SpecificationAttributeOptions.SingleOrDefault(t => t.Name == e.DiametrName);
//            //        if (o != null)
//            //        {
//            //            diametrId = o.SpecificationAttributeOptionId;
//            //        }
//            //    }

//            //    if (!string.IsNullOrEmpty(e.LenghtName))
//            //    {

//            //        var o = attrDlina.SpecificationAttributeOptions.SingleOrDefault(t => t.Name == e.LenghtName);
//            //        if (o != null)
//            //        {
//            //            dlinaId = o.SpecificationAttributeOptionId;
//            //        }
//            //    }

//            //    if (!string.IsNullOrEmpty(e.MaterialName))
//            //    {

//            //        var o = attrMaterial.SpecificationAttributeOptions.SingleOrDefault(t => t.Name == e.MaterialName);
//            //        if (o != null)
//            //        {
//            //            materialId = o.SpecificationAttributeOptionId;
//            //        }
//            //    }

//            //    if (!string.IsNullOrEmpty(e.StoneName))
//            //    {

//            //        var o = attrStone.SpecificationAttributeOptions.SingleOrDefault(t => t.Name == e.StoneName);
//            //        if (o != null)
//            //        {
//            //            stoneId = o.SpecificationAttributeOptionId;
//            //        }
//            //    }


//            //    DataManager.Pictures.Insert(picture);

//            //    var product = new Product
//            //    {
//            //        CategoryId = catId.Id,
//            //        MarkupId = markup.Id,
//            //        PictureId = picture.Id,
//            //        Name = catId.ProductName,
//            //        SeoName = catId.ProductName.ToTranslit(),
//            //        Published = true,
//            //        Price = price,

//            //    };

//            //    DataManager.Products.Insert(product);


//            //    if (coverid != null)
//            //    {
//            //        DataManager.MappingProductSpecificationAttributeToProducts.Insert(new MappingProductSpecificationAttributeToProduct
//            //        {
//            //            ProductId = product.ProductId,
//            //            ProductSpecificationAttributeId = attrCover.ProductSpecificationAttributeId,
//            //            SpecificationAttributeOptionId = coverid.Value
//            //        });
//            //    }
//            //    if (dlinaId != null)
//            //    {
//            //        DataManager.MappingProductSpecificationAttributeToProducts.Insert(new MappingProductSpecificationAttributeToProduct
//            //        {
//            //            ProductId = product.ProductId,
//            //            ProductSpecificationAttributeId = attrDlina.ProductSpecificationAttributeId,
//            //            SpecificationAttributeOptionId = dlinaId.Value
//            //        });
//            //    }
//            //    if (diametrId != null)
//            //    {
//            //        DataManager.MappingProductSpecificationAttributeToProducts.Insert(new MappingProductSpecificationAttributeToProduct
//            //        {
//            //            ProductId = product.ProductId,
//            //            ProductSpecificationAttributeId = attrDiametr.ProductSpecificationAttributeId,
//            //            SpecificationAttributeOptionId = diametrId.Value
//            //        });
//            //    }
//            //    if (materialId != null)
//            //    {
//            //        DataManager.MappingProductSpecificationAttributeToProducts.Insert(new MappingProductSpecificationAttributeToProduct
//            //        {
//            //            ProductId = product.ProductId,
//            //            ProductSpecificationAttributeId = attrMaterial.ProductSpecificationAttributeId,
//            //            SpecificationAttributeOptionId = materialId.Value
//            //        });
//            //    }
//            //    if (stoneId != null)
//            //    {
//            //        DataManager.MappingProductSpecificationAttributeToProducts.Insert(new MappingProductSpecificationAttributeToProduct
//            //        {
//            //            ProductId = product.ProductId,
//            //            ProductSpecificationAttributeId = attrStone.ProductSpecificationAttributeId,
//            //            SpecificationAttributeOptionId = stoneId.Value
//            //        });
//            //    }





//            //}

//            return View();
//        }

//        public TestController(DataManager dataManager) : base(dataManager)
//        {
//        }
//    }
//}