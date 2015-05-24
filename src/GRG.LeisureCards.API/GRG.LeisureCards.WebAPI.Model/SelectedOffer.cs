using System;

namespace GRG.LeisureCards.WebAPI.Model
{
    public class SelectedOffer
    {
        public string LeisureCardCode { get; set; }

        public DateTime SelectedDateTime { get; set; }

        public string OfferCategory { get; set; }

        public string OfferTitle { get; set; }
    }
}
