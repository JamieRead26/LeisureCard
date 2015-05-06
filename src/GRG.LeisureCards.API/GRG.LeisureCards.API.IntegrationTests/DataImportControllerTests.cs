using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using GRG.LeisureCards.Data;
using GRG.LeisureCards.Model;
using GRG.LeisureCards.Persistence.NHibernate.ClassMaps;
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
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("GRG.LeisureCards.API.IntegrationTests.RedLetter_Products.xml");
            var memStream = new MemoryStream();
            stream.CopyTo(memStream);
            var base64 = Convert.ToBase64String(memStream.ToArray());

            var client = new RestClient(Config.BaseAddress);

            var request = new RestRequest("DataImport/RedLetter/", Method.POST);
            request.AddParameter("", base64);
            request.AddHeader("accepts", "application/json");
            request.AddHeader("AdminCode", "12345-54321");

            var response = client.Execute<DataImportJournalEntry>(request);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("Red Letter Offers", response.Data.Key);
        }
    }
}
