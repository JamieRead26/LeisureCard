using GRG.LeisureCards.Model;
using NUnit.Framework;
using RestSharp;

namespace GRG.LeisureCards.API.IntegrationTests
{
    [TestFixture]
    public class TestControllerTests
    {
        [Test]
        public void TestUnitOfwork()
        {
            var client = new RestClient(Config.BaseAddress);

            var request = new RestRequest("Test/UOW", Method.GET);
            request.AddHeader("accepts", "application/json");

            var response = client.Execute(request).Content;

            Assert.AreEqual(true, bool.Parse(response.Replace("\"", "")));
        }
    }
}
