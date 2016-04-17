using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Jewerly.Web.Areas.Default.Models
{
    public class SitemapNode
    {
        public string Url { get; set; }
        public double? Priority { get; set; }


        public SitemapNode(string url)
        {
            Url = url;
            Priority = null; 
        }
        public SitemapNode(string url, double priority)
        {
            Url = url;
            Priority = priority;
        }

    }

    
}