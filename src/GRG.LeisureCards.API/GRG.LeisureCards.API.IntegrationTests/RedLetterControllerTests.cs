using System.Collections.Generic;
using System.Linq;
using System.Net;
using GRG.LeisureCards.WebAPI.Model;
using Newtonsoft.Json;
using NUnit.Framework;

namespace GRG.LeisureCards.API.IntegrationTests
{
    [TestFixture]
    public class RedLetterControllerTests : ControllerTests
    {
       

      /*  [Test]
        public void FindByKeyword_401()
        {
            var client = new RestClient(Config.BaseAddress);

            var request = new RestRequest("RedLetter/FindByKeyword/{keyword}", Method.GET);
            request.AddParameter("keyword", "amphibious");
            request.AddHeader("accepts", "application/json");

            var response = client.Execute<List<RedLetterProductSummary>>(request);

            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Test]
        public void FindByKeyword()
        {
            var client = new RestClient(Config.BaseAddress);

            var request = new RestRequest("RedLetter/FindByKeyword/{keyword}", Method.GET);
            request.AddParameter("keyword", "amphibious");
            request.AddHeader("accepts", "application/json");
            request.AddHeader("SessionToken", Config.GetSessionToken());

            var response = client.Execute(request);
            var products = JsonConvert.DeserializeObject<List<RedLetterProductSummary>>(response.Content);

            Assert.AreEqual(1, products.Count);
        }

        [Test]
        public void GetById()
        {
            var client = new RestClient(Config.BaseAddress);

            var request = new RestRequest("RedLetter/Get/{id}", Method.GET);
            request.AddParameter("id", "5850");
            request.AddHeader("accepts", "application/json");
            request.AddHeader("SessionToken", Config.GetSessionToken());

            var response = client.Execute<RedLetterProductSummary>(request);

            Assert.IsFalse(string.IsNullOrWhiteSpace(response.Data.Title));
        }*/

        //TODO : Fix data race
        [Test]
        public void GetRandomSpecialOffers()
        {
            var service = UserSession.GetRedLetterService();

            const int count = 3;

            var products = service.GetSpecialOffers(count);
            Assert.AreEqual(count, products.Count());
            var key1 = products.Aggregate(string.Empty, (s, summary) => s + summary.Id.ToString());

            products = service.GetSpecialOffers(count);
            Assert.AreEqual(count, products.Count());
            var key2 = products.Aggregate(string.Empty, (s, summary) => s + summary.Id.ToString());

            products = service.GetSpecialOffers(count);
            Assert.AreEqual(count, products.Count());
            var key3 = products.Aggregate(string.Empty, (s, summary) => s + summary.Id.ToString());

            Assert.IsFalse((key1==key2)&&(key1==key3)&&(key2==key3));
        }

       
    }
}
