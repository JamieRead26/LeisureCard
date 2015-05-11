using FluentNHibernate.Mapping;
using GRG.LeisureCards.Model;

namespace GRG.LeisureCards.Persistence.NHibernate.ClassMaps
{
    public class OfferCategoryClassMap : ClassMap<OfferCategory>
    {
        public OfferCategoryClassMap()
        {
            Id(x => x.Key).GeneratedBy.Assigned();
            Map(x => x.Name);

            HasManyToMany(x => x.LeisureCards)
                .Inverse()
                .Table("LeisureCardOfferCategories")
                .Not.LazyLoad();
        }
    }
}
