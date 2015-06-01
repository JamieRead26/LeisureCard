using NUnit.Framework;
using System.Linq;

namespace GRG.LeisureCards.API.IntegrationTests
{
    [TestFixture]
    public class TwoForControllerTests : ControllerTests
    {
        [Test]
        public void GetAll()
        {
            Assert.IsTrue(UserSession.GetTwoforOneService().GetAll().Any());
        }

        [Test]
        public void LocationSearch()
        {
           Assert.IsTrue(UserSession.GetTwoforOneService().FindByLocation("southampton", 10).Any());
        }
    }
}