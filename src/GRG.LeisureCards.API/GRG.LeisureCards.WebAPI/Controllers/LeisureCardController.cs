using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using GRG.LeisureCards.Model;
using GRG.LeisureCards.Persistence;
using GRG.LeisureCards.Service;
using GRG.LeisureCards.WebAPI.Authentication;
using GRG.LeisureCards.WebAPI.Filters;

namespace GRG.LeisureCards.WebAPI.Controllers
{
    public class LeisureCardController : ApiController
    {
        private readonly ILeisureCardService _leisureCardService;
        private readonly ILeisureCardRepository _leisureCardRepository;
        private readonly IUserSessionService _userSessionService;

        public LeisureCardController(ILeisureCardService leisureCardService, ILeisureCardRepository leisureCardRepository)
        {
            _leisureCardService = leisureCardService;
            _leisureCardRepository = leisureCardRepository;
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
        public void Update(string cardNumber, DateTime expiryDate, DateTime renewalDate)
        {
           
        }

        [HttpPostAttribute]
        [SessionAuthFilter()]
        [Route("LeisureCard/GetAllCardNumbers")]
        public List<string> GetAllCardNumbers()
        {
            return _leisureCardRepository.GetAll().Select(c => c.Code).ToList();
        }
    }
}
