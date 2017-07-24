using System.Web;
using System.Web.Optimization;

namespace AEOWeb
{
    public class BundleConfig
    {
        // 有关 Bundling 的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/js")
                .Include(
                "~/Scripts/angular.min.js",
                "~/Scripts/angular-route.min.js",
                "~/Scripts/angular-resource.min.js",
                "~/Scripts/ng-table.min.js",
                "~/Scripts/bootstrap.min.js",
                "~/Scripts/jquery-1.9.1.min.js",
                "~/Scripts/angular-ui/ui-bootstrap-tpls.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/js/flot")
                .Include(
                "~/Scripts/flot/jquery.flot.min.js",
                "~/Scripts/flot/jquery.flot.pie.min.js"));
            bundles.Add(new StyleBundle("~/Content/css")
                .Include(
                "~/Content/bootstrap.min.css",
                "~/Content/ng-table.min.css",
                "~/Content/dashboard.css"));
        }
    }
}