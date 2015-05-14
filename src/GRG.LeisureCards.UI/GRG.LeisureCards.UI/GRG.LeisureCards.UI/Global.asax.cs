using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Optimization;
using System.Configuration;

namespace GRG.LeisureCards.UI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            string tenant = ConfigurationManager.AppSettings.Get("tenant-key");
            if (string.IsNullOrEmpty(tenant))
                tenant = "base";

            // register bundles 
            BundleConfig.RegisterBundles(BundleTable.Bundles, tenant);
        }
    }
}
