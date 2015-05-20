using System;
using Bootstrap4NHibernate.Data;
using GRG.LeisureCards.DomainModel;

namespace GRG.LeisureCards.Data.Test
{
    public class MembershipTierDataFixture : DataFixture
    {
        public readonly MembershipTier Bronze = new MembershipTier { TierKey = "Bronze", Name = "Bronze" };
        public readonly MembershipTier Silver = new MembershipTier { TierKey = "Silver", Name = "Silver" };
        public readonly MembershipTier Gold = new MembershipTier { TierKey = "Gold", Name = "Gold" };
        public override object[] GetEntities(IFixtureContainer fixtureContainer)
        {
            return new[]
            {
                Bronze,
                Silver,
                Gold
            };
        }
    }
}
