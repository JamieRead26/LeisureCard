using System;
using System.Collections.Generic;
using GRG.LeisureCards.DomainModel;

namespace GRG.LeisureCards.Persistence.NHibernate
{
    public class CardGenerationLogRepository : Repository<CardGenerationLog, string>, ICardGenerationLogRepository
    {
        public IEnumerable<CardGenerationLog> Get(DateTime @from, DateTime to)
        {
            return Session.QueryOver<CardGenerationLog>()
                .Where(c => c.GeneratedDate >= from.Date)
                .Where(c => c.GeneratedDate <= to.Date)
                .List();
        }
    }
}
