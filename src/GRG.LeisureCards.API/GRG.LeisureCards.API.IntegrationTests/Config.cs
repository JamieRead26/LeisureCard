using GRG.LeisureCards.Model;
using NUnit.Framework;
using RestSharp;

namespace GRG.LeisureCards.API.IntegrationTests
{
    public static class Config
    {
        public const string BaseAddress = "http://LeisureCard";//"http://localhost:1623";// "http://52.17.232.144:1623/";//  // "http://52.17.166.61/LeisureCardAPI"; 

        public static string GetSessionToken()
        {
            return GetSessionToken("Registered1");
        }

        public static string GetSessionToken(string code)
        {
            var client = new RestClient(BaseAddress);

            var request = new RestRequest("LeisureCard/Login/{code}", Method.GET);
            request.AddParameter("code", code);
            request.AddHeader("accepts", "application/json");

            var response = client.Execute<LeisureCardRegistrationResponse>(request);

            Assert.AreEqual("Ok", response.Data.Status);

            return response.Data.SessionInfo.SessionToken;
        }

        public static string GetAdminSessionToken()
        {
            return GetSessionToken("Admin");
        }
    }
}
