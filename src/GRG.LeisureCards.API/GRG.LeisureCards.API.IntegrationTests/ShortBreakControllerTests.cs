﻿using System.Net;
using NUnit.Framework;
using RestSharp;

namespace GRG.LeisureCards.API.IntegrationTests
{
    [TestFixture]
    public class ShortBreakControllerTests : ControllerTests
    {
        [Test]
        public void ClaimOffer()
        {
            var client = new RestClient(Config.BaseAddress);

            var request = new RestRequest("ShortBreaks/ClaimOffer/{title}", Method.GET);
            request.AddHeader("accepts", "application/json");
            request.AddHeader("title", "cottages");
            request.AddHeader("SessionToken", Config.GetSessionToken());

            Assert.AreEqual(HttpStatusCode.OK, client.Execute(request).StatusCode);

        }
    }
}
