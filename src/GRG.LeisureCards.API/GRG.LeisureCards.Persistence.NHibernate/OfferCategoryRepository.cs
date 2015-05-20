using System.Collections.Generic;
using System.Linq;
using GRG.LeisureCards.DomainModel;
using NHibernate.Linq;

namespace GRG.LeisureCards.Persistence.NHibernate
{
    public class OfferCategoryRepository : Repository<OfferCategory, int>, IOfferCategoryRepository
    {
        public OfferCategory RedLetter
        {
            get { return Session.Query<OfferCategory>().FirstOrDefault(c => c.OfferCategoryKey == "RL"); }
        }

        public OfferCategory ShortBreaks
        {
            get { return Session.Query<OfferCategory>().FirstOrDefault(c => c.OfferCategoryKey == "SB"); }
        }

        public OfferCategory TwoForOne
        {
            get { return Session.Query<OfferCategory>().FirstOrDefault(c => c.OfferCategoryKey == "241"); }
        }
    }
}
