using System;
using System.Collections.Generic;
using System.Linq;
using Bootstrap4NHibernate.Data;
using GRG.LeisureCards.Model;

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
            var card = fixtureContainer.Get<LeisureCardDataFixture>().Cards.First(c => c.Code == "Registered");
            var from = DateTime.Now - TimeSpan.FromDays(30);

            for (var i = 0; i < 25; i++)
                usages.Add(new LeisureCardUsage{LeisureCard = card, LoginDateTime = from+TimeSpan.FromDays(i)});

            return usages.ToArray();
        }
    }
}
