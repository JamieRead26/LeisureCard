using System;

namespace GRG.LeisureCards.DomainModel
{
    public class SelectedOffer
    {
        public virtual int Id { get; set; }

        public virtual string LeisureCardCode { get; set; }

        public virtual DateTime SelectedDateTime { get; set; }

        public virtual OfferCategory OfferCategory { get; set; }

        public virtual string OfferTitle { get; set; }

        public virtual string OfferId { get; set; }

        public virtual string SessionToken { get; set; }
    }
}
