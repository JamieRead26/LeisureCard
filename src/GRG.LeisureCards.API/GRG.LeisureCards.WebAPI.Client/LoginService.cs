using GRG.LeisureCards.WebAPI.ClientContract;
using GRG.LeisureCards.WebAPI.Model;
using RestSharp;

namespace GRG.LeisureCards.WebAPI.Client
{
    public class LoginService : ILoginService
    {
        private readonly string _baseurl;

        public LoginService(string baseurl)
        {
            _baseurl = baseurl;
        }

        public LeisureCardRegistrationResponse Login(string code, string tenantKey, out ISession session)
        {
            var request = new RestRequest("LeisureCard/Login/{code}/{tenantKey}", Method.GET);
            request.AddParameter("code", code);
            request.AddParameter("tenantKey", tenantKey);
            request.AddHeader("accepts", "application/json");

            var response = new RestClient(_baseurl).Execute<LeisureCardRegistrationResponse>(request);

            session = response.Data.Status.ToUpper() == "OK" ? new Session(_baseurl, response.Data.SessionInfo) : null;

            return response.Data;
        }
    }
}
