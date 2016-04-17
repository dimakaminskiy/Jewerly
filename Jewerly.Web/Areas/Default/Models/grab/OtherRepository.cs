using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jewerly.Web.Areas.Default.Models.grab
{


    class Extracter
    {
        public OtherRepository Repository { get; set; }

        public Extracter(OtherRepository repository)
        {
            Repository = repository;
        }

        public ExportPrroduct OtherProductToExportProduct(OtherProduct product)
        {
            var e = new ExportPrroduct();
            e.Price = decimal.Parse(product.Price);

            var category = Repository.Categories.Where(t => t.Id.ToString() == product.CategoryId).FirstOrDefault();

            if (category != null)
            {
                e.CategoryName = category.Name;
            }



            //  var category = Repository.Categories.Where(t => t.Id.ToString() == product.CategoryId).First().Name;
            //  e.CategoryName = category;
            e.Picture = product.Picture;

            if (product.CoverId != null && !string.IsNullOrEmpty(product.CoverId))
            {
                var cov = Repository.Covers.Where(t => t.Id.ToString() == product.CoverId).First().Name;
                e.CoverName = cov;
            }
            if (!string.IsNullOrEmpty(product.DiametrId) && product.DiametrId != "0")
            {
                var d = Repository.Diametrs.Where(t => t.Id.ToString() == product.DiametrId).First().Name;
                e.DiametrName = d;
            }
            if (product.MaterialId != null && !string.IsNullOrEmpty(product.MaterialId))
            {
                var m = Repository.Materials.Where(t => t.Id.ToString() == product.MaterialId).First().Name;
                e.MaterialName = m;
            }

            if (product.StoneId != null && !string.IsNullOrEmpty(product.StoneId))
            {
                var s = Repository.Stones.Where(t => t.Id.ToString() == product.StoneId).First().Name;
                e.StoneName = s;
            }
            if (!string.IsNullOrEmpty(product.LenghtId) && product.LenghtId != "0")
            {
                var l = Repository.Lenghts.Where(t => t.Id.ToString() == product.LenghtId).First().Name;
                e.LenghtName = l;
            }
            return e;
        }



    }




    public class ExportPrroduct
    {
        public string Picture { get; set; }
        public decimal Price { get; set; }
        public string CoverName { get; set; }
        public string LenghtName { get; set; }
        public string StoneName { get; set; }
        public string CategoryName { get; set; }
        public string DiametrName { get; set; }
        public string MaterialName { get; set; }
    }


    public class OtherStone
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class OtherCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ParentId { get; set; }
    }

    public class OtherLenght
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class OtherCover
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class OtherDiametr
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class OtherMaterial
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class OtherRepository
    {
        public List<OtherStone> Stones = new List<OtherStone>();
        public List<OtherCategory> Categories = new List<OtherCategory>();
        public List<OtherLenght> Lenghts = new List<OtherLenght>();
        public List<OtherDiametr> Diametrs = new List<OtherDiametr>();
        public List<OtherMaterial> Materials = new List<OtherMaterial>();
        public List<OtherProduct> Products = new List<OtherProduct>();
        public List<OtherCover> Covers = new List<OtherCover>();


        public void addStones(OtherStone stone)
        {
            if (Stones.All(t => t.Name != stone.Name))
            {
                Stones.Add(stone);
            }
        }




        public OtherRepository()
        {

            Covers.Add(new OtherCover()
            {
                Id = 16,
                Name = "гипоаллергенное"
            });

            Covers.Add(new OtherCover()
            {
                Id = 21,
                Name = "мельхиор"
            });

            Covers.Add(new OtherCover()
            {
                Id = 26,
                Name = "позолота"
            });

            Covers.Add(new OtherCover()
            {
                Id = 29,
                Name = "родий"
            });

            Covers.Add(new OtherCover()
            {
                Id = 33,
                Name = "серебро"
            });

            Covers.Add(new OtherCover()
            {
                Id = 35,
                Name = "серебро безоксидное"
            });

            Covers.Add(new OtherCover()
            {
                Id = 36,
                Name = "серебро оксидированное"
            });

            Covers.Add(new OtherCover()
            {
                Id = 37,
                Name = "серебро/золото"
            });

            Diametrs.Add(new OtherDiametr()
            {
                Id = 1,
                Name = "0-1"
            });

            Diametrs.Add(new OtherDiametr()
            {
                Id = 2,
                Name = "1-5"
            });

            Diametrs.Add(new OtherDiametr()
            {
                Id = 3,
                Name = "5-7"
            });

            Diametrs.Add(new OtherDiametr()
            {
                Id = 4,
                Name = "8-9"
            });

            Diametrs.Add(new OtherDiametr()
            {
                Id = 5,
                Name = "10-12"
            });

            Diametrs.Add(new OtherDiametr()
            {
                Id = 6,
                Name = "12-15"
            });

            Diametrs.Add(new OtherDiametr()
            {
                Id = 7,
                Name = "15-20"
            });

            Diametrs.Add(new OtherDiametr()
            {
                Id = 8,
                Name = "20++"
            });
        }


    }


    public class OtherProduct
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public string Price { get; set; }
        public string CoverId { get; set; }
        public string LenghtId { get; set; }
        public string StoneId { get; set; }
        public string CategoryId { get; set; }
        public string DiametrId { get; set; }
        public string MaterialId { get; set; }

    }


}