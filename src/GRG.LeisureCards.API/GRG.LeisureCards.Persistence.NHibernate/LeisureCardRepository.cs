using System;
using System.Collections.Generic;
using GRG.LeisureCards.Model;

namespace GRG.LeisureCards.Persistence.NHibernate
{
    public class LeisureCardRepository : Repository<LeisureCard, string>, ILeisureCardRepository
    {
        public IEnumerable<LeisureCard> GetRegistrationHistory(DateTime @from, DateTime to)
        {
            return Session.QueryOver<LeisureCard>()
                .Where(u => u.RegistrationDate >= from)
                .Where(u => u.RegistrationDate <= to)
                .OrderBy(f=>f.RegistrationDate).Desc
                .List();
        }
    }
}
