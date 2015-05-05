using System.Collections.Generic;
using System.Net;
using System.Reflection;
using GRG.LeisureCards.Data;
using GRG.LeisureCards.Model;
using GRG.LeisureCards.Persistence.NHibernate.ClassMaps;
using NUnit.Framework;
using RestSharp;

namespace GRG.LeisureCards.API.IntegrationTests
{
    [TestFixture]
    public class RedLetterControllerTests
    {
        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            DataBootstrap.PrepDb(Assembly.GetAssembly(typeof(LeisureCardClassMap)));
        }


        [Test]
        public void FindByKeyword_401()
        {
            var client = new RestClient(Config.BaseAddress);

            var request = new RestRequest("RedLetter/Find/{keyword}", Method.GET);
            request.AddParameter("keyword", "amphibious");
            request.AddHeader("accepts", "application/json");

            var response = client.Execute<List<RedLetterProductSummary>>(request);

            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Test]
        public void FindByKeyword()
        {
            var client = new RestClient(Config.BaseAddress);

            var request = new RestRequest("RedLetter/Find/{keyword}", Method.GET);
            request.AddParameter("keyword", "amphibious");
            request.AddHeader("accepts", "application/json");
            request.AddHeader("SessionToken", GetSessionToken());

            var response = client.Execute<List<RedLetterProductSummary>>(request);

            Assert.AreEqual(1, response.Data.Count);
        }

        [Test]
        public void GetById()
        {
            var client = new RestClient(Config.BaseAddress);

            var request = new RestRequest("RedLetter/Get/{id}", Method.GET);
            request.AddParameter("id", "5850");
            request.AddHeader("accepts", "application/json");
            request.AddHeader("SessionToken", GetSessionToken());

            Assert.IsNotNull(client.Execute<RedLetterProductSummary>(request));
        }

        public string GetSessionToken()
        {
            var client = new RestClient(Config.BaseAddress);

            var request = new RestRequest("LeisureCard/Login/{code}", Method.GET);
            request.AddParameter("code", "Registered");
            request.AddHeader("accepts", "application/json");

            var response = client.Execute<LeisureCardRegistrationResponse>(request);

            Assert.AreEqual("Ok", response.Data.Status);

            return response.Data.SessionToken;
        }
    }
}
