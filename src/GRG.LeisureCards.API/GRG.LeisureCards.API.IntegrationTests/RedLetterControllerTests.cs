﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Reflection;
using GRG.LeisureCards.Data;
using GRG.LeisureCards.Model;
using GRG.LeisureCards.Persistence.NHibernate.ClassMaps;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
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
            request.AddHeader("SessionToken", GetSessionToken());

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
            request.AddHeader("SessionToken", GetSessionToken());

            var response = client.Execute<RedLetterProductSummary>(request);

            Assert.IsFalse(string.IsNullOrWhiteSpace(response.Data.Title));
        }

        [Test]
        public void GetRandomSpecialOffers()
        {
            const int count = 3;
            var client = new RestClient(Config.BaseAddress);

            var request = new RestRequest("RedLetter/GetRandomSpecialOffers/{count}", Method.GET);
            request.AddParameter("count", count);
            request.AddHeader("accepts", "application/json");
            request.AddHeader("SessionToken", GetSessionToken());

            var response = client.Execute(request);
            var products = JsonConvert.DeserializeObject<List<RedLetterProductSummary>>(response.Content);

            Assert.AreEqual(count, products.Count);
            var key1 = products.Aggregate(string.Empty, (s, summary) => s + summary.Id.ToString());

            response = client.Execute<List<RedLetterProductSummary>>(request);
            products = JsonConvert.DeserializeObject<List<RedLetterProductSummary>>(response.Content);
            Assert.AreEqual(count, products.Count);
            var key2 = products.Aggregate(string.Empty, (s, summary) => s + summary.Id.ToString());

            response = client.Execute<List<RedLetterProductSummary>>(request);
            products = JsonConvert.DeserializeObject<List<RedLetterProductSummary>>(response.Content);
            Assert.AreEqual(count, products.Count);
            var key3 = products.Aggregate(string.Empty, (s, summary) => s + summary.Id.ToString());

            Assert.IsFalse((key1==key2)&&(key1==key3)&&(key2==key3));
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
