using System;
using System.Collections.Generic;
using System.Linq;
using GRG.LeisureCards.WebAPI.Model;
using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;

namespace GRG.LeisureCards.API.IntegrationTests
{
    [TestFixture]
    public class ReportsControllerTests : ControllerTests
    {
        [Test]
        public void GetLoginHistory()
        {
            var client = new RestClient(Config.BaseAddress);

            var request = new RestRequest("Reports/GetLoginHistory/{from}/{to}", Method.GET);
            request.AddParameter("from", new DateTime(2000,1,1));
            request.AddParameter("to", new DateTime(2001,1,1));
            request.AddHeader("accepts", "application/json");
            request.AddHeader("SessionToken", Config.GetAdminSessionToken());

            var response = client.Execute(request);
            var history = JsonConvert.DeserializeObject<List<WebAPI.Model.LeisureCardUsage>>(response.Content);

            Assert.IsTrue(history.Count>1);
            Assert.IsTrue(history.FirstOrDefault().LoginDateTime>history.LastOrDefault().LoginDateTime);
            Assert.IsFalse(string.IsNullOrWhiteSpace(history.FirstOrDefault().LeisureCardCode));
        }

        [Test]
        public void GetSelectedOfferHistory()
        {
            var client = new RestClient(Config.BaseAddress);

            var request = new RestRequest("Reports/GetSelectedOfferHistory/{from}/{to}", Method.GET);
            request.AddParameter("from", new DateTime(2000, 1, 1));
            request.AddParameter("to", new DateTime(2001, 1, 1));
            request.AddHeader("accepts", "application/json");
            request.AddHeader("SessionToken", Config.GetAdminSessionToken());

            var response = client.Execute(request);
            var history = JsonConvert.DeserializeObject<List<WebAPI.Model.SelectedOffer>>(response.Content);

            Assert.IsTrue(history.Count > 1);
            Assert.IsTrue(history.FirstOrDefault().SelectedDateTime > history.LastOrDefault().SelectedDateTime);
            Assert.IsFalse(string.IsNullOrWhiteSpace(history.FirstOrDefault().LeisureCardCode));
        }

        [Test]
        public void GetCardActivationHistory()
        {
            var client = new RestClient(Config.BaseAddress);

            var request = new RestRequest("Reports/GetCardActivationHistory/{from}/{to}", Method.GET);
            request.AddParameter("from", new DateTime(2000, 1, 1));
            request.AddParameter("to", new DateTime(2020, 1, 1));
            request.AddHeader("accepts", "application/json");
            request.AddHeader("SessionToken", Config.GetAdminSessionToken());

            var response = client.Execute(request).Content;
            var history = JsonConvert.DeserializeObject<List<LeisureCard>>(response);

            Assert.IsTrue(history.Count > 1);
            Assert.IsTrue(history.FirstOrDefault().RegistrationDate > history.LastOrDefault().RegistrationDate);
        }
    }
}
