﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using GRG.LeisureCards.TestResources;
using GRG.LeisureCards.WebAPI.Model;
using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using DataImportJournalEntry = GRG.LeisureCards.WebAPI.Model.DataImportJournalEntry;

namespace GRG.LeisureCards.API.IntegrationTests
{
    [TestFixture]
    public class DataImportControllerTests : ControllerTests
    {
        [Test]
        public void GetLastGoodRedLetterImportJournal()
        {
            var client = new RestClient(Config.BaseAddress);

            var request = new RestRequest("DataImport/GetLastGoodRedLetterImportJournal/", Method.GET);
            request.AddHeader("accepts", "application/json");
            request.AddHeader("SessionToken", Config.GetAdminSessionToken());

            var result = client.Execute<DataImportJournalEntry>(request);
            Assert.AreEqual("RedLetter", result.Data.UploadKey);
        }

        [Test]
        public void GetLastBadRedLetterImportJournal()
        {
            var client = new RestClient(Config.BaseAddress);

            var request = new RestRequest("DataImport/GetLastBadRedLetterImportJournal/", Method.GET);
            request.AddHeader("accepts", "application/json");
            request.AddHeader("SessionToken", Config.GetAdminSessionToken());

            var result = client.Execute<DataImportJournalEntry>(request);
            Assert.IsNull(result.Data);
        }

        [Test]
        public void GetLastGoodTwoForOneImportJournal()
        {
            var client = new RestClient(Config.BaseAddress);

            var request = new RestRequest("DataImport/GetLastGoodTwoForOneImportJournal/", Method.GET);
            request.AddHeader("accepts", "application/json");
            request.AddHeader("SessionToken", Config.GetAdminSessionToken());

            var result = client.Execute<DataImportJournalEntry>(request);
            Assert.AreEqual("241", result.Data.UploadKey);
        }

        [Test]
        public void GetLastBadTwoForOneImportJournal()
        {
            var client = new RestClient(Config.BaseAddress);

            var request = new RestRequest("DataImport/GetLastBadTwoForOneImportJournal/", Method.GET);
            request.AddHeader("accepts", "application/json");
            request.AddHeader("SessionToken", Config.GetAdminSessionToken());

            var result = client.Execute<DataImportJournalEntry>(request);
            Assert.IsNull(result.Data);
        }

        [Test]
        public void UploadRedLetterData()
        {
            var sessionToken = Config.GetAdminSessionToken();

            var client = new RestClient(Config.BaseAddress);

            var request = new RestRequest("DataImport/RedLetter/", Method.POST);
            request.AddHeader("accepts", "application/json");
            request.AddHeader("SessionToken", sessionToken);

            var response = client.Execute<DataImportJournalEntry>(request);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("RedLetter", response.Data.UploadKey);
            Assert.IsTrue(response.Data.Success);

            request = new RestRequest("DataImport/GetRedLetterImportJournal/{count}/{toId}", Method.GET);
            request.AddParameter("count", 10);
            request.AddParameter("toId", 0);
            request.AddHeader("accepts", "application/json");
            request.AddHeader("SessionToken", sessionToken);

            var content = client.Execute(request).Content;

            Assert.AreEqual(response.Data.Id, JsonConvert.DeserializeObject<List<DataImportJournalEntry>>(content).FirstOrDefault().Id);

            request = new RestRequest("DataImport/GetLastGoodRedLetterImportJournal", Method.GET);
            request.AddHeader("accepts", "application/json");
            request.AddHeader("SessionToken", sessionToken);

            var lastKnownGood = client.Execute<DataImportJournalEntry>(request);

            Assert.AreEqual(response.Data.Id, lastKnownGood.Data.Id);
            Assert.AreEqual(response.Data.FileKey, lastKnownGood.Data.FileKey);
        }

        //[Test]
        //public void UploadLeisureCardData()
        //{
        //    using (var dataStream = ResourceStreams.GetLeisureCardStream())
        //    using (var memStream = new MemoryStream())
        //    {
        //        dataStream.CopyTo(memStream);
        //        var fileBytes = memStream.ToArray();
        //        var sessionToken = Config.GetAdminSessionToken();

        //        var client = new RestClient(Config.BaseAddress);

        //        dataStream.Position = 0;
        //        var request = new RestRequest("DataImport/LeisureCards/", Method.POST);
        //        request.AddFile("LeisureCards.csv", fileBytes, "LeisureCards.csv");
        //        request.AddHeader("accepts", "application/json");
        //        request.AddHeader("SessionToken", sessionToken);

        //        var response = client.Execute<DataImportJournalEntry>(request);

        //        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        //        Assert.AreEqual(DataImportKey.LeisureCards.Key, response.Data.UploadKey);
        //        Assert.IsTrue(response.Data.Success);

        //        request = new RestRequest("DataImport/GetLeisureCardImportJournal/{count}/{toId}", Method.GET);
        //        request.AddParameter("count", 10);
        //        request.AddParameter("toId", 0);
        //        request.AddHeader("accepts", "application/json");
        //        request.AddHeader("SessionToken", sessionToken);

        //        var content = client.Execute(request).Content;

        //        Assert.AreEqual(response.Data.Id, JsonConvert.DeserializeObject<List<DataImportJournalEntry>>(content).FirstOrDefault().Id);

        //        request = new RestRequest("DataImport/GetLastGoodLeisureCardImportJournal", Method.GET);
        //        request.AddHeader("accepts", "application/json");
        //        request.AddHeader("SessionToken", sessionToken);

        //        var lastKnownGood = client.Execute<DataImportJournalEntry>(request);

        //        Assert.AreEqual(response.Data.Id, lastKnownGood.Data.Id);
        //        Assert.AreEqual(response.Data.FileKey, lastKnownGood.Data.FileKey);
        //    }
        //}

        //[Test]
        //public void UploadRedLetterData_Fail()
        //{
        //    using (var dataStream = ResourceStreams.GetRedLetterBadDataStream())
        //    using (var memStream = new MemoryStream())
        //    {
        //        dataStream.CopyTo(memStream);
        //        var fileBytes = memStream.ToArray();
        //        var sessionToken = Config.GetAdminSessionToken();

        //        var client = new RestClient(Config.BaseAddress);

        //        dataStream.Position = 0;
        //        var request = new RestRequest("DataImport/RedLetter/", Method.POST);
        //        request.AddFile("RedLetterData.csv", fileBytes, "RedLetterData.csv");
        //        request.AddHeader("accepts", "application/json");
        //        request.AddHeader("SessionToken", sessionToken);

        //        var response = client.Execute<DataImportJournalEntry>(request);

        //        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        //        Assert.AreEqual("RedLetter", response.Data.UploadKey);
        //        Assert.IsFalse(response.Data.Success);

        //        request = new RestRequest("DataImport/GetRedLetterImportJournal/{count}/{toId}", Method.GET);
        //        request.AddParameter("count", 10);
        //        request.AddParameter("toId", 0);
        //        request.AddHeader("accepts", "application/json");
        //        request.AddHeader("SessionToken", sessionToken);

        //        var content = client.Execute(request).Content;

        //        Assert.AreEqual(response.Data.Id, JsonConvert.DeserializeObject<List<DataImportJournalEntry>>(content).FirstOrDefault().Id);

        //        request = new RestRequest("DataImport/GetLastBadRedLetterImportJournal", Method.GET);
        //        request.AddHeader("accepts", "application/json");
        //        request.AddHeader("SessionToken", sessionToken);

        //        var lastKnownBad = client.Execute<DataImportJournalEntry>(request);

        //        Assert.AreEqual(response.Data.Id, lastKnownBad.Data.Id);
        //        Assert.AreEqual(response.Data.FileKey, lastKnownBad.Data.FileKey);
        //        Assert.AreEqual(response.Data.UploadKey, lastKnownBad.Data.UploadKey);
        //    }
        //}

        [Test]
        public void Upload241Data()
        {
            using (var dataStream = ResourceStreams.Get241LetterDataStream())
            using (var memStream = new MemoryStream())
            {
                dataStream.CopyTo(memStream);
                var fileBytes = memStream.ToArray();
                var sessionToken = Config.GetAdminSessionToken();

                var client = new RestClient(Config.BaseAddress);

                dataStream.Position = 0;
                var request = new RestRequest("DataImport/TwoForOne/", Method.POST);
                request.AddFile("241Dta.csv", fileBytes, "241Dta.csv");
                request.AddHeader("accepts", "application/json");
                request.AddHeader("SessionToken", sessionToken);

                var response = client.Execute<DataImportJournalEntry>(request);

                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                Assert.AreEqual("241", response.Data.UploadKey);
                Assert.IsTrue(response.Data.Success);

                request = new RestRequest("DataImport/GetTwoForOneImportJournal/{count}/{toId}", Method.GET);
                request.AddParameter("count", 10);
                request.AddParameter("toId", 0);
                request.AddHeader("accepts", "application/json");
                request.AddHeader("SessionToken", sessionToken);

                var reponse = client.Execute(request);

                Assert.AreEqual(response.Data.Id, JsonConvert.DeserializeObject<List<DataImportJournalEntry>>(reponse.Content).FirstOrDefault().Id);

                request = new RestRequest("DataImport/GetLastGoodTwoForOneImportJournal", Method.GET);
                request.AddHeader("accepts", "application/json");
                request.AddHeader("SessionToken", sessionToken);

                var lastKnownGood = client.Execute<DataImportJournalEntry>(request);

                Assert.AreEqual(response.Data.Id, lastKnownGood.Data.Id);
                Assert.AreEqual(response.Data.FileKey, lastKnownGood.Data.FileKey);

                request = new RestRequest("TwoForOne/FindByLocation/{postCodeOrTown}/{radiusMiles}", Method.GET);
                request.AddHeader("accepts", "application/json");
                request.AddParameter("radiusMiles", 1);
                request.AddParameter("postCodeOrTown", "SK12 1BY");
                request.AddHeader("SessionToken", sessionToken);

                reponse = client.Execute(request);
                var results = JsonConvert.DeserializeObject<List<TwoForOneOfferGeoSearchResult>>(reponse.Content);

                Assert.AreEqual(1, results.Count);
                Assert.AreEqual("SK12 1BY", results.First().TwoForOneOffer.PostCode);
                Assert.AreEqual(0, results.First().Distance);
            }
        }
    }
}
