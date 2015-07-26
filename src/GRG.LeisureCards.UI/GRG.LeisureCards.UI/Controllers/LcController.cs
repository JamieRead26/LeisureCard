using System;
using System.Web.Mvc;
using log4net;

namespace GRG.LeisureCards.UI.Controllers
{
    public abstract class LcController : Controller
    {
        protected readonly ILog Log;

        protected LcController()
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