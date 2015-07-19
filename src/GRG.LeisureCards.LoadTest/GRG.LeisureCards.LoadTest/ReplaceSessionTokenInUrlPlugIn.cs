using Microsoft.VisualStudio.TestTools.WebTesting;

namespace GRG.LeisureCards.LoadTest
{
    public class ReplaceSessionTokenInUrlPlugIn : WebTestPlugin
    {
        public override void PreRequestDataBinding(object sender, PreRequestDataBindingEventArgs e)
        {
            object sessionToken;
            if (e.WebTest.Context.TryGetValue("SessionToken", out sessionToken) &&
                e.Request.Url.Contains("pdf/241voucher"))
                e.Request.Url = e.Request.Url.Substring(0, e.Request.Url.LastIndexOf("/")) + "/" + (string) sessionToken;
        }
    }
}
