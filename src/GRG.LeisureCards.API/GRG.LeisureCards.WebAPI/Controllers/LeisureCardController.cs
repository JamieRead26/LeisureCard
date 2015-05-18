using System;
using System.Web.Http;
using GRG.LeisureCards.Model;
using GRG.LeisureCards.Service;
using GRG.LeisureCards.WebAPI.Authentication;
using GRG.LeisureCards.WebAPI.Filters;

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
            {
                result.SessionInfo = new SessionInfo
                {
                    CardRenewalDate = result.LeisureCard.RenewalDate.Value,
                    SessionToken = _userSessionService.GetToken(result.LeisureCard)
                };

                result.LeisureCard.AddUsage(new LeisureCardUsage {LoginDateTime = DateTime.Now});
            }

            return result;
        }

        [HttpGet]
        [SessionAuthFilter]
        [Route("LeisureCard/GetSessionInfo")]
        public SessionInfo GetSessionInfo()
        {
            return ((LeisureCardPrincipal) RequestContext.Principal).SessionInfo;
        }


        [HttpPostAttribute]
        [SessionAuthFilter(true)]
        [Route("LeisureCard/Update")]
        public void Update()
        {
           
        }
    }
}
