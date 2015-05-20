using System.Reflection;
using GRG.LeisureCards.Data;
using GRG.LeisureCards.Data.Test;
using GRG.LeisureCards.Persistence.NHibernate.ClassMaps;
using NUnit.Framework;

namespace GRG.LeisureCards.Service.IntegrationTests
{
    [TestFixture]
    public class LeisureCardRegistrationServiceTests
    {
        //[TestFixtureSetUp]
        //public void FixtureSetup()
        //{
        //    DataBootstrap.PrepDb(Assembly.GetAssembly(typeof(LeisureCardClassMap)), Assembly.GetAssembly(typeof(TwoForOneOfferDataFixture)));
        //}

        [Test]
        public void RegisterCard_OK()
        {
            //var sut = new LeisureCardService(new CardRenewalLogic(), new LeisureCardRepository());

            //Assert.Equals(sut.Register("12345"), RegistrationResult.Ok);

        }
    }
}
