using System.Web.Http;
using log4net;

namespace GRG.LeisureCards.WebAPI.Controllers
{
    public abstract class LcApiController : ApiController
    {
        protected readonly ILog Log;

        protected LcApiController()
        {
            Log = LogManager.GetLogger(GetType());
        }
    }
}