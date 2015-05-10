using System;
using Bootstrap4NHibernate.Data;
using GRG.LeisureCards.Model;

namespace GRG.LeisureCards.Data.Test
{
    public class MembershipTierDataFixture : DataFixture
    {
        public readonly MembershipTier Bronze = new MembershipTier { Key = "Bronze", Name = "Bronze" };
        public readonly MembershipTier Silver = new MembershipTier { Key = "Silver", Name = "Silver" };
        public readonly MembershipTier Gold = new MembershipTier { Key = "Gold", Name = "Gold" };
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
