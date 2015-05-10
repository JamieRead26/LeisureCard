using FluentNHibernate.Mapping;
using GRG.LeisureCards.Model;

namespace GRG.LeisureCards.Persistence.NHibernate.ClassMaps
{
    public class LeisureCardClassMap : ClassMap<LeisureCard>
    {
        public LeisureCardClassMap()
        {
            Id(x => x.Code);
            Map(x => x.CancellationDate);
            Map(x => x.RegistrationDate);
            Map(x => x.RenewalDate);
            Map(x => x.ExpiryDate);
            Map(x => x.UploadedDate);
            References(x => x.MembershipTier);
            HasManyToMany(x => x.OfferCategories)
               .Cascade.All()
               .Table("LeisureCardOfferCategories");
        }
    }
}
