using System;
using System.Configuration;
using System.Reflection;
using System.Web.Http;
using System.Web;
using System.Web.Mvc;
using log4net;
using log4net.Config;

namespace GRG.LeisureCards.WebAPI
{
    public class WebApiApplication : HttpApplication
    {
        public static string AppRoot;

        private static readonly ILog Log = LogManager.GetLogger(typeof(WebApiApplication));

        public void Application_Start()
        {
            XmlConfigurator.Configure();
            Log.Info("Leisure Cards API Web App Started");

            AreaRegistration.RegisterAllAreas();

            Mappings.Mapping.Register();
            GlobalConfiguration.Configure(WebApiConfig.Register);

           AppRoot = HttpContext.Current.Server.MapPath("~");

        }
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");
            if (HttpContext.Current.Request.HttpMethod == "OPTIONS")
            {
                HttpContext.Current.Response.AddHeader("Cache-Control", "no-cache");
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Accept, SessionToken");
                HttpContext.Current.Response.AddHeader("Access-Control-Max-Age", "1728000");
                HttpContext.Current.Response.End();
            }
        }
    }
}
