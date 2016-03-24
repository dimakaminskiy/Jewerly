using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jewerly.Web.Utils
{
    public class EmailSettings
    {
        public string Link = "trendycats.com.ua";
        public string MailFromAddress = "spartak84259@gmail.com";
        public string ServerName = "smtp.gmail.com";
        public bool UseSsl = true;
        public int ServerPort = 587; //465;
        public string password = "8425999mama";
    }
}