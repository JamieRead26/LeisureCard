using System;
using System.Collections.Generic;
using System.Linq;
using Bootstrap4NHibernate.Data;
using GRG.LeisureCards.DomainModel;

namespace GRG.LeisureCards.Data.Test
{
    public class LeisureCardUsageDataFixture : DataFixture
    {
        public override Type[] Dependencies
        {
            get { return new[] {typeof (LeisureCardDataFixture)}; }
        }

        public override object[] GetEntities(IFixtureContainer fixtureContainer)
        {
            var usages = new List<LeisureCardUsage>();
            foreach (var card in fixtureContainer.Get<LeisureCardDataFixture>().Cards)
            {
                var from = new DateTime(2000,1,1);

                for (var i = 0; i < 25; i++)
                    usages.Add(new LeisureCardUsage { LeisureCard = card, LoginDateTime = from + TimeSpan.FromDays(i) });
            }

            return usages.ToArray();
        }
    }
}
