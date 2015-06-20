using System;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Optimization;
using GRG.LeisureCards.UI.Shizzle;
using log4net;
using log4net.Config;

namespace GRG.LeisureCards.UI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MvcApplication));

        protected void Application_Start()
        {
            XmlConfigurator.Configure();
            Log.Info("Leisure Cards UI Web App Started");

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //BundleConfig.RegisterBundles(BundleTable.Bundles, "GRG");
        }

        protected void Application_BeginRequest(object sender, EventArgs ea)
        {
            Log.Debug("Application_BeginRequest : " + Request.Url.Host);

            var incomingUrl = ConfigurationManager.AppSettings["IncomingUrl"];

            if (string.IsNullOrEmpty(incomingUrl)) 
                incomingUrl = Request.Url.Host;

            Log.Debug("Looking up tenant based on  : " + incomingUrl);

            BundleConfig.RegisterBundles(BundleTable.Bundles, TenantCache.Instance.GetTenant(incomingUrl).Key);
        }
    }
}
