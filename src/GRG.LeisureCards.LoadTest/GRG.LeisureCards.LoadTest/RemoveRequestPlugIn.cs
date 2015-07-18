using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.WebTesting;

namespace GRG.LeisureCards.LoadTest
{


    public class RemoveRequestPlugIn : WebTestPlugin
    {
        public override void PostRequest(object sender, PostRequestEventArgs e)
        {
            List<WebTestRequest> remove = new List<WebTestRequest>();
            foreach (WebTestRequest dependent in e.Request.DependentRequests)
            {
                if (dependent.Url.Contains("CategoryKey"))
                {
                remove.Add(dependent);
                }
            }
            foreach (WebTestRequest dependent in remove)
            {
                e.Request.DependentRequests.Remove(dependent);
            }
        }
    }
}
