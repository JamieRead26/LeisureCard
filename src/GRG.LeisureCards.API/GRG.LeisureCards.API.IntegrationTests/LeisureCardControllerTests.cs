﻿using System.Reflection;
using GRG.LeisureCards.Data;
using GRG.LeisureCards.Model;
using GRG.LeisureCards.Persistence.NHibernate.ClassMaps;
using NUnit.Framework;
using RestSharp;

namespace GRG.LeisureCards.API.IntegrationTests
{
    [TestFixture]
    public class LeisureCardControllerTests
    {
        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            DataBootstrap.PrepDb(Assembly.GetAssembly(typeof (LeisureCardClassMap)));
        }

        [Test]
        public void Registration_Ok()
        {
            RegistrationTest("Unregistered", "Ok");
        }

        [Test]
        public void Registration_CardAlreadyRegistered()
        {
            RegistrationTest("Registered", "Ok");
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
            var client = new RestClient(Config.BaseAddress);

            var request = new RestRequest("LeisureCard/Login/{code}", Method.GET);
            request.AddParameter("code", code);
            request.AddHeader("accepts", "application/json");
            request.AddHeader("SessionToken", Config.GetSessionToken());

            var response = client.Execute<LeisureCardRegistrationResponse>(request);

            Assert.AreEqual(expectedStatus, response.Data.Status);
        }

        [Test]
        public void GetSessionInfoTest()
        {
            var client = new RestClient(Config.BaseAddress);

            var request = new RestRequest("LeisureCard/GetSessionInfo", Method.GET);
            request.AddHeader("accepts", "application/json");
            request.AddHeader("SessionToken", Config.GetSessionToken());

            Assert.IsNotNull(client.Execute<SessionInfo>(request).Data.CardRenewalDate);
        }
    }
}
