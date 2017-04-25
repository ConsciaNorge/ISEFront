using System.Web;
using System.Web.Optimization;

namespace ISEFront
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                "~/Scripts/angular.min.js",
                "~/Scripts/angular-route.min.js",
                "~/Scripts/angular-sanitize.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/angular-ui").Include(
                "~/Scripts/angular-ui/ui-bootstrap.min.js",
                "~/Scripts/angular-ui/ui-bootstrap-tpls.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/dashboard/css").Include(
                      "~/Content/bootstrap.css"));

            bundles.Add(new StyleBundle("~/Content/light-bootstrap-dashboard").Include(
                "~/Content/light-bootstrap-dashboard/light-bootstrap-dashboard.css",
                "~/Content/light-bootstrap-dashboard/pe-icon-7-stroke.css"));

            bundles.Add(new StyleBundle("~/Content/animate").Include(
                "~/Content/animate.min.css"));

            bundles.Add(new StyleBundle("~/Content/fonts").Include(
                "~/Content/font-awesome.css"));

            bundles.Add(new ScriptBundle("~/bundles/light-bootstrap-dashboard").Include(
                "~/Scripts/light-bootstrap-dashboard/bootstrap-checkbox-radio-switch.js",
                "~/Scripts/light-bootstrap-dashboard/light-bootstrap-dashboard.js",
                "~/Scripts/light-bootstrap-dashboard/bootstrap-notify.js"));

            bundles.Add(new ScriptBundle("~/bundles/momentjs").Include(
                "~/Scripts/moment.min.js"
                ));

            bundles.Add(new StyleBundle("~/Content/bootstrap-datetime-picker").Include(
                      "~/Content/bootstrap-datetimepicker.min.css"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap-datetime-picker").Include(
                "~/Scripts/bootstrap-datetimepicker.min.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/dashboard")
                .IncludeDirectory("~/js/controllers", "*.js", true)
                .IncludeDirectory("~/js/interfaces", "*.js", true)
                .IncludeDirectory("~/js/models", "*.js", true)
                .IncludeDirectory("~/js/services", "*.js", true)
                .IncludeDirectory("~/js/directives", "*.js", true)
                .IncludeDirectory("~/js/app", "*.js", true)
                );

            bundles.Add(new ScriptBundle("~/bundles/angular-file-upload")
                .Include("~/Scripts/ng-file-upload.min.js")
                );
        }
    }
}
