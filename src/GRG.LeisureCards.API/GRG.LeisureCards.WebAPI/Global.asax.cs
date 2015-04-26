using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using GRG.LeisureCards.Data;
using GRG.LeisureCards.Persistence.NHibernate.ClassMaps;

namespace GRG.LeisureCards.WebAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        public void Application_Start()
        {
#if DEBUG
            DataBootstrap.PrepDb(Assembly.GetAssembly(typeof(LeisureCardClassMap)));
#endif
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
