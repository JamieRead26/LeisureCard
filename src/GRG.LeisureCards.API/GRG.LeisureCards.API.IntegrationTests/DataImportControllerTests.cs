﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using GRG.LeisureCards.Data;
using GRG.LeisureCards.Model;
using GRG.LeisureCards.Persistence.NHibernate.ClassMaps;
using GRG.LeisureCards.TestResources;
using Newtonsoft.Json;
using NHibernate.Util;
using NUnit.Framework;
using RestSharp;

namespace GRG.LeisureCards.API.IntegrationTests
{
    [TestFixture]
    public class DataImportControllerTests
    {
        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            DataBootstrap.PrepDb(Assembly.GetAssembly(typeof(LeisureCardClassMap)));
        }

        [Test]
        public void UploadRedLetterData()
        {
            using (var dataStream = ResourceStreams.GetRedLetterDataStream())
            using (var memStream = new MemoryStream())
            {
                dataStream.CopyTo(memStream);

                var base64 = Convert.ToBase64String(memStream.ToArray());

                var client = new RestClient(Config.BaseAddress);

                var request = new RestRequest("DataImport/RedLetter/", Method.POST);
                request.AddParameter("", base64);
                request.AddHeader("accepts", "application/json");
                request.AddHeader("AdminCode", "12345-54321");

                var response = client.Execute<DataImportJournalEntry>(request);

                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                Assert.AreEqual(DataImportKey.RedLetter.Key, response.Data.Key);
                Assert.IsTrue(response.Data.Success);

                request = new RestRequest("DataImport/GetRedLetterImportJournal/{count}/{toId}", Method.GET);
                request.AddParameter("count", 10);
                request.AddParameter("toId", 0);
                request.AddHeader("accepts", "application/json");
                request.AddHeader("AdminCode", "12345-54321");

                var content = client.Execute(request).Content;

                Assert.AreEqual(response.Data.Id,JsonConvert.DeserializeObject<List<DataImportJournalEntry>>(content).FirstOrDefault().Id);
            }
        }

        [Test]
        public void Upload241Data()
        {
            using (var dataStream = ResourceStreams.Get241LetterDataStream())
            using (var memStream = new MemoryStream())
            {
                dataStream.CopyTo(memStream);

                var base64 = Convert.ToBase64String(memStream.ToArray());

                var client = new RestClient(Config.BaseAddress);

                var request = new RestRequest("DataImport/TwoForOne/", Method.POST);
                request.AddParameter("", base64);
                request.AddHeader("accepts", "application/json");
                request.AddHeader("AdminCode", "12345-54321");

                var response = client.Execute<DataImportJournalEntry>(request);

                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                Assert.AreEqual(DataImportKey.TwoForOne.Key, response.Data.Key);
                Assert.IsTrue(response.Data.Success);

                request = new RestRequest("DataImport/GetTwoForOneImportJournal/{count}/{toId}", Method.GET);
                request.AddParameter("count", 10);
                request.AddParameter("toId", 0);
                request.AddHeader("accepts", "application/json");
                request.AddHeader("AdminCode", "12345-54321");

                var content = client.Execute(request).Content;

                Assert.AreEqual(response.Data.Id, JsonConvert.DeserializeObject<List<DataImportJournalEntry>>(content).FirstOrDefault().Id);
            }
        }
    }
}
