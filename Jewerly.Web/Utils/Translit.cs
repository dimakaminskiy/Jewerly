using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Jewerly.Web.Utils
{
    public static class Translit
    {
        public static string TranslitText(this string text)
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
            char f = builder[0];
            builder.Remove(0, 1);
            return char.ToUpper(f) + builder.ToString();

        }
    }
}


  

