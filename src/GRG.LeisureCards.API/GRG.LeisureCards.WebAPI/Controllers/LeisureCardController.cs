using System.Web.Http;
using GRG.LeisureCards.Model;
using GRG.LeisureCards.Service;

namespace GRG.LeisureCards.WebAPI.Controllers
{
    public class LeisureCardController : ApiController
    {
        private readonly ILeisureCardService _leisureCardService;

        public LeisureCardController(ILeisureCardService leisureCardService)
        {
            _leisureCardService = leisureCardService;
        }
        
        [HttpGet]
        [Route("LeisureCard/Register/{code}")]
        public LeisureCardRegistrationResponse Register(string code)
        {
            return _leisureCardService.Register(code);
        }
    }
}
