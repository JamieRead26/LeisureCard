using System.Reflection;
using GRG.LeisureCards.Data;
using GRG.LeisureCards.Persistence.NHibernate.ClassMaps;
using NUnit.Framework;

namespace GRG.LeisureCards.API.IntegrationTests
{
    public abstract class ControllerTests
    {
        public static bool Done;
        public static object Lock = new object();

        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            if (Done)
                return;

            lock(Lock)
            {
                if (Done)
                    return;
                
                DataBootstrap.PrepDb(Assembly.GetAssembly(typeof(LeisureCardClassMap)), Config.DbConnectionDetails);

                Done = true;
            }
            
        }
    }
}
