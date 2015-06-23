using System;
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

        protected T Dispatch<T>(Func<T> func)
        {
            try
            {
                return func();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw ex;
            }
        }

        protected void Dispatch(Action func)
        {
            try
            {
                func();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw ex;
            }
        }
    }
}