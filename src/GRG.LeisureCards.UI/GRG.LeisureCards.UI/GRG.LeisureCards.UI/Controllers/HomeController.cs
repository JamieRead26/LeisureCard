using System.Web.Mvc;
using GRG.LeisureCards.UI.Content;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        [Route("")]
        public ActionResult Index()
        {
            foreach (var kvp in HtmlContent.GetContent((string)Session["tenantKey"], "footer"))
                ViewData.Add(kvp.Key, kvp.Value);

            foreach (var kvp in HtmlContent.GetContent((string)Session["tenantKey"], "header"))
                ViewData.Add(kvp.Key, kvp.Value); 

            return View();
        }

        [Route("partial/{name}")]
        public ActionResult Partial(string name)
        {
            foreach (var kvp in HtmlContent.GetContent((string) Session["tenantKey"], name))
                ViewData.Add(kvp.Key,kvp.Value);
            
            return View("Partials/" + name);
        }
    }
}