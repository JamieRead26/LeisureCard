using System.Collections.Generic;
using GRG.LeisureCards.WebAPI.ClientContract;
using GRG.LeisureCards.WebAPI.Model;
using Newtonsoft.Json;
using RestSharp;

namespace GRG.LeisureCards.WebAPI.Client
{
    public class RedLetterService : Service, IRedLetterService
    {
        public RedLetterService(string baseUrl, string sessionToken) : base(baseUrl, sessionToken)
        {
        }

        public IEnumerable<RedLetterProductSummary> GetSpecialOffers(int count)
        {
            var request = GetRestRequest("RedLetter/GetRandomSpecialOffers/{count}", Method.GET);
            request.AddParameter("count", count);

            var response = new RestClient(BaseUrl).Execute(request);
            return JsonConvert.DeserializeObject<List<RedLetterProductSummary>>(response.Content);
        }
    }
}
