using System.Collections.Generic;

namespace GRG.LeisureCards.DomainModel
{
    public class OfferCategory
    {
        public OfferCategory()
        {
            LeisureCards = new List<LeisureCard>();    
        }

        public virtual string OfferCategoryKey { get; set; }
        public virtual string Name { get; set; }
        public virtual IList<LeisureCard> LeisureCards { get; set; }
    }
}
