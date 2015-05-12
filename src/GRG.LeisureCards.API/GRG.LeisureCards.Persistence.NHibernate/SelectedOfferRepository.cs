using System;
using System.Collections.Generic;
using GRG.LeisureCards.Model;

namespace GRG.LeisureCards.Persistence.NHibernate
{
    public class SelectedOfferRepository : Repository<SelectedOffer, int>, ISelectedOfferRepository
    {
        public IEnumerable<SelectedOffer> Get(DateTime @from, DateTime to)
        {
            return Session.QueryOver<SelectedOffer>()
                .Where(u => u.SelectedDateTime >= from)
                .Where(u => u.SelectedDateTime <= to)
                .OrderBy(u=>u.SelectedDateTime).Desc
                .List();
        }
    }
}
