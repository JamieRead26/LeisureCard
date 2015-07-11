using Bootstrap4NHibernate.Data;
using GRG.LeisureCards.DomainModel;

namespace GRG.LeisureCards.GenerateDBSchema
{
    public class TenantDataFixture : DataFixture
    {
        public Tenant GRG { get; private set; }

        public override object[] GetEntities(IFixtureContainer fixtureContainer)
        {
            GRG = new Tenant { TenantKey = "GRG", Name = "Grass Roots Group", Active = true, Domain = "leisurecards.grg.com" };

            return new[] { GRG };
        }
    }
}
