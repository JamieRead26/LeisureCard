using Bootstrap4NHibernate.Data;
using GRG.LeisureCards.DomainModel;

namespace GRG.LeisureCards.GenerateDBSchema
{
    public class TenantDataFixture : DataFixture
    {
        public Tenant GRG { get; private set; }
        public Tenant NPower { get; private set; }

        public override object[] GetEntities(IFixtureContainer fixtureContainer)
        {
            GRG = new Tenant { TenantKey = "GRG", Name = "Grass Roots Group", Active = true, Domain = "leisurecards.grg.com" };
            NPower = new Tenant { TenantKey = "NPower", Name = "NPower", Active = true, Domain = "leisurecards.npower.com" };

            if (Config.Release == Release.Prod)
            {
                GRG.Domain = "www.leisure-card.co.uk";
                NPower.Domain = "www.npowerdaysout.com";
            }

            if (Config.Release == Release.UAT)
            {
                GRG.Domain = "leisure-card-uat.grgtest.co.uk";
                NPower.Domain = "npowerthankyou-uat.grgtest.co.uk";
            }

            return new[] { GRG, NPower };
        }
    }
}
