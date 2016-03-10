using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Jewerly.Web.Models
{
    public class ProductSortModel
    {
        public string Sort { get; private set; }

        public string SortByDefult
        {
           get { return "novelty";  }
        }
       
        public List<SortOption> SortOptions { get { return _sortOptions; } }

        public ProductSortModel(string sort)
        {
            if (!string.IsNullOrEmpty(sort) &&  _sortOptions.Any(t => t.Value == sort))
            {
                Sort = (sort == SortByDefult) ? "" : sort;
            }
            else
            {
                Sort = "";
            }
        }
        private static readonly List<SortOption> _sortOptions = new List<SortOption>()
        {
            new SortOption() {Name = "от дешевых к дорогим", Value = "cheap"},
            new SortOption() {Name = "от дорогих к дешевым", Value = "expensive"},
            new SortOption() {Name = "новинки", Value = "novelty"}
        };
       
        public class SortOption
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }

    }
}