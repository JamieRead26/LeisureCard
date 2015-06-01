using System;
using System.Collections.Generic;
using GRG.LeisureCards.WebAPI.ClientContract;
using GRG.LeisureCards.WebAPI.Model;
using Newtonsoft.Json;
using RestSharp;

namespace GRG.LeisureCards.WebAPI.Client
{
    public class ReportService : Service, IReportService
    {
        public ReportService(string baseUrl, string sessionToken) : base(baseUrl, sessionToken)
        {
        }

        public IEnumerable<LeisureCardUsage> GetloginHistory(DateTime @from, DateTime to)
        {
            var request = GetRestRequest("Reports/GetLoginHistory/{from}/{to}", Method.GET);
            request.AddParameter("from", from);
            request.AddParameter("to", to);

            var response =  new RestClient(BaseUrl).Execute(request);
            return JsonConvert.DeserializeObject<List<LeisureCardUsage>>(response.Content);
        }

        public IEnumerable<SelectedOffer> GetSelectedOfferHistory(DateTime from, DateTime to)
        {
            var request = GetRestRequest("Reports/GetSelectedOfferHistory/{from}/{to}", Method.GET);
            request.AddParameter("from", from);
            request.AddParameter("to", to);

            var response = new RestClient(BaseUrl).Execute(request);
            return JsonConvert.DeserializeObject<List<SelectedOffer>>(response.Content);
        }

        public IEnumerable<LeisureCard> GetCardActivationHistory(DateTime from, DateTime to)
        {
            var request = GetRestRequest("Reports/GetCardActivationHistory/{from}/{to}", Method.GET);
            request.AddParameter("from", from);
            request.AddParameter("to", to);

            var response = new RestClient(BaseUrl).Execute(request).Content;
            return JsonConvert.DeserializeObject<List<LeisureCard>>(response);
        }
    }
}
