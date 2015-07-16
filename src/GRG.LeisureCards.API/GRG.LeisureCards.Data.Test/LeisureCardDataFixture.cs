using System;
using Bootstrap4NHibernate.Data;
using GRG.LeisureCards.DomainModel;

namespace GRG.LeisureCards.Data.Test
{
    public class LeisureCardDataFixture : DataFixture
    {
        public LeisureCard[] Cards;

        public override object[] GetEntities(IFixtureContainer fixtureContainer)
        {
            var offerCategoryFixture = fixtureContainer.Get<OfferCategoryDataFixture>();
            var membershipTierDataFixture = fixtureContainer.Get<MembershipTierDataFixture>();
            var tenantFixture = fixtureContainer.Get<TenantDataFixture>();
            
            Cards = new[]
            {
                new LeisureCard
                {
                    Code = "InactiveClient", 
                    UploadedDate = DateTime.Now,
                    Reference = "IntTest",
                    MembershipTier = membershipTierDataFixture.Gold,
                    TenantKey = tenantFixture.Inactive.TenantKey
                },
                new LeisureCard
                {
                    Code = "Unregistered", 
                    UploadedDate = DateTime.Now,
                    Reference = "IntTest",
                    MembershipTier = membershipTierDataFixture.Gold,
                    TenantKey = tenantFixture.GRG.TenantKey
                },
                new LeisureCard
                {
                    Code = "Admin",
                    RenewalDate = DateTime.Now,
                    ExpiryDate = DateTime.Now + TimeSpan.FromDays(365),
                    RegistrationDate = DateTime.Now,
                    UploadedDate = DateTime.Now,
                    MembershipTier = membershipTierDataFixture.Gold,
                    TenantKey = tenantFixture.GRG.TenantKey
                },
                new LeisureCard
                {
                    Code = "Registered1",
                    RenewalDate = DateTime.Now,
                    ExpiryDate = DateTime.Now + TimeSpan.FromDays(365),
                    RegistrationDate = DateTime.Now,
                    UploadedDate = DateTime.Now,
                    MembershipTier = membershipTierDataFixture.Gold,
                    TenantKey = tenantFixture.GRG.TenantKey
                },
                new LeisureCard
                {
                    Code = "Registered2",
                    RenewalDate = DateTime.Now,
                    ExpiryDate = DateTime.Now + TimeSpan.FromDays(365),
                    RegistrationDate = DateTime.Now + TimeSpan.FromDays(1),
                    UploadedDate = DateTime.Now,
                    MembershipTier = membershipTierDataFixture.Gold,
                    TenantKey = tenantFixture.GRG.TenantKey
                },
                new LeisureCard
                {
                    Code = "Registered3",
                    RenewalDate = DateTime.Now,
                    ExpiryDate = DateTime.Now + TimeSpan.FromDays(365),
                    RegistrationDate = DateTime.Now + TimeSpan.FromDays(2),
                    UploadedDate = DateTime.Now,
                    MembershipTier = membershipTierDataFixture.Gold,
                    TenantKey = tenantFixture.GRG.TenantKey
                },
                new LeisureCard
                {
                    Code = "Registered4",
                    RenewalDate = DateTime.Now,
                    ExpiryDate = DateTime.Now + TimeSpan.FromDays(365),
                    RegistrationDate = DateTime.Now + TimeSpan.FromDays(3),
                    UploadedDate = DateTime.Now,
                    MembershipTier = membershipTierDataFixture.Gold,
                    TenantKey = tenantFixture.GRG.TenantKey
                },
                new LeisureCard
                {
                    Code = "PopupNotMandatory",
                    RenewalDate = DateTime.Now,
                    ExpiryDate = DateTime.Now + TimeSpan.FromDays(365),
                    RegistrationDate = DateTime.Now + TimeSpan.FromDays(3),
                    UploadedDate = DateTime.Now,
                    MembershipTier = membershipTierDataFixture.Gold,
                    TenantKey = tenantFixture.PopupNotMandatory.TenantKey
                },
                new LeisureCard
                {
                    Code = "PopupMandatory",
                    RenewalDate = DateTime.Now,
                    ExpiryDate = DateTime.Now + TimeSpan.FromDays(365),
                    RegistrationDate = DateTime.Now + TimeSpan.FromDays(3),
                    UploadedDate = DateTime.Now,
                    MembershipTier = membershipTierDataFixture.Gold,
                    TenantKey = tenantFixture.PopupMandatory.TenantKey
                },
                new LeisureCard
                {
                    Code = "Cancelled", 
                    Suspended = true, 
                    UploadedDate = DateTime.Now,
                    TenantKey = tenantFixture.GRG.TenantKey
                }
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
                    typeof(MembershipTierDataFixture),
                    typeof(TenantDataFixture)
                };  
            } 
        }
    }
}
