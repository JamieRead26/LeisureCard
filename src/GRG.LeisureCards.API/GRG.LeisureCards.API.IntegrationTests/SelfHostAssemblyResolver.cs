using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Dispatcher;
using GRG.LeisureCards.Model;
using GRG.LeisureCards.WebAPI.Controllers;

namespace GRG.LeisureCards.API.IntegrationTests
{
    public class SelfHostAssemblyResolver : IAssembliesResolver
    {
        public ICollection<Assembly> GetAssemblies()
        {
            return new List<Assembly> {Assembly.GetAssembly(typeof (LeisureCardController))};
        }
    }
}
