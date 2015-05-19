using System.Collections.Generic;
using GRG.LeisureCards.WebAPI.Model;
using NUnit.Framework;
using RestSharp;

namespace GRG.LeisureCards.API.IntegrationTests
{
    [TestFixture]
    public class TwoForControllerTests : ControllerTests
    {
        [Test]
        public void GetAll()
        {
            var client = new RestClient(Config.BaseAddress);

            var request = new RestRequest("TwoForOne/GetAll", Method.GET);
            request.AddHeader("accepts", "application/json");
            request.AddHeader("SessionToken", Config.GetSessionToken());

            Assert.IsNotNull(client.Execute<List<TwoForOneOffer>>(request).Data.Count>0);
        }

        [Test]
        public void LocationSearch()
        {
            var client = new RestClient(Config.BaseAddress);

            var request = new RestRequest("TwoForOne/FindByLocation/{postCodeOrTown}/{radiusMiles}", Method.GET);
            request.AddHeader("accepts", "application/json");
            request.AddParameter("postCodeOrTown", "southampton");
            request.AddParameter("radiusMiles", 10);
            request.AddHeader("SessionToken", Config.GetSessionToken());

            Assert.IsNotNull(client.Execute<List<TwoForOneOfferGeoSearchResult>>(request).Data.Count > 0);
        }
    }
}