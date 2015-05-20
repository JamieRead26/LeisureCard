using System;
using System.Collections.Generic;
using GRG.LeisureCards.DomainModel;

namespace GRG.LeisureCards.Persistence.NHibernate
{
    public class LeisureCardUsageRepository : Repository<LeisureCardUsage, int>, ILeisureCardUsageRepository
    {
        public IEnumerable<LeisureCardUsage> Get(int count, int toId)
        {
            var query = Session.QueryOver<LeisureCardUsage>();

            if (toId > 0)
                query.Where(j => j.Id < toId);

            query.OrderBy(x => x.Id).Desc
                .Take(count);

            var result = query.List();

            return result;
        }

        public IEnumerable<LeisureCardUsage> Get(string cardId, int count, int toId)
        {
            var query = Session.QueryOver<LeisureCardUsage>();
              
             if (toId > 0)
                query.Where(j => j.Id < toId);

            query.Where(x => x.LeisureCard.Code == cardId)
                .OrderBy(x => x.Id)
                .Desc
                .Take(count);

            var result = query.List();

            return result;
        }

        public IEnumerable<LeisureCardUsage> Get(DateTime @from, DateTime to)
        {
            return Session.QueryOver<LeisureCardUsage>()
                .Where(u => u.LoginDateTime >= from)
                .Where(u => u.LoginDateTime <= to)
                .OrderBy(u=>u.LoginDateTime).Desc
                .List();
        }
    }
}
