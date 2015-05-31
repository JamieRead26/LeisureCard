using System;
using GRG.leisureCards.WebAPI.ClientContract;
using GRG.LeisureCards.WebAPI.Model;
using RestSharp;

namespace GRG.LeisureCards.WebAPI.Client
{
    public class LeisureCardService : ILeisureCardService
    {
        private readonly string _baseUrl;
        private readonly string _sessionToken;

        public LeisureCardService(string baseUrl, string sessionToken)
        {
            _baseUrl = baseUrl;
            _sessionToken = sessionToken;
        }

        public CardUpdateResponse Update(string codeOrRef, DateTime renewalDateTime, bool suspended)
        {
            var client = new RestClient(_baseUrl);

            var request = new RestRequest("LeisureCard/Update/{cardNumberOrRef}/{renewalDate}/{suspended]", Method.GET);
            request.AddParameter("cardNumberOrRef", codeOrRef);
            request.AddParameter("renewalDate", renewalDateTime);
            request.AddParameter("suspended", suspended);
            request.AddHeader("accepts", "application/json");
            request.AddHeader("SessionToken", _sessionToken);

            return client.Execute<CardUpdateResponse>(request).Data;
        }

        public SessionInfo GetSessionInfo()
        {
            var client = new RestClient(_baseUrl);

            var request = new RestRequest("LeisureCard/GetSessionInfo", Method.GET);
            request.AddHeader("accepts", "application/json");
            request.AddHeader("SessionToken", _sessionToken);

            return client.Execute<SessionInfo>(request).Data;
        }

        public CardGenerationResponse GenerateCards(string reference, int numOfcards, int renewalPeriodMonths)
        {
            var client = new RestClient(_baseUrl);

            var request = new RestRequest("LeisureCard/GenerateCards/{reference}/{numberOfCards}/{renewalPeriodMonths}", Method.GET);
            request.AddParameter("reference", reference);
            request.AddParameter("numberOfCards", numOfcards);
            request.AddParameter("renewalPeriodMonths", renewalPeriodMonths);
            request.AddHeader("accepts", "application/json");
            request.AddHeader("SessionToken", _sessionToken);

            return client.Execute<CardGenerationResponse>(request).Data;
        }
    }
}
