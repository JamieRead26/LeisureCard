using System;
using System.Linq;
using GRG.LeisureCards.DomainModel;

namespace GRG.LeisureCards.Persistence.NHibernate
{
    public class SessionRepository : Repository<Session, string>, ISessionRepository
    {
        public Session GetLiveByCardCode(string code)
        {
            var utcNow = DateTime.UtcNow;
            var sessions = Session.QueryOver<Session>()
                .Where(c => c.CardCode == code)
                .OrderBy(s=>s.ExpiryUtc).Desc
                .List();

            foreach (var session in sessions.Where(s=>s.ExpiryUtc<utcNow))
                Session.Delete(session);

            return sessions.FirstOrDefault(s => s.ExpiryUtc > utcNow);
        }
    }
}
