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
        public static void RegisterBundles(BundleCollection bundles, string tenant)
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
                        "~/Scripts/app/adminController.js",
                        "~/Scripts/app/app.js",
                        "~/Scripts/app/directives.js",
                        "~/Scripts/app/config.js");

            appScriptsBundle.Builder = nullBuilder;
            appScriptsBundle.Transforms.Add(scriptTransformer);
            appScriptsBundle.Orderer = nullOrderer;
            bundles.Add(appScriptsBundle);

            var commonScriptsBundle = new Bundle("~/bundles/sitejs");
            commonScriptsBundle.Include(
                        "~/Scripts/jquery.bxslider.min.js");

            commonScriptsBundle.Builder = nullBuilder;
            commonScriptsBundle.Transforms.Add(scriptTransformer);
            commonScriptsBundle.Orderer = nullOrderer;
            bundles.Add(commonScriptsBundle);

            var commonStylesBundle = new Bundle("~/bundles/css");
            
            commonStylesBundle.Include(
                        string.Format("~/Content/global/css/1140-grid.scss"),
                        string.Format("~/Content/global/css/bxslider.scss"),
                        string.Format("~/Content/global/css/admin.scss"),

                        string.Format("~/Content/{0}/css/footer.scss", tenant),
                        string.Format("~/Content/{0}/css/header.scss", tenant),
                        string.Format("~/Content/{0}/css/form.scss", tenant),
                        string.Format("~/Content/{0}/css/offers.scss", tenant),
                        string.Format("~/Content/{0}/css/site.scss", tenant));

            commonStylesBundle.Builder = nullBuilder;
            commonStylesBundle.Transforms.Add(styleTransformer);
            commonStylesBundle.Orderer = nullOrderer;
            bundles.Add(commonStylesBundle);

            BundleTable.EnableOptimizations = true;
        }
    }
}
