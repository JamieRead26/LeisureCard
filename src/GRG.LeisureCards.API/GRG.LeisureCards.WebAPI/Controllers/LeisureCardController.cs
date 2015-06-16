using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using AutoMapper;
using GRG.LeisureCards.DomainModel;
using GRG.LeisureCards.Persistence;
using GRG.LeisureCards.Persistence.NHibernate;
using GRG.LeisureCards.Service;
using GRG.LeisureCards.WebAPI.Authentication;
using GRG.LeisureCards.WebAPI.Filters;
using GRG.LeisureCards.WebAPI.Model;
using NHibernate.Util;
using LeisureCard = GRG.LeisureCards.DomainModel.LeisureCard;
using SessionInfo = GRG.LeisureCards.DomainModel.SessionInfo;

namespace GRG.LeisureCards.WebAPI.Controllers
{
    public class LeisureCardController : ApiController
    {
        private readonly ILeisureCardService _leisureCardService;
        private readonly ILeisureCardRepository _leisureCardRepository;
        private readonly IUserSessionService _userSessionService;
        
        public LeisureCardController(
            ILeisureCardService leisureCardService, 
            ILeisureCardRepository leisureCardRepository,
            IUserSessionService userSessionService)
        {
            _leisureCardService = leisureCardService;
            _leisureCardRepository = leisureCardRepository;
            _userSessionService = userSessionService;
        }

        [HttpGet]
        [Route("LeisureCard/Login/{code}")]
        public Model.LeisureCardRegistrationResponse Login(string code)
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
            }

            var returnVal = Mapper.Map<Model.LeisureCardRegistrationResponse>(result);

            return returnVal;
        }

        [HttpGet]
        [SessionAuthFilter]
        [Route("LeisureCard/GetSessionInfo")]
        public Model.SessionInfo GetSessionInfo()
        {
            return Mapper.Map<Model.SessionInfo>(((LeisureCardPrincipal) RequestContext.Principal).SessionInfo);
        }

        [HttpGet]
        [SessionAuthFilter(true)]
        [Route("LeisureCard/Update/{cardNumberOrRef}/{renewalDate}/{suspended}")]
        [UnitOfWork]
        public CardUpdateResponse Update(string cardNumberOrRef, DateTime? renewalDate, bool suspended)
        {
            var crd = _leisureCardRepository.Get(cardNumberOrRef);
            var cards = crd == null ? _leisureCardRepository.GetByRef(cardNumberOrRef) : new[] { crd };

            var leisureCards = cards as LeisureCard[] ?? cards.ToArray();
            foreach (var card in leisureCards)
            {
                card.RenewalDate = renewalDate;
                card.Suspended = suspended;

                _leisureCardRepository.SaveOrUpdate(card);
            }

            return new CardUpdateResponse
            {
                CardsUpdated = leisureCards.Count(), 
                Prototype = leisureCards.Any()? Mapper.Map<Model.LeisureCard>(leisureCards[0]): null
            };
        }

        [HttpGet]
        [SessionAuthFilter(true)]
        [Route("LeisureCard/GetAllCardNumbers")]
        public IEnumerable<Model.LeisureCard> GetAllCardNumbers()
        {
            return _leisureCardRepository.GetAll().Select(Mapper.Map<Model.LeisureCard>);
        }

        [HttpGet]
        [SessionAuthFilter(true)]
        [Route("LeisureCard/GetCardNumbersForUpdate")]
        public IEnumerable<Model.LeisureCard> GetCardNumbersForUpdate()
        {
            var allCards = _leisureCardRepository.GetAll().Select(Mapper.Map<Model.LeisureCard>).ToList();

            foreach (var reference in allCards.Select(c => c.Reference).ToList().Distinct())
            {
                var card = allCards.First(c => c.Reference == reference);

                allCards.Add(new Model.LeisureCard
                {
                    Code = reference,
                    ExpiryDate = card.ExpiryDate,
                    Suspended = card.Suspended,
                    RenewalDate = card.RenewalDate,
                    Status = card.Status
                });
            }

            return allCards;
        }

        [HttpGet]
        [SessionAuthFilter(true)]
        [Route("LeisureCard/GenerateCards/{reference}/{numberOfCards}/{renewalPeriodMonths}")]
        public Model.CardGenerationResponse GenerateCards(string reference, int numberOfCards, int renewalPeriodMonths)
        {
            try
            {
                return new CardGenerationResponse
                {
                    CardGenerationLog =
                        Mapper.Map<Model.CardGenerationLog>(_leisureCardService.GenerateCards(reference, numberOfCards,
                            renewalPeriodMonths)),
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new CardGenerationResponse
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }
    }
}
