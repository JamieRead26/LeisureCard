using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using GRG.LeisureCards.DomainModel;
using GRG.LeisureCards.Persistence;
using GRG.LeisureCards.Persistence.NHibernate;
using GRG.LeisureCards.Service;
using GRG.LeisureCards.WebAPI.Authentication;
using GRG.LeisureCards.WebAPI.Filters;
using GRG.LeisureCards.WebAPI.Model;
using CardGenerationLog = GRG.LeisureCards.DomainModel.CardGenerationLog;
using LeisureCardRegistrationResponse = GRG.LeisureCards.Service.LeisureCardRegistrationResponse;
using LeisureCardUsage = GRG.LeisureCards.DomainModel.LeisureCardUsage;
using SessionInfo = GRG.LeisureCards.DomainModel.SessionInfo;

namespace GRG.LeisureCards.WebAPI.Controllers
{
    public class LeisureCardController : ApiController
    {
        private readonly ILeisureCardService _leisureCardService;
        private readonly ILeisureCardRepository _leisureCardRepository;
        private readonly IUserSessionService _userSessionService;

        public LeisureCardController(ILeisureCardService leisureCardService, 
            ILeisureCardRepository leisureCardRepository)
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
                    SessionToken = _userSessionService.GetToken(result.LeisureCard),
                    IsAdmin = result.LeisureCard == AdminLeisureCard.Instance
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


        [HttpGet]
        [SessionAuthFilter(true)]
        [Route("LeisureCard/Update/{cardNumberOrRef}/{expiryDate}/{renewalDate}/{suspended}")]
        [UnitOfWork]
        public CardUpdateResponse Update(string cardNumberOrRef, DateTime? expiryDate, DateTime? renewalDate, bool suspended)
        {
            var crd = _leisureCardRepository.Get(cardNumberOrRef);
            var cards = crd == null ? _leisureCardRepository.GetByRef(cardNumberOrRef) : new[] { crd };

            foreach (var card in cards)
            {
                card.ExpiryDate = expiryDate;
                card.RenewalDate = renewalDate;
                card.Suspended = suspended;

                _leisureCardRepository.SaveOrUpdate(card);
            }

            return new CardUpdateResponse{CardsUpdated = cards.Count()};
        }

        [HttpGet]
        [SessionAuthFilter(true)]
        [Route("LeisureCard/GetAllCardNumbers")]
        public List<LeisureCardInfo> GetAllCardNumbers()
        {
            return _leisureCardRepository.GetAll().Select(c => new LeisureCardInfo(c)).ToList();
        }

        [HttpGet]
        [SessionAuthFilter(true)]
        [Route("LeisureCard/GenerateCards/{reference}/{numberOfCards}/{renewalPeriodMonths}")]
        public CardGenerationLog GenerateCards(string reference, int numberOfCards, int renewalPeriodMonths)
        {
            return _leisureCardService.GenerateCards(reference, numberOfCards, renewalPeriodMonths);
        }
    }
}
