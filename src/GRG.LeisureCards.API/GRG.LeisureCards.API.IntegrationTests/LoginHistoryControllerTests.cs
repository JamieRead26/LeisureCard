using NUnit.Framework;

namespace GRG.LeisureCards.API.IntegrationTests
{
    [TestFixture]
    public class LoginHistoryControllerTests : ControllerTests
    {
        

       /* [Test]

        public void Page()
        {
            var client = new RestClient(Config.BaseAddress);
            var toId = 0;

            for (var i = 0; i < 3   ; i++)
            {
                var url = string.Format("LoginHistory/Get/5/{0}", toId);

                var request = new RestRequest(url, Method.GET);
                request.AddHeader("accepts", "application/json");
                request.AddHeader("SessionToken", Config.GetAdminSessionToken());

                var response = client.Execute(request);
                var history = JsonConvert.DeserializeObject<List<LeisureCardUsage>>(response.Content);
            
                Assert.AreEqual(5, history.Count);
                Assert.IsTrue(history.FirstOrDefault().Id>history.LastOrDefault().Id);

                if (toId>0)
                    Assert.IsTrue(history.FirstOrDefault().Id<toId);

                toId = history.FirstOrDefault().Id;
            }
        }*/
    }
}
