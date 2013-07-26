using System.Web;
using System.Web.Optimization;

namespace VacationVillaManager
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-1.9.1.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-1.10.3.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            //bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));

            // Bootstrap bundles

            bundles.Add(new ScriptBundle("~/bundles/scripts-bootstrap").Include(
                "~/Scripts/jquery-1.9.1.js",
                "~/Scripts/jquery-migrate-1.9.1.js",
                "~/Scripts/bootstrap.js",
                "~/Scripts/jquery.validate.js",
                "~/scripts/jquery.validate.unobtrusive.js",
                "~/Scripts/jquery.validate.unobtrusive-custom-for-bootstrap.js"
                ));

            bundles.Add(new StyleBundle("~/content/css").Include(
                "~/Content/bootstrap.css",
                "~/Content/body.css",
                "~/Content/bootstrap-responsive.css",
                "~/Content/bootstrap-mvc-validation.css"
                ));

            bundles.Add(new StyleBundle("~/content/csspublic").Include(
                "~/Content/public/bootstrap.css",
                "~/Content/body.css",
                "~/Content/bootstrap-responsive.css",
                "~/Content/bootstrap-mvc-validation.css"
                ));

            bundles.Add(new StyleBundle("~/unitycss").Include(
                "~/Content/unity/plugins/bootstrap/css/bootstrap.css",
                "~/Content/unity/css/style.css",
                "~/Content/unity/css/headers/header1.css",
                "~/Content/unity/plugins/bootstrap/css/bootstrap-responsive.css",
                "~/Content/unity/css/style_responsive.css",
                "~/Content/unity/css/themes/default.css",
                "~/Content/unity/css/themes/headers/default.css"
                ));

            bundles.Add(new ScriptBundle("~/unityjs").Include(
                "~/Content/unity/js/jquery-1.8.2.js",
                "~/Content/unity/js/modernizr.custom.js",
                "~/Content/unity/plugins/bootstrap/js/bootstrap.js"
                ));

            //BundleTable.EnableOptimizations = true;
            
        }
    }
}