using NUnit.Framework;

namespace GRG.LeisureCards.API.IntegrationTests
{
    [TestFixture]
    public class ShortBreakControllerTests : ControllerTests
    {
        [Test]
        public void ClaimOffer()
        {
            UserSession.GetShortBreakService().ClaimOffer("cottages");
        }
    }
}
