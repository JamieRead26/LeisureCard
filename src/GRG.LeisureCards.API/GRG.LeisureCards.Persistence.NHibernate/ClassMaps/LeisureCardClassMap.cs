using FluentNHibernate.Mapping;
using GRG.LeisureCards.DomainModel;

namespace GRG.LeisureCards.Persistence.NHibernate.ClassMaps
{
    public class LeisureCardClassMap : ClassMap<LeisureCard>
    {
        public LeisureCardClassMap()
        {
            Id(x => x.Code);
            Map(x => x.Suspended);
            Map(x => x.RegistrationDate);
            Map(x => x.RenewalDate);
            Map(x => x.ExpiryDate);
            Map(x => x.UploadedDate);
            Map(x => x.Deleted);
            Map(x => x.Reference);
            Map(x => x.RenewalPeriodMonths);
            Map(x => x.MembershipTermsAccepted);
            References(x => x.MembershipTier);
            HasManyToMany(x => x.OfferCategories)
               .Cascade.All()
               .Table("LeisureCardOfferCategories");
            Map(x => x.TenantKey);
            
        }
    }
}
