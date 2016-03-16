using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using Jewerly.Domain;
using Jewerly.Web.Models;


namespace Jewerly.Web.Utils
{
    public  static class HtmlMenuHeler
    {


        public static string ActionWithOldQuery(this UrlHelper helper, string controller, string action, object routeValues, string oldQuery)
        {
            RouteValueDictionary newRoute = new RouteValueDictionary(routeValues);

            var current = helper.RequestContext.RouteData.Values;

            // Merge the new values INTO the current values, overwriting any existing values/querystrings
            foreach (var item in newRoute)
                current[item.Key] = item.Value;

            string h =  helper.Action(action, controller)  + oldQuery;
            return h;
        }




        public static string RouteValueChange(this UrlHelper url, object routeValues)
        {
            // Convert new route values to a dictionary
            RouteValueDictionary newRoute = new RouteValueDictionary(routeValues);

            // Get the route data of the current Url
            var current = url.RequestContext.RouteData.Values;

            // Merge the new values INTO the current values, overwriting any existing values/querystrings
            foreach (var item in newRoute)
                current[item.Key] = item.Value;

            // Generate the new Url
           
            return url.RouteUrl(current);
        }


        public static MvcHtmlString GridCategories(this HtmlHelper html, List<Category> list)
        {
            StringBuilder builder = new StringBuilder();
            StringBuilder lineBuilder = new StringBuilder();
            var urlHelper = new UrlHelper(html.ViewContext.RequestContext);
            int c = 1;
            for (int i = 0; i < list.Count ; i++)
            {
                lineBuilder.AppendFormat(
                    "<div class=\"col-sm-6 text-center\"><img style=\"margin:auto;\" class=\"img-responsive img-thumbnail\" src=\"{0}\" alt=\"{1}\" title=\"{1}\">",
                    list[i].CategoryPicture.Path, list[i].Name);

                lineBuilder.AppendFormat("<a class=\"name_category hidden-xs\" href=\"{0}\"><span>{1}</span></a>", urlHelper.Action("Index", "Store", new {id = list[i].Id, name = list[i].SeoName}), list[i].Name);
                lineBuilder.AppendFormat("<a class=\"name_category_mini visible-xs\" href=\"{0}\"><span>{1}</span></a></div>", urlHelper.Action("Index", "Store", new { id = list[i].Id, name = list[i].SeoName }), list[i].Name);
                if (c % 2 == 0)
                {
                    lineBuilder.AppendLine("<div class=\"clearfix hidden-xs\"> </div>");
                }
                builder.AppendLine(lineBuilder.ToString());
                lineBuilder.Clear();
                c++;
            }
            if (list.Count % 2 != 0)
            {
                c = list.Count;
                while (c%2!=0)
                {
                    builder.AppendLine("<div class=\"col-sm-6\"></div>");
                    c++;
                }
            }
            builder.AppendLine("<div class=\"clearfix hidden-xs\"> </div>");
            return new MvcHtmlString(builder.ToString());


        }

        public static MvcHtmlString SortOptions(this HtmlHelper html, ProductSortModel model,
            string catId, string catName)
        {
             var urlHelper = new UrlHelper(html.ViewContext.RequestContext);
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("<ul class=\"dropdown-menu\">");
            StringBuilder lineBuilder= new StringBuilder();

            foreach (var option in model.SortOptions)
            {
               
                if (string.IsNullOrEmpty(model.Sort))
                {
                    lineBuilder.AppendFormat(
                            "<li><a {0} href=\"{1}\">{2}</a></li>",                          
                            option.Value == model.SortByDefult? "class=\"active\"":"",
                            urlHelper.Action("Index", "Store", new { id = catId, name = catName, sort = option.Value,page=1}),
                            option.Name);   
                 }
                else
                {
                    lineBuilder.AppendFormat(
                             "<li><a {0} href=\"{1}\">{2}</a></li>",
                             option.Value == model.Sort ? "class=\"active\"" : "",
                             urlHelper.Action("Index", "Store", new { id = catId, name = catName, sort = option.Value,page=1 }),
                             option.Name);    
                }

                builder.AppendLine(lineBuilder.ToString());
                lineBuilder.Clear();
            }
            builder.AppendLine("</ul>");
            return new MvcHtmlString(builder.ToString());
        }





