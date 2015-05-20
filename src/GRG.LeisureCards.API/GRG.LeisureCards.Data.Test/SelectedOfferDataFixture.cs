using System;
using System.Collections.Generic;
using Bootstrap4NHibernate.Data;
using GRG.LeisureCards.DomainModel;

namespace GRG.LeisureCards.Data.Test
{
    public class SelectedOfferDataFixture : DataFixture
    {
        public override object[] GetEntities(IFixtureContainer fixtureContainer)
        {
            var leisureCardFixture = fixtureContainer.Get<LeisureCardDataFixture>();
            var offerCategoryFixture = fixtureContainer.Get<OfferCategoryDataFixture>();

            var selectedOffers = new List<SelectedOffer>();

            foreach (var offerCategory in offerCategoryFixture.All)
            {
                foreach (var leisureCard in leisureCardFixture.Cards)
                {
                    var from = new DateTime(2000, 1, 1);
                    for (var i = 0; i < 30; i++)
                    {
                        selectedOffers.Add(new SelectedOffer
                        {
                            LeisureCard = leisureCard,
                            OfferCategory = offerCategory,
                            SelectedDateTime = from + TimeSpan.FromDays(i),
                            OfferId = i.ToString(),
                            OfferTitle = i.ToString()
                        });
                    }
                }
            }

            return selectedOffers.ToArray();
        }

        public override Type[] Dependencies
        {
            get
            {
                return new[]
                {
                    typeof (LeisureCardDataFixture),
                    typeof (OfferCategoryDataFixture)
                };
            }
        }
    }
}
