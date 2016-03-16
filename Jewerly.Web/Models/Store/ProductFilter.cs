using System.Collections.Generic;
using System.Linq;

namespace Jewerly.Web.Models
{
    public class ProductFilter
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SeoName { get; set; }
        public int CurrentOptionId { get; set; }
        public List<ProductFilterOption> Options { get; set; }

        public ProductFilterOption CurrentOption
        {
            get
            {
                if (CurrentOptionId == 0) return null;
                var option = Options.SingleOrDefault(t => t.Id == CurrentOptionId);
                return option;
            }
        }

    }
    public class ProductFilterOption
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}