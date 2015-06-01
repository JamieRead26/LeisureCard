using System.Collections.Generic;
using GRG.LeisureCards.WebAPI.ClientContract;
using GRG.LeisureCards.WebAPI.Model;
using RestSharp;

namespace GRG.LeisureCards.WebAPI.Client
{
    public class TwoForOneService : Service, ITwoForOneService
    {
        public TwoForOneService(string baseUrl, string sessionToken) : base(baseUrl, sessionToken)
        {
        }

        public IEnumerable<TwoForOneOffer> GetAll()
        {
            return
                new RestClient(BaseUrl).Execute<List<TwoForOneOffer>>(GetRestRequest("TwoForOne/GetAll", Method.GET))
                    .Data;
        }

        public IEnumerable<TwoForOneOfferGeoSearchResult> FindByLocation(string townOrPostcode, int radiusMiles)
        {
            var request = GetRestRequest("TwoForOne/FindByLocation/{postCodeOrTown}/{radiusMiles}", Method.GET);
            request.AddParameter("postCodeOrTown", townOrPostcode);
            request.AddParameter("radiusMiles", radiusMiles);

            return new RestClient(BaseUrl).Execute<List<TwoForOneOfferGeoSearchResult>>(request).Data;
        }
    }
}
