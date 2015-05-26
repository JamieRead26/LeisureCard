using System;
using GRG.LeisureCards.WebAPI.Model;
using NUnit.Framework;
using RestSharp;

namespace GRG.LeisureCards.API.IntegrationTests
{
    [TestFixture]
    public class LeisureCardControllerTests : ControllerTests
    {
        [Test]
        public void Registration_Ok()
        {
            RegistrationTest("Unregistered", "Ok");
        }

        [Test]
        public void Registration_CardAlreadyRegistered()
        {
            RegistrationTest("Registered1", "Ok");
        }

        [Test]
        public void Registration_CardSuspended()
        {
            RegistrationTest("Cancelled", "CardSuspended");
        }

        [Test]
        public void Registration_CardNotFound()
        {
            RegistrationTest("xxx", "CodeNotFound");
        }

        public void RegistrationTest(string code, string expectedStatus)
        {
            var client = new RestClient(Config.BaseAddress);

            var request = new RestRequest("LeisureCard/Login/{code}", Method.GET);
            request.AddParameter("code", code);
            request.AddHeader("accepts", "application/json");
            request.AddHeader("SessionToken", Config.GetSessionToken());

            var response = client.Execute<LeisureCardRegistrationResponse>(request);

            Assert.AreEqual(expectedStatus, response.Data.Status);
        }

        [Test]
        public void Update()
        {
            var client = new RestClient(Config.BaseAddress);

            var request = new RestRequest("LeisureCard/Update/{cardNumber}/{expiryDate}/{renewalDate}", Method.GET);
            request.AddParameter("cardNumber", "Registered1");
            request.AddParameter("expiryDate", DateTime.Now+TimeSpan.FromDays(265));
            request.AddParameter("renewalDate", DateTime.Now+TimeSpan.FromDays(265));
            request.AddHeader("accepts", "application/json");
            request.AddHeader("SessionToken", Config.GetAdminSessionToken());

            var response = client.Execute<LeisureCard>(request);

            Assert.IsNotNull(response.Data);
        }

       

        [Test]
        public void GetSessionInfoTest()
        {
            var client = new RestClient(Config.BaseAddress);

            var request = new RestRequest("LeisureCard/GetSessionInfo", Method.GET);
            request.AddHeader("accepts", "application/json");
            request.AddHeader("SessionToken", Config.GetSessionToken());

            var response = client.Execute(request).Content;

            Assert.IsNotNull(client.Execute<SessionInfo>(request).Data.CardRenewalDate);
        }

       

        [Test]
        public void CardGenerationTest()
        {
            var client = new RestClient(Config.BaseAddress);

            var request = new RestRequest("LeisureCard/GenerateCards/{reference}/{numberOfCards}/{renewalPeriodMonths}", Method.GET);
            request.AddParameter("reference", "TEST");
            request.AddParameter("numberOfCards", 10);
            request.AddParameter("renewalPeriodMonths", 12);
            request.AddHeader("accepts", "application/json");
            request.AddHeader("SessionToken", Config.GetAdminSessionToken());

            var response = client.Execute<CardGenerationLog>(request);

            Assert.IsNotNull(response.Data);
            Assert.AreEqual("TEST", response.Data.Ref);
        }
    }
}
