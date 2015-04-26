using System.Reflection;
using GRG.LeisureCards.Data;
using GRG.LeisureCards.Model;
using GRG.LeisureCards.Persistence.NHibernate.ClassMaps;
using GRG.LeisureCards.WebAPI.App_Start;
using GRG.LeisureCards.WebAPI.DependencyResolution;
using Microsoft.Owin.Hosting;
using NUnit.Framework;
using OwinSelfhostSample;
using RestSharp;

namespace GRG.LeisureCards.API.IntegrationTests
{
    [TestFixture]
    public class LeisureCardControllerTests
    {
        private const string BaseAddress = "http://localhost:1623";

        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            DataBootstrap.PrepDb(Assembly.GetAssembly(typeof(LeisureCardClassMap)));

          //  WebApp.Start<Startup>(BaseAddress);
        }

        [Test]
        public void Registration_Ok()
        {
            RegistrationTest("Unregistered", "Ok");
        }

        [Test]
        public void Registration_CardAlreadyRegistered()
        {
            RegistrationTest("Registered", "CardAlreadyRegistered");
        }

        [Test]
        public void Registration_CardSuspended()
        {
            RegistrationTest("Suspended", "CardSuspended");
        }

        [Test]
        public void Registration_CardNotFound()
        {
            RegistrationTest("xxx", "CodeNotFound");
        }

        public void RegistrationTest(string code, string expectedStatus)
        {
            var client = new RestClient(BaseAddress);

            var request = new RestRequest("LeisureCard/Register/{code}", Method.GET);
            request.AddParameter("code", code);
            request.AddHeader("accepts", "application/json");

            var response = client.Execute<LeisureCardRegistrationResponse>(request);

            Assert.AreEqual(expectedStatus, response.Data.Status);
        }
    }
}
