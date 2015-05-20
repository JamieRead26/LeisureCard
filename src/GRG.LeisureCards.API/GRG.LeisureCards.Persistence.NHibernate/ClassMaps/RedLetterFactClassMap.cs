using FluentNHibernate.Mapping;
using GRG.LeisureCards.DomainModel;

namespace GRG.LeisureCards.Persistence.NHibernate.ClassMaps
{
    public class RedLetterFactClassMap: ClassMap<RedLetterFact>
    {
        public RedLetterFactClassMap()
        {
            Id(x => x.Id);
            Map(x => x.Fact);
            References(x => x.RedLetterProduct);
        }
    }
}
