using System.Linq;
using GRG.LeisureCards.Model;
using NHibernate.Linq;

namespace GRG.LeisureCards.Persistence.NHibernate
{
    public class OfferCategoryRepository : Repository<OfferCategory, int>, IOfferCategoryRepository
    {
        public OfferCategory RedLetter
        {
            get { return Session.Query<OfferCategory>().FirstOrDefault(c => c.Key == "RL"); }
        }

        public OfferCategory ShortBreaks
        {
            get { return Session.Query<OfferCategory>().FirstOrDefault(c => c.Key == "SB"); }
        }

        public OfferCategory TwoForOne
        {
            get { return Session.Query<OfferCategory>().FirstOrDefault(c => c.Key == "241"); }
        }
    }
}
