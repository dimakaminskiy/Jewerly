using System.Collections.Generic;
using System.Linq;

namespace Jewerly.Web.Models
{
    public class CategoryModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SeoName { get; set; }
        public int? ParentCategoryId { get; set; }
        public List<CategoryModel> SubCategories { get; set; }
    }

    public class MenuCategories
    {
        public CategoryModel TopCategory { get; private set; }
        public CategoryModel CurrentCategory { get; private set; }

        public MenuCategories(int? curentCategoryId, List<CategoryModel> categories)
        {
            Categories = categories;
            if (curentCategoryId == null)
            {
                TopCategory = null;
                CurrentCategory = null;
            }
            else
            {
                Initialize(curentCategoryId.Value);
            }
           
        }

        public List<CategoryModel> Categories { get; private set; }

        private void Initialize(int id)
        {
            var cat = Categories.SingleOrDefault(t => t.Id == id);
            if (cat == null)
            {
                foreach (var category in Categories)
                {
                    cat = category.SubCategories.SingleOrDefault(t => t.Id == id);
                    if (cat!=null) break;
                }

                if (cat == null)
                {
                     TopCategory = null;
                     CurrentCategory = null;
                }

                else
                {
                    CurrentCategory = cat;
                    var top = Categories.SingleOrDefault(t => t.Id == cat.ParentCategoryId);
                    TopCategory = top ?? null;
                }
               
            }
            else 
            {
                CurrentCategory = cat;
                TopCategory = cat;
            }
           
        }
    }
}