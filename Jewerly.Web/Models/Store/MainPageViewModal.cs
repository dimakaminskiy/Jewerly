using System.Collections.Generic;
using System.Linq;
using Jewerly.Domain;

namespace Jewerly.Web.Models
{
    public class MainPageViewModal
    {
        public List<Category> HomePageCategories { get; set; }
        public MenuCategories MenuCategories { get; set; }
    }

   
}