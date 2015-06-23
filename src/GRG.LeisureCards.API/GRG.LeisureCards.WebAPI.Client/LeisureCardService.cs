using System;
using GRG.LeisureCards.WebAPI.ClientContract;
using GRG.LeisureCards.WebAPI.Model;
using RestSharp;

namespace GRG.LeisureCards.WebAPI.Client
{
    public class LeisureCardService : Service, ILeisureCardService
    {
        public LeisureCardService(string baseUrl, string sessionToken) : base(baseUrl, sessionToken)
        {}

        public CardUpdateResponse Update(string codeOrRef, DateTime renewalDate, bool suspended)
        {
            var request = GetRestRequest("LeisureCard/Update/{cardNumberOrRef}/{renewalDate}/{suspended}", Method.GET);
           
            request.AddParameter("cardNumberOrRef", codeOrRef);
            request.AddParameter("renewalDate", renewalDate);
            request.AddParameter("suspended", suspended);
            
            return new RestClient(BaseUrl).Execute<CardUpdateResponse>(request).Data;
        }

        public SessionInfo GetSessionInfo()
        {
            return new RestClient(BaseUrl).Execute<SessionInfo>(GetRestRequest("LeisureCard/GetSessionInfo", Method.GET)).Data;
        }

        public CardGenerationResponse GenerateCards(string reference, int numOfcards, int renewalPeriodMonths)
        {
            var request = GetRestRequest("LeisureCard/GenerateCards/{reference}/{numberOfCards}/{renewalPeriodMonths}", Method.GET);
            request.AddParameter("reference", reference);
            request.AddParameter("numberOfCards", numOfcards);
            request.AddParameter("renewalPeriodMonths", renewalPeriodMonths);

            return new RestClient(BaseUrl).Execute<CardGenerationResponse>(request).Data;
        }

        public void AcceptTerms()
        {
            new RestClient(BaseUrl).Execute<CardGenerationResponse>(GetRestRequest("LeisureCard/AcceptTerms", Method.GET));
        }
    }
}
