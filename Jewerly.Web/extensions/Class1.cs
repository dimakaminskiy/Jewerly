using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Jewerly.Web.extensions
{
    public static class TranslitExtension
    {
        public static string ToTranslit(this string text)
        {
            string str = text.Replace(" ", "-").ToLower();

            Regex pattern = new Regex("[.,:&ьъ!@\"\'()]");
            str = pattern.Replace(str, "");

            StringBuilder builder = new StringBuilder();
            builder.Append(str);

            
            string[] latLow =
            {
                "a", "b", "v", "g", "d", "e", "yo", "zh", "z", "i", "y", "k", "l", "m", "n", "o", "p",
                "r", "s", "t", "u", "f", "kh", "ts", "ch", "sh", "shch", "y",  "e", "yu", "ya"
            };
            string[] rusLow =
            {
                "а", "б", "в", "г", "д", "е", "ё", "ж", "з", "и", "й", "к", "л", "м", "н", "о", "п",
                "р","с", "т", "у", "ф", "х", "ц", "ч", "ш", "щ",  "ы", "э", "ю", "я"
            };
            for (int i = 0; i <= 30; i++)
            {
                builder.Replace(rusLow[i], latLow[i]);
            }
            return builder.ToString();
        }
    }

    public static class SelectListHelperExtensions
    {
        public static SelectList PreAppend(this SelectList list, string dataTextField, string selectedValue,
            bool selected = false)
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
            items.Add(new SelectListItem() {Selected = selected, Text = dataTextField, Value = selectedValue});
            items.AddRange(list);

            return new SelectList(items, "Value", "Text", value);
        }
    }

}