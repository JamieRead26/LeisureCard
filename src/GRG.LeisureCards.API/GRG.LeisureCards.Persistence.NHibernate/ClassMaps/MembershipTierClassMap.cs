using FluentNHibernate.Mapping;
using GRG.LeisureCards.DomainModel;

namespace GRG.LeisureCards.Persistence.NHibernate.ClassMaps
{
    public class MembershipTierClassMap : ClassMap<MembershipTier>
    {
        public MembershipTierClassMap()
        {
            Id(x => x.TierKey).GeneratedBy.Assigned();
            Map(x => x.Name);
        }
    }
}
