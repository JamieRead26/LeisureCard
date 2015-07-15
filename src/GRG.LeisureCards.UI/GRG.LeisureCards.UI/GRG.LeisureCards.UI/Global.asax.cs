﻿using System;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Optimization;
using GRG.LeisureCards.UI.Content;
using GRG.LeisureCards.UI.Shizzle;
using GRG.LeisureCards.WebAPI.Client;
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

            HtmlContent.Init(
                Server,
                new TenantService(ConfigurationManager.AppSettings["ApiUrl"]).GetAll().Select(t => t.TenantKey).ToArray());

            //BundleConfig.RegisterBundles(BundleTable.Bundles, "GRG");
        }

        //protected void Application_BeginRequest(object sender, EventArgs ea)
        //{
        //    Log.Debug("Application_BeginRequest : " + Request.Url.Host);

        //    var incomingUrl = ConfigurationManager.AppSettings["IncomingUrl"];

        //    if (string.IsNullOrEmpty(incomingUrl)) 
        //        incomingUrl = Request.Url.Host;

        //    Log.Debug("Looking up tenant based on  : " + incomingUrl);

        //    BundleConfig.RegisterBundles(BundleTable.Bundles, TenantCache.Instance.GetTenant(incomingUrl).Key);
        //}

        protected void Session_Start(object sender, EventArgs ea)
        {
            Log.Debug("Application_BeginRequest : " + Request.Url.Host);

            var incomingUrl = ConfigurationManager.AppSettings["IncomingUrl"];

            if (string.IsNullOrEmpty(incomingUrl))
                incomingUrl = Request.Url.Host;

            Log.Debug("Looking up tenant based on : " + incomingUrl);

            var tenantKey = TenantCache.Instance.GetTenant(incomingUrl).TenantKey;

            Session["TenantKey"] = tenantKey;
            Session["ApiBaseUrl"] = ConfigurationManager.AppSettings["ApiUrl"];
            Session["GoogleScriptName"] = string.Format("google_analytics_{0}.js",
                ConfigurationManager.AppSettings["Release"]);

            BundleConfig.RegisterBundles(BundleTable.Bundles, tenantKey);
        }
    }
}
