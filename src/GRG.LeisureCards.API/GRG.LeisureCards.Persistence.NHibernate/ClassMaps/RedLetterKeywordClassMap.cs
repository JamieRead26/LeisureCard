using FluentNHibernate.Mapping;
using GRG.LeisureCards.DomainModel;

namespace GRG.LeisureCards.Persistence.NHibernate.ClassMaps
{
    public class RedLetterKeywordClassMap : ClassMap<RedLetterKeyword>
    {
        public RedLetterKeywordClassMap()
        {
            Id(x => x.Keyword).GeneratedBy.Assigned();

            HasManyToMany(x => x.Products)
                .Inverse()
                .Table("ProductKeywords")
                .Not.LazyLoad();
        }
    }
}
