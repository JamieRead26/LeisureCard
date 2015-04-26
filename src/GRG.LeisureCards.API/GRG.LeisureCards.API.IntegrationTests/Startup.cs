using Owin;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using GRG.LeisureCards.API.IntegrationTests;
using GRG.LeisureCards.WebAPI;
using GRG.LeisureCards.WebAPI.App_Start;

namespace OwinSelfhostSample
{
    public class Startup
    {
        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder appBuilder)
        {
            // Configure Web API for self-host. 
            var config = new HttpConfiguration();

            //config.Services.Replace(typeof(IAssembliesResolver), new SelfHostAssemblyResolver());

            StructuremapMvc.Start();
            WebApiConfig.Register(config);

            

            
            StructuremapWebApi.Start();

            appBuilder.UseWebApi(config);
        }
    }
} 
