using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Xml.Linq;
using Jewerly.Domain;
using Jewerly.Web.Areas.Default.Models;
using Jewerly.Web.Utils;

namespace Jewerly.Web.Controllers
{
    public class SiteController : BaseController

    {
      [OutputCache(Duration = 86400, Location = System.Web.UI.OutputCacheLocation.Any)]
        public ActionResult RobotsText()
        {
            var content = new StringBuilder("User-agent: *" + Environment.NewLine);
            content.Append("Disallow: " + Environment.NewLine);
            content.Append("Disallow: ").Append("/Account" + Environment.NewLine);
            content.Append("Disallow: ").Append("/Error" + Environment.NewLine);
            content.Append("Disallow: ").Append("/signalr" + Environment.NewLine);
            content.Append("Disallow: ").Append("/Admin" + Environment.NewLine);
            
            content.Append("Sitemap: ").Append(FullyQualifiedApplicationPath(HttpContext)).Append("/sitemap.xml" + Environment.NewLine);
            return File(
                    Encoding.UTF8.GetBytes(content.ToString()),
                    "text/plain");
        }

      [HttpGet]
      [OutputCache(Duration = 24 * 60 * 60, Location = System.Web.UI.OutputCacheLocation.Any)]
      public ActionResult SitemapXml()
      {
          var content = GetSitemapXml();
          return Content(content, "application/xml", Encoding.UTF8);
      }

        private const string SitemapsNamespace = "http://www.sitemaps.org/schemas/sitemap/0.9";

      [NonAction]
      private IEnumerable<SitemapNode> GetSitemapNodes()
      {
          List<SitemapNode> nodes = new List<SitemapNode>();
          
          string baseUrl = FullyQualifiedApplicationPath(HttpContext);

          nodes.Add(new SitemapNode(baseUrl, 1.0));
          nodes.Add(new SitemapNode(baseUrl + Url.Action("Index", "About", null), 0.8));
          nodes.Add(new SitemapNode(baseUrl + Url.Action("Delivery", "About", null), 0.8));
          nodes.Add(new SitemapNode(baseUrl + Url.Action("Index", "Reviews", null), 0.8));
          nodes.Add(new SitemapNode(baseUrl + Url.Action("Contacts", "About", null), 0.8));
          
          var  storeHelper = new StoreHelper(DataManager);
          var categories =  storeHelper.GetListMenuCategories();
          foreach (var c in categories)
          {
              nodes.Add(new SitemapNode(baseUrl + Url.Action("Index", "Store", new { id = c.Id, name = c.SeoName }, null), 0.8));
              nodes.AddRange(c.SubCategories.Select(s => new SitemapNode(baseUrl + Url.Action("Index", "Store", new { id = s.Id, name = s.SeoName }, null))
              {
                  Priority = 0.5,
              }));
          }
          return nodes;
      }


      [NonAction]
      private string GetSitemapXml()
      {

          XElement root;
          XNamespace xmlns = XNamespace.Get(SitemapsNamespace);
          XNamespace xsi = XNamespace.Get("http://www.w3.org/2001/XMLSchema-instance");
          XNamespace schemaLocation = XNamespace.Get(SitemapsNamespace);

          root = new XElement(xmlns + "urlset",
                  new XAttribute(XNamespace.Xmlns + "xsi", xsi),
                  new XAttribute(xsi + "schemaLocation", schemaLocation)              
          );

          //XElement root;
          //XNamespace xmlns = SitemapsNamespace;

          var nodes = GetSitemapNodes();

          //root = new XElement(xmlns + "urlset",
          //     //new XAttribute(XNamespace.Xmlns + "xsi", xmlns.NamespaceName),
          //     new XAttribute(xmlns + "schemaLocation", SitemapsNamespace)
          //    );

          foreach (var node in nodes)
          {
              root.Add(
              new XElement(xmlns + "url",
                  new XElement(xmlns + "loc", Uri.EscapeUriString(node.Url)),
                  node.Priority == null ? null : new XElement(xmlns + "priority", node.Priority.Value.ToString("F1", CultureInfo.InvariantCulture))
                  ));
          }

          using (var ms = new MemoryStream())
          {
              using (var writer = new StreamWriter(ms, Encoding.UTF8))
              {
                  root.Save(writer);
              }

              return Encoding.UTF8.GetString(ms.ToArray());
          }
      }





        public SiteController(DataManager dataManager) : base(dataManager)
        {
        }
    }
}