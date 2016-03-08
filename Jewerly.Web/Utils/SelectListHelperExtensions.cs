using System.Collections.Generic;
using System.Web.Mvc;

namespace Jewerly.Web.Utils
{
    public static class SelectListHelperExtensions
    {
        public static SelectList PreAppend(this SelectList list, string dataTextField, string selectedValue, bool selected = false)
        {
            var items = new List<SelectListItem>();
            string value = "";
            if (selected == false)
            {
                value = list.SelectedValue.ToString();
            }
            else
            {
                value = selectedValue;
            }
            items.Add(new SelectListItem() { Selected = selected, Text = dataTextField, Value = selectedValue });
            items.AddRange(list);

            return new SelectList(items, "Value", "Text", value);
        }

        public static SelectList Default(this SelectList list, string DataTextField, string SelectedValue)
        {
            return list.PreAppend(DataTextField, SelectedValue, true);
        }
    }

  

}