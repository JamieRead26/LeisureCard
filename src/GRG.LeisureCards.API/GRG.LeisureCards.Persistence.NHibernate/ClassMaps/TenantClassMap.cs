using FluentNHibernate.Mapping;
using GRG.LeisureCards.DomainModel;

namespace GRG.LeisureCards.Persistence.NHibernate.ClassMaps
{
    public class TenantClassMap : ClassMap<Tenant>
    {
        public TenantClassMap()
        {
            Id(x => x.Key);
            Map(x => x.Name);
        }
    }
}
