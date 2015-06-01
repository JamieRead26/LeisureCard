using GRG.LeisureCards.DomainModel;

namespace GRG.LeisureCards.Persistence.NHibernate
{
    public class SessionRepository : Repository<Session, string>, ISessionRepository
    {
    }
}
