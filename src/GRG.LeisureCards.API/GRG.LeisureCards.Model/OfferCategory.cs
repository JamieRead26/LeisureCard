using System.Collections.Generic;

namespace GRG.LeisureCards.Model
{
    public class OfferCategory
    {
        public OfferCategory()
        {
            LeisureCards = new List<LeisureCard>();    
        }

        public virtual string Key { get; set; }
        public virtual string Name { get; set; }
        public virtual IList<LeisureCard> LeisureCards { get; set; }
    }
}
