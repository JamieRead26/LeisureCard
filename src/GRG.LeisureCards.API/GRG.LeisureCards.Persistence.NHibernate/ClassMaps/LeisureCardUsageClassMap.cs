using FluentNHibernate.Mapping;
using GRG.LeisureCards.DomainModel;

namespace GRG.LeisureCards.Persistence.NHibernate.ClassMaps
{
    public class LeisureCardUsageClassMap : ClassMap<LeisureCardUsage>
    {
        public LeisureCardUsageClassMap()
        {
            Id(x => x.Id);
            Map(x => x.LoginDateTime);
            References(x => x.LeisureCard);
        }
    }
}
