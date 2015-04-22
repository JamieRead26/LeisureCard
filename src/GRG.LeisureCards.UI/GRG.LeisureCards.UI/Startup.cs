using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GRG.LeisureCards.UI.Startup))]
namespace GRG.LeisureCards.UI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
