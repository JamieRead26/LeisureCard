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


        [HttpGet]
        [SessionAuthFilter(true)]
        [Route("LeisureCard/Update/{cardNumberOrRef}/{expiryDate}/{renewalDate}")]
        [UnitOfWork]
        public void Update(string cardNumberOrRef, DateTime? expiryDate, DateTime? renewalDate)
        {
            var crd = _leisureCardRepository.Get(cardNumberOrRef);
            var cards = crd == null ? _leisureCardRepository.GetByRef(cardNumberOrRef) : new[] { crd };

            if (!cards.Any())
                throw new Exception("No cards found matching ref or code");

            foreach (var card in cards)
            {
                card.ExpiryDate = expiryDate;
                card.RenewalDate = renewalDate;

                _leisureCardRepository.SaveOrUpdate(card);
            }
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
