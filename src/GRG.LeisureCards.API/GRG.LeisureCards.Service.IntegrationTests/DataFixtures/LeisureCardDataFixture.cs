using Bootstrap4NHibernate.Data;
using GRG.LeisureCards.Model;

namespace GRG.LeisureCards.Service.IntegrationTests.DataFixtures
{
    public class LeisureCardDataFixture : DataFixture
    {
        public override object[] GetEntities(IFixtureContainer fixtureContainer)
        {
            return new object[]
            {
                new LeisureCard {Code = "12345"}
            };
        }
    }
}
