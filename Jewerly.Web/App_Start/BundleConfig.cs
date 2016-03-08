using System.Web;
using System.Web.Optimization;

namespace Jewerly.Web
{
    public class BundleConfig
    {
        //Дополнительные сведения об объединении см. по адресу: http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Используйте версию Modernizr для разработчиков, чтобы учиться работать. Когда вы будете готовы перейти к работе,
            // используйте средство сборки на сайте http://modernizr.com, чтобы выбрать только нужные тесты.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/modalform").Include(
                  "~/Scripts/modalform.js"));


            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
            bundles.Add(new ScriptBundle("~/bundles/file_uploader").Include(
                "~/Scripts/fine-uploader/header.js",
                 "~/Scripts/fine-uploader/util.js",
                  "~/Scripts/fine-uploader/button.js",
                   "~/Scripts/fine-uploader/ajax.requester.js",
                    "~/Scripts/fine-uploader/deletefile.ajax.requester.js",
                     "~/Scripts/fine-uploader/handler.base.js",
                      "~/Scripts/fine-uploader/window.receive.message.js",
                       "~/Scripts/fine-uploader/handler.form.js",
                       "~/Scripts/fine-uploader/handler.xhr.js",
                       "~/Scripts/fine-uploader/uploader.basic.js",
                       "~/Scripts/fine-uploader/dnd.js",
                       "~/Scripts/fine-uploader/uploader.js",
                       "~/Scripts/fine-uploader/jquery-plugin.js"
            ));
        }
    }
}
