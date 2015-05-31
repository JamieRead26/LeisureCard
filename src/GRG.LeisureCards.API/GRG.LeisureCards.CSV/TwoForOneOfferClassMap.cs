using CsvHelper.Configuration;
using GRG.LeisureCards.DomainModel;

namespace GRG.LeisureCards.CSV
{
    public sealed class TwoForOneOfferClassMap : CsvClassMap<TwoForOneOffer>
    {
        public TwoForOneOfferClassMap()
        {
            Map(m => m.Id).Name("OfferId");
            Map(m => m.OutletName).Name("Outlet Name");
            Map(m => m.Address1).Name("Address line 1");
            Map(m => m.Address2).Name("Address line 2");
            Map(m => m.TownCity).Name("Town/city");
            Map(m => m.County).Name("County");
            Map(m => m.PostCode).Name("Postcode");
            Map(m => m.Phone).Name("Phone  ");
            Map(m => m.Website).Name("Website");
            Map(m => m.Description).Name("Description");
            Map(m => m.DisabledAccess).Name("Disabled access");
            Map(m => m.BookingInstructions).Name("BookingInstructions");
            Map(m => m.ClaimCode).Name("ClaimCode");
        }
    }
}
