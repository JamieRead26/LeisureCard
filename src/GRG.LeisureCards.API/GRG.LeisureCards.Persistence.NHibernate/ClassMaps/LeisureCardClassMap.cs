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
            Map(x => x.IsAdmin);
            References(x => x.MembershipTier);
            HasManyToMany(x => x.OfferCategories)
               .Cascade.All()
               .Table("LeisureCardOfferCategories");
        }
    }
}
