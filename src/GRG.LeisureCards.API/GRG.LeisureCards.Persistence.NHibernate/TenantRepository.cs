using GRG.LeisureCards.DomainModel;

namespace GRG.LeisureCards.Persistence.NHibernate
{
    public class TenantRepository : Repository<Tenant,string>, ITenantRepository
    {
    }
}