        public static MvcHtmlString LiActionLink(this HtmlHelper html, string text, string action, string controller, string cssClass = "active")
        {
            var context = html.ViewContext;
            if (context.Controller.ControllerContext.IsChildAction)
                context = html.ViewContext.ParentActionViewContext;
            var routeValues = context.RouteData.Values;
            var currentAction = routeValues["action"].ToString();
            var currentController = routeValues["controller"].ToString();


            routeValues.Remove("name");
            routeValues.Remove("id");


            var li = currentAction.Equals(action, StringComparison.InvariantCulture) &&
                     currentController.Equals(controller, StringComparison.InvariantCulture)
                ? " class=\"" + cssClass + "\""
                : String.Empty;

             var urlHelper = new UrlHelper(html.ViewContext.RequestContext);
            var g = urlHelper.Action(action, controller);

            var link = html.ActionLink(text, action, controller, new object(), null).ToHtmlString();

            var str = String.Format("<li {0}>{1}</li>",li,link);
            return new MvcHtmlString(str);
        }

        public static MvcHtmlString MenuCategoriesLinks(this HtmlHelper html, MenuCategories menu)
        {
            var builder = new StringBuilder();

            var urlHelper = new UrlHelper(html.ViewContext.RequestContext);

            foreach (var category in menu.Categories)
            {
                builder.AppendLine("<li role='presentation'><span><a href=\"" 
                    + urlHelper.Action("Index","Store", new {id = category.Id, name = category.SeoName})+ "\">" + category.Name + "</a></span>");
                builder.AppendLine("<a class=\"menu-icon\" role=\"button\" data-toggle=\"collapse\" data-parent=\"#accordion\" href='#" +
                                   category.Id + "' aria-expanded=\"true\" aria-controls='" + category.Id + "'>" + "<span class='glyphicon glyphicon-resize-full'></span>" + "</a>");


                if (menu.TopCategory!=null && category.Id == menu.TopCategory.Id)
                {
                    builder.AppendLine("<div id='" + category.Id + "' class='panel-collapse collapse in' role='tabpanel' aria-labelledby='headingOne'>");      
                }
                else
                {
                    builder.AppendLine("<div id='" + category.Id + "' class='panel-collapse collapse' role='tabpanel' aria-labelledby='headingOne'>"); 
                }
               
                builder.AppendLine("<div class=\"panel-body\"> <ul class=\"nav nav-pills nav-stacked\">");


                foreach (var subCategory in category.SubCategories)
                {

                    if (menu.CurrentCategory!=null && subCategory.Id == menu.CurrentCategory.Id)
                    {
                        builder.AppendLine("<li class=\"active\">");
                    }
                    else
                    {
                        builder.AppendLine("<li >");
                    }

                    builder.AppendLine(
                        html.ActionLink(subCategory.Name, "Index", "Store",
                            new
                            {
                                name = subCategory.SeoName,
                                id = subCategory.Id
                            }, null).ToString());
                    builder.AppendLine("</li>");

                }


                builder.AppendLine("</ul></div>");

                builder.AppendLine("</div>");
                builder.AppendLine("</li>");
            }
           

            
        














        //public static MvcHtmlString MenuCategoriesLinks(this HtmlHelper html,MenuViewModel menu)
        //{
        //    var builder = new StringBuilder();

        //    foreach (var category in menu.Categories)
        //    {
        //        builder.AppendLine("<li role='presentation'>");
        //        builder.AppendLine("<a role=\"button\" data-toggle=\"collapse\" data-parent=\"#accordion\" href='#" +
        //                           category.Id + "' aria-expanded=\"true\" aria-controls='" + category.Id + "'>" + category.Name + "</a>");


        //        if (category.Id == menu.SelectCategory)
        //        {
        //            builder.AppendLine("<div id='" + category.Id + "' class='panel-collapse collapse in' role='tabpanel' aria-labelledby='headingOne'>");      
        //        }
        //        else
        //        {
        //            builder.AppendLine("<div id='" + category.Id + "' class='panel-collapse collapse' role='tabpanel' aria-labelledby='headingOne'>"); 
        //        }
              

        //        builder.AppendLine("<div class=\"panel-body\"> <ul class=\"nav nav-pills nav-stacked\">");


        //        foreach (var subCategory in category.SubCategories)
        //        {

        //            if (subCategory.Id == menu.SelectSubCategory)
        //            {
        //                builder.AppendLine("<li class=\"active\">");     
        //            }
        //            else
        //            {
        //                builder.AppendLine("<li >"); 
        //            }
                   
        //            builder.AppendLine(
        //                html.ActionLink(subCategory.Name, "Index", "Store", 
        //                    new
        //                    {
        //                        name = subCategory.TranslitName, id = subCategory.Id
        //                    },null).ToString());
        //            builder.AppendLine("</li>");

        //        }


        //        builder.AppendLine("</ul></div>");

        //        builder.AppendLine("</div>");
        //        builder.AppendLine("</li>");
        //    }
           
            return new MvcHtmlString(builder.ToString());
        }

