using GRG.LeisureCards.WebAPI.ClientContract;
using RestSharp;

namespace GRG.LeisureCards.WebAPI.Client
{
    public class ShortBreakService : Service, IShortBreakService
    {
        public ShortBreakService(string baseUrl, string sessionToken) : base(baseUrl, sessionToken)
        {
        }

        public void ClaimOffer(string title)
        {
            var request = GetRestRequest("ShortBreaks/ClaimOffer/{title}", Method.GET);
            request.AddHeader("title", title);

            new RestClient(BaseUrl).Execute(request);
        }
    }
}
