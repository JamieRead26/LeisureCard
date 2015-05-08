using System.Collections.Generic;
using GRG.LeisureCards.Model;
using NUnit.Framework;
using RestSharp;

namespace GRG.LeisureCards.API.IntegrationTests
{
    [TestFixture]
    public class TwoForControllerTests
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
    }
}
