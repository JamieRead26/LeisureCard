using System;
using System.Collections.Generic;
using GRG.LeisureCards.DomainModel;
using NHibernate;

namespace GRG.LeisureCards.Persistence.NHibernate
{
    public class SelectedOfferRepository : Repository<SelectedOffer, int>, ISelectedOfferRepository
    {
        public IEnumerable<SelectedOffer> Get(DateTime @from, DateTime to)
        {
            var results = Session.QueryOver<SelectedOffer>()
                .Where(u => u.SelectedDateTime >= from)
                .Where(u => u.SelectedDateTime <= to)
                .OrderBy(u=>u.SelectedDateTime).Desc
                .List();

            foreach (var selectedOffer in results)
            {
                NHibernateUtil.Initialize(selectedOffer.LeisureCard);
                NHibernateUtil.Initialize(selectedOffer.OfferCategory);
            }

            return results;
        }
    }
}
