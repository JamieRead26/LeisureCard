using System.Reflection;
using GRG.LeisureCards.WebAPI.ClientContract;
using GRG.LeisureCards.Data;
using GRG.LeisureCards.Persistence.NHibernate.ClassMaps;
using GRG.LeisureCards.WebAPI.Client;
using NUnit.Framework;

namespace GRG.LeisureCards.API.IntegrationTests
{
    public abstract class ControllerTests
    {
        public static bool Done;
        public static object Lock = new object();

        protected readonly ILoginService LoginService;

        protected ControllerTests()
        {
            LoginService = new LoginService(Config.BaseAddress);
        }

        private static ISession _adminSession;
        public ISession AdminSession
        {
            get
            {
                if (_adminSession == null)
                    LoginService.Login(Config.AdminCode, out _adminSession);

                return _adminSession;
            }
        }

        private static ISession _userSession;
        public ISession UserSession
        {
            get
            {
                if (_userSession == null)
                    LoginService.Login(Config.UserCode, out _userSession);

                return _userSession;
            }
        }

      /*  [SetUp]
        public void FixtureSetUp()
        {
            if (Done)
                return;

            lock (Lock)
            {
                if (Done)
                    return;

                DataBootstrap.PrepDb(Assembly.GetAssembly(typeof(LeisureCardClassMap)), Config.DbConnectionDetails);

                Done = true;
            }
            
        }*/
    }
}
