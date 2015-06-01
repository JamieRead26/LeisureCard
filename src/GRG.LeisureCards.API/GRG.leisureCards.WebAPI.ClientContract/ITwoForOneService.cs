using System.Collections.Generic;
using GRG.LeisureCards.WebAPI.Model;

namespace GRG.LeisureCards.WebAPI.ClientContract
{
    public interface ITwoForOneService
    {
        IEnumerable<TwoForOneOffer> GetAll();
        IEnumerable<TwoForOneOfferGeoSearchResult> FindByLocation(string townOrPostcode, int radiusMiles);
    }
}