        public static MvcHtmlString PageLinks(this HtmlHelper html, int currentPage, int totalPages,
       Func<int, string> pageUrl)
        {
            StringBuilder builder = new StringBuilder();

            //Prev
            var prevBuilder = new TagBuilder("a");
            prevBuilder.InnerHtml = "&laquo;"; //закрывает тег а.
            if (currentPage == 1)
            {
                prevBuilder.MergeAttribute("href", pageUrl.Invoke(currentPage));
                builder.AppendLine("<li class=\"active\">" + prevBuilder.ToString() + "</li>");
            }
            else
            {
                prevBuilder.MergeAttribute("href", pageUrl.Invoke(currentPage - 1));
                builder.AppendLine("<li>" + prevBuilder.ToString() + "</li>");
            }
            //По порядку
            for (int i = 1; i <= totalPages; i++)
            {
                //Условие что выводим только необходимые номера
                if (((i <= 3) || (i > (totalPages - 3))) || ((i > (currentPage - 2)) && (i < (currentPage + 2))))
                {
                    var subBuilder = new TagBuilder("a");
                    subBuilder.InnerHtml = i.ToString(CultureInfo.InvariantCulture);
                    if (i == currentPage)
                    {
                        subBuilder.MergeAttribute("href", pageUrl.Invoke(i));
                        builder.AppendLine("<li class=\"active\">" + subBuilder.ToString() + "</li>");
                    }
                    else
                    {
                        subBuilder.MergeAttribute("href", pageUrl.Invoke(i));
                        builder.AppendLine("<li>" + subBuilder.ToString() + "</li>");
                    }
                }
                else if ((i == 4) && (currentPage > 5))
                {
                    //Троеточие первое
                    builder.AppendLine("<li class=\"disabled\"> <a href=\"#\">...</a> </li>");
                }
                else if ((i == (totalPages - 3)) && (currentPage < (totalPages - 4)))
                {
                    //Троеточие второе
                    builder.AppendLine("<li class=\"disabled\"> <a href=\"#\">...</a> </li>");
                }
            }
            //Next
            var nextBuilder = new TagBuilder("a");
            nextBuilder.InnerHtml = "&raquo;";
            if (currentPage == totalPages)
            {
                nextBuilder.MergeAttribute("href", pageUrl.Invoke(currentPage));
                builder.AppendLine("<li class=\"active\">" + nextBuilder.ToString() + "</li>");
            }
            else
            {
                nextBuilder.MergeAttribute("href", pageUrl.Invoke(currentPage + 1));
                builder.AppendLine("<li>" + nextBuilder.ToString() + "</li>");
            }
            return new MvcHtmlString("<ul class='pagination'>" + builder.ToString() + "</ul>");
        }


    }
}