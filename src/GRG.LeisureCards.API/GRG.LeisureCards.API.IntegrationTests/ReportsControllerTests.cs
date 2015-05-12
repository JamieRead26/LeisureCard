using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GRG.LeisureCards.Model;
using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;

namespace GRG.LeisureCards.API.IntegrationTests
{
    [TestFixture]
    public class ReportsControllerTests
    {
        [Test]
        public void FindByKeyword()
        {
            var client = new RestClient(Config.BaseAddress);

            var request = new RestRequest("Reports/GetLoginHistory/{from}/{to}", Method.GET);
            request.AddParameter("from", new DateTime(2000,1,1));
            request.AddParameter("to", new DateTime(2001,1,1));
            request.AddHeader("accepts", "application/json");
            request.AddHeader("AdminCode", "12345-54321");
            request.AddHeader("SessionToken", Config.GetAdminSessionToken());

            var response = client.Execute(request);
            var history = JsonConvert.DeserializeObject<List<LeisureCardUsage>>(response.Content);

            Assert.IsTrue(history.Count>1);
            Assert.IsTrue(history.FirstOrDefault().LoginDateTime>history.LastOrDefault().LoginDateTime);
        }
    }
}
