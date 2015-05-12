using System;
using Bootstrap4NHibernate.Data;
using GRG.LeisureCards.Model;

namespace GRG.LeisureCards.Data.Test
{
    public class LeisureCardDataFixture : DataFixture
    {
        public LeisureCard[] Cards;

        public override object[] GetEntities(IFixtureContainer fixtureContainer)
        {
            var offerCategoryFixture = fixtureContainer.Get<OfferCategoryDataFixture>();
            var membershipTierDataFixture = fixtureContainer.Get<MembershipTierDataFixture>();
            
            Cards = new[]
            {
                new LeisureCard {Code = "Unregistered"},
                new LeisureCard
                {
                    Code = "Admin",
                    RenewalDate = DateTime.Now + TimeSpan.FromDays(365),
                    RegistrationDate = DateTime.Now,
                    UploadedDate = DateTime.Now,
                    MembershipTier = membershipTierDataFixture.Gold,
                    IsAdmin = true
                },
                new LeisureCard
                {
                    Code = "Registered1",
                    RenewalDate = DateTime.Now + TimeSpan.FromDays(365),
                    RegistrationDate = DateTime.Now,
                    UploadedDate = DateTime.Now,
                    MembershipTier = membershipTierDataFixture.Gold
                },
                new LeisureCard
                {
                    Code = "Registered2",
                    RenewalDate = DateTime.Now + TimeSpan.FromDays(365),
                    RegistrationDate = DateTime.Now,
                    UploadedDate = DateTime.Now,
                    MembershipTier = membershipTierDataFixture.Gold
                },
                new LeisureCard
                {
                    Code = "Registered3",
                    RenewalDate = DateTime.Now + TimeSpan.FromDays(365),
                    RegistrationDate = DateTime.Now,
                    UploadedDate = DateTime.Now,
                    MembershipTier = membershipTierDataFixture.Gold
                },
                new LeisureCard
                {
                    Code = "Registered4",
                    RenewalDate = DateTime.Now + TimeSpan.FromDays(365),
                    RegistrationDate = DateTime.Now,
                    UploadedDate = DateTime.Now,
                    MembershipTier = membershipTierDataFixture.Gold
                },
                new LeisureCard {Code = "Cancelled", CancellationDate = true}
            };

            foreach (var leisureCard in Cards)
                foreach (var offerCategory in offerCategoryFixture.All)
                    leisureCard.AddOfferCategory(offerCategory);

            return Cards;
        }

        public override Type[] Dependencies
        {
            get
            {
                return new[]
                {
                    typeof(OfferCategoryDataFixture),
                    typeof(MembershipTierDataFixture)
                };  
            } 
        }
    }
}
