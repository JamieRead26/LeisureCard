using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRG.LeisureCards.Model
{
    public class SelectedOffer
    {
        public virtual int Id { get; set; }

        public virtual LeisureCard LeisureCard { get; set; }

        public virtual DateTime SelectedDateTime { get; set; }

        public virtual OfferCategory OfferCategory { get; set; }

        public virtual string OfferTitle { get; set; }

        public virtual string OfferId { get; set; }
    }
}
