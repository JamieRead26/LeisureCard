using System.Web;
using System.Web.Optimization;
using BundleTransformer.Core.Builders;
using BundleTransformer.Core.Orderers;
using BundleTransformer.Core.Resolvers;
using BundleTransformer.Core.Transformers;

namespace GRG.LeisureCards.UI
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.UseCdn = true;
            bundles.IgnoreList.Clear();

            var nullBuilder = new NullBuilder();
            var styleTransformer = new StyleTransformer();
            var scriptTransformer = new ScriptTransformer();
            var nullOrderer = new NullOrderer();

            BundleResolver.Current = new CustomBundleResolver();

            var appScriptsBundle = new Bundle("~/bundles/appjs");
            appScriptsBundle.Include(
                        "~/Scripts/app/authController.js",
                        "~/Scripts/app/offersHomeController.js",
                        "~/Scripts/app/offers241Controller.js",
                        "~/Scripts/app/offersExperienceController.js",
                        "~/Scripts/app/app.js",
                         "~/Scripts/app/config.js",
                        "~/Scripts/app/directives.js");

            appScriptsBundle.Builder = nullBuilder;
            appScriptsBundle.Transforms.Add(scriptTransformer);
            appScriptsBundle.Orderer = nullOrderer;
            bundles.Add(appScriptsBundle);

            var commonScriptsBundle = new Bundle("~/bundles/sitejs");
            commonScriptsBundle.Include(
                        "~/Scripts/jquery.bxslider.min.js",
                        "~/Scripts/site.base.js");

            commonScriptsBundle.Builder = nullBuilder;
            commonScriptsBundle.Transforms.Add(scriptTransformer);
            commonScriptsBundle.Orderer = nullOrderer;
            bundles.Add(commonScriptsBundle);

            var commonStylesBundle = new Bundle("~/bundles/css");
            commonStylesBundle.Include(
                        "~/Content/css/1140-grid.scss",
                        "~/Content/css/footer.scss",
                        "~/Content/css/header.scss",
                        "~/Content/css/form.scss",
                        "~/Content/css/offers.scss",
                        "~/Content/css/bxslider.scss",
                        "~/Content/css/site.scss");

            commonStylesBundle.Builder = nullBuilder;
            commonStylesBundle.Transforms.Add(styleTransformer);
            commonStylesBundle.Orderer = nullOrderer;
            bundles.Add(commonStylesBundle);

            BundleTable.EnableOptimizations = true;
        }
    }
}
