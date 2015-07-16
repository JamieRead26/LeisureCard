using System.Collections.Generic;
using GRG.LeisureCards.WebAPI.Model;
using RestSharp;

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

        protected RestRequest GetRestRequest(string url, Method method)
        {
            var request = new RestRequest(url, method) {RequestFormat = DataFormat.Json};
            
            request.AddHeader("accepts", "application/json");
            request.AddHeader("SessionToken", SessionToken);
           // request.AddHeader("Host", new Uri(BaseUrl).Host);

            return request;
        }
    }
}