using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using GRG.LeisureCards.Data;
using GRG.LeisureCards.Persistence.NHibernate.ClassMaps;
using System.Web;
using System.Web.Mvc;

namespace GRG.LeisureCards.WebAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        public void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
//#if DEBUG
            DataBootstrap.PrepDb(Assembly.GetAssembly(typeof(LeisureCardClassMap)), Config.DbConnectionDetails);
//#endif
            GlobalConfiguration.Configure(WebApiConfig.Register);
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
