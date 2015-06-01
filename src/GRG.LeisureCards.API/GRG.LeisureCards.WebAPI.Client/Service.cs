namespace GRG.LeisureCards.WebAPI.Client
{
    public class Service
    {
        protected readonly string BaseUrl;
        protected readonly string SessionToken;

        protected Service(string baseUrl, string sessionToken)
        {
            BaseUrl = baseUrl;
            SessionToken = sessionToken;
        }
    }
}