using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRG.LeisureCards.DomainModel
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

    public class SelectedOfferInfo
    {
        public SelectedOfferInfo() { }

        public SelectedOfferInfo(SelectedOffer selectedOffer)
        {
            Id = selectedOffer.Id;
          //  LeisureCardCode = selectedOffer.LeisureCard.Code;
            SelectedDateTime = selectedOffer.SelectedDateTime;
           // OfferCategory = selectedOffer.OfferCategory.Name;
            OfferTitle = selectedOffer.OfferTitle;
            OfferId = selectedOffer.OfferId;
        }

        public int Id { get; set; }

        public string LeisureCardCode { get; set; }

        public DateTime SelectedDateTime { get; set; }

        public string OfferCategory { get; set; }

        public string OfferTitle { get; set; }

        public string OfferId { get; set; }
    }
}
