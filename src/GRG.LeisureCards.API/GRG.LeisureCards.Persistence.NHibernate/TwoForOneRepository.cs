using System.Linq;
using GRG.LeisureCards.Model;
using NHibernate.Linq;

namespace GRG.LeisureCards.Persistence.NHibernate
{
    public class TwoForOneRepository : Repository<TwoForOneOffer, int>, ITwoForOneRepository
    {
        public OfferCategory OfferCategory
        {
            get { return Session.Query<OfferCategory>().SingleOrDefault(x => x.Key == "241"); }
        }
    }
}
