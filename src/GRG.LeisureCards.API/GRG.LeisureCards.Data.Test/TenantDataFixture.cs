using Bootstrap4NHibernate.Data;
using GRG.LeisureCards.DomainModel;

namespace GRG.LeisureCards.Data.Test
{
    public class TenantDataFixture : DataFixture
    {
        public Tenant GRG { get; private set; }
        public override object[] GetEntities(IFixtureContainer fixtureContainer)
        {
            GRG = new Tenant {Key = "GRG", Name = "Grass Roots Group"};
            
            return new[]{GRG};
        }
    }
}
