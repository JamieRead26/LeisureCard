using FluentNHibernate.Mapping;
using GRG.LeisureCards.DomainModel;

namespace GRG.LeisureCards.Persistence.NHibernate.ClassMaps
{
    public class CardGenerationLogClassMap : ClassMap<CardGenerationLog>
    {
        public CardGenerationLogClassMap()
        {
            Id(x => x.Ref);
            Map(x => x.GeneratedDate);
        }
    }
}
