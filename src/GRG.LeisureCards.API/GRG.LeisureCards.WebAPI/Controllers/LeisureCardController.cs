using System.Web.Http;
using GRG.LeisureCards.Model;
using GRG.LeisureCards.Service;

namespace GRG.LeisureCards.WebAPI.Controllers
{
    public class LeisureCardController : ApiController
    {
        private readonly ILeisureCardService _leisureCardService;
        private readonly IUserSessionService _userSessionService;

        public LeisureCardController(ILeisureCardService leisureCardService)
        {
            _leisureCardService = leisureCardService;
            _userSessionService = UserSessionService.Instance;
        }

        [HttpGet]
        [Route("LeisureCard/Login/{code}")]
        public LeisureCardRegistrationResponse Login(string code)
        {
            var result = _leisureCardService.Login(code);

            if (result.Status == "Ok")
                result.SessionToken = _userSessionService.GetToken(result.LeisureCard);

            return result;
        }
    }
}
