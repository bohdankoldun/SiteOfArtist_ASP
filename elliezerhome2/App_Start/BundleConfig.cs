using System.Web;
using System.Web.Optimization;

namespace elliezerhome2
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*")); 

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap_gallery").Include(
                      "~/Scripts/jquery.blueimp-gallery.min.js",
                      "~/Scripts/bootstrap-image-gallery*"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/timer").Include(
                    "~/Scripts/Gulpfile.js",
                    "~/Scripts/jquery.time-to*"
                    ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/my_bootstrap.css",
                      "~/Content/Site.css"));

            bundles.Add(new StyleBundle("~/Content/bootstrap_gallery").Include(
                   "~/Content/blueimp-gallery.min.css",
                   "~/Content/bootstrap-image-gallery.min.css"
                   ));
        }
    }
}
