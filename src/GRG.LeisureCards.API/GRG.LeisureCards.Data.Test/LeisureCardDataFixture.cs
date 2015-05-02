using System;
using Bootstrap4NHibernate.Data;
using GRG.LeisureCards.Model;

namespace GRG.LeisureCards.Data.Test
{
    public class LeisureCardDataFixture : DataFixture
    {
        public override object[] GetEntities(IFixtureContainer fixtureContainer)
        {
            return new[]
            {
                new LeisureCard {Code = "Unregistered"},
                new LeisureCard
                {
                    Code = "Registered",
                    RenewalDate = DateTime.Now + TimeSpan.FromDays(365),
                    Registered = DateTime.Now
                },
                new LeisureCard {Code = "Suspended", Suspended = true}
            };
        }
    }
}
