using System.Collections.Generic;
using GRG.LeisureCards.DomainModel;

namespace GRG.LeisureCards.Persistence.NHibernate
{
    public class TwoForOneRepository : Repository<TwoForOneOffer, int>, ITwoForOneRepository
    {
        public IEnumerable<TwoForOneOffer> GetWithNoLatLong()
        {
            return Session.QueryOver<TwoForOneOffer>().Where(o => o.Latitude == 0).Where(o => o.Longitude == 0).List();
        }
    }
}
