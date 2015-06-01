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

        public LeisureCardRegistrationResponse Login(string code, out ISession session)
        {
            var client = new RestClient(_baseurl);

            var request = new RestRequest("LeisureCard/Login/{code}", Method.GET);
            request.AddParameter("code", code);
            request.AddHeader("accepts", "application/json");

            var response = client.Execute<LeisureCardRegistrationResponse>(request);

            session = response.Data.Status.ToUpper() == "OK" ? new Session(_baseurl, response.Data.LeisureCard, response.Data.SessionInfo) : null;

            return response.Data;
        }
    }
}
