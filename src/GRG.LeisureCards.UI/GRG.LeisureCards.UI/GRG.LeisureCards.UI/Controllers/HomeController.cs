using System.Web.Mvc;
using GRG.LeisureCards.UI.Content;
using GRG.LeisureCards.UI.Controllers;

namespace WebApplication1.Controllers
{
    public class HomeController : LcController
    {
        [Route("")]
        public ActionResult Index()
        {
            return Dispatch(() =>
            {
                foreach (var kvp in HtmlContent.GetContent((string) Session["tenantKey"], "footer"))
                    ViewData.Add(kvp.Key, kvp.Value);

                foreach (var kvp in HtmlContent.GetContent((string) Session["tenantKey"], "header"))
                    ViewData.Add(kvp.Key, kvp.Value);

                return View();
            });
        }

        [Route("partial/{name}")]
        public ActionResult Partial(string name)
        {
            return Dispatch(() =>
            {
                foreach (var kvp in HtmlContent.GetContent((string) Session["tenantKey"], name))
                    ViewData.Add(kvp.Key, kvp.Value);

                return View("Partials/" + name);
            });
        }
    }
}