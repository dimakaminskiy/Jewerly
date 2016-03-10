using System.Collections.Generic;
using System.Web.Mvc;
using Jewerly.Domain;

namespace Jewerly.Web.Models
{
    public class ProductSpecificationAttribiteViewModel
    {
        public int AttributeId { get; set; }
        public string Name { get; set; }
        public SelectList Options { get; set; } 
    }

    public class ProductChoiceAttributesViewModel
    {
        public int ProductId { get; set; }
        public int ProductChoiceAttributeId { get; set; }
        public List<ProductChoiceAttribute> ProductChoiceAttributes { get; set; }
        public List<ChoiceAttributeOptionViewModel> ChoiceAttributeOptions { get; set; }

    }

    public class ChoiceAttributeOptionViewModel : ChoiceAttributeOption
    {
        public ChoiceAttributeOptionViewModel()
        {
            Available = false;    
        }
        public bool Available { get; set; } 
    }


}