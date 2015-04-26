using GRG.LeisureCards.Persistence.NHibernate;
using GRG.LeisureCards.Service.BusinessLogic;
using NUnit.Framework;

namespace GRG.LeisureCards.Service.IntegrationTests
{
    [TestFixture]
    public class LeisureCardRegistrationServiceTests
    {
        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            DataBootstrap.PrepDb();
        }

        [Test]
        public void RegisterCard_OK()
        {
            //var sut = new LeisureCardService(new CardRenewalLogic(), new LeisureCardRepository());

            //Assert.Equals(sut.Register("12345"), RegistrationResult.Ok);

        }
    }
}
