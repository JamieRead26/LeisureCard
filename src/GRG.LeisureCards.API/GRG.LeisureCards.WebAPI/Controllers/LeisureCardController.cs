using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using AutoMapper;
using GRG.LeisureCards.DomainModel;
using GRG.LeisureCards.Persistence;
using GRG.LeisureCards.Persistence.NHibernate;
using GRG.LeisureCards.Service;
using GRG.LeisureCards.Service.BusinessLogic;
using GRG.LeisureCards.WebAPI.Authentication;
using GRG.LeisureCards.WebAPI.Filters;
using GRG.LeisureCards.WebAPI.Model;
using LeisureCard = GRG.LeisureCards.DomainModel.LeisureCard;
using SessionInfo = GRG.LeisureCards.DomainModel.SessionInfo;

namespace GRG.LeisureCards.WebAPI.Controllers
{
    public class LeisureCardController : LcApiController
    {
        private readonly ILeisureCardService _leisureCardService;
        private readonly ILeisureCardRepository _leisureCardRepository;
        private readonly IUserSessionService _userSessionService;
        private readonly ICardExpiryLogic _cardExpiryLogic;

        public LeisureCardController(
            ILeisureCardService leisureCardService, 
            ILeisureCardRepository leisureCardRepository,
            IUserSessionService userSessionService,
            ICardExpiryLogic cardExpiryLogic)
        {
            _leisureCardService = leisureCardService;
            _leisureCardRepository = leisureCardRepository;
            _userSessionService = userSessionService;
            _cardExpiryLogic = cardExpiryLogic;
        }

        [HttpGet]
        [Route("LeisureCard/Login/{code}")]
        public Model.LeisureCardRegistrationResponse Login(string code)
        {
            return Dispatch(() =>{
                var result = _leisureCardService.Login(code);

                if (result.Status == "Ok")
                {
                    result.SessionInfo = new SessionInfo
                    {
                        CardExpiryDate = result.LeisureCard.ExpiryDate.Value,
                        SessionToken = _userSessionService.GetToken(result.LeisureCard),
                        IsAdmin = result.LeisureCard == AdminLeisureCard.Instance
                    };
                }

                var returnVal = Mapper.Map<Model.LeisureCardRegistrationResponse>(result);

                return returnVal;
            });
        }

        [HttpGet]
        [SessionAuthFilter]
        [Route("LeisureCard/AcceptTerms")]
        public void AcceptTerms()
        {
            Dispatch(()=> _leisureCardService.AcceptMembershipTerms(_userSessionService.GetSession(((LeisureCardPrincipal) RequestContext.Principal).SessionInfo.SessionToken).CardCode));
        }

        [HttpGet]
        [SessionAuthFilter]
        [Route("LeisureCard/GetSessionInfo")]
        public Model.SessionInfo GetSessionInfo()
        {
            return Dispatch(()=>Mapper.Map<Model.SessionInfo>(((LeisureCardPrincipal) RequestContext.Principal).SessionInfo));
        }

        [HttpGet]
        [SessionAuthFilter(true)]
        [Route("LeisureCard/Update/{cardNumberOrRef}/{renewalDate}/{suspended}")]
        [UnitOfWork]
        public CardUpdateResponse Update(string cardNumberOrRef, DateTime renewalDate, bool suspended)
        {
            return Dispatch(() =>
            {
                var crd = _leisureCardRepository.Get(cardNumberOrRef);
                var cards = crd == null ? _leisureCardRepository.GetByRef(cardNumberOrRef) : new[] {crd};

                var leisureCards = cards as LeisureCard[] ?? cards.ToArray();
                foreach (var card in leisureCards)
                {
                    card.RenewalDate = renewalDate;
                    _cardExpiryLogic.SetExpiryDate(card, card.RenewalDate.Value);
                    card.Suspended = suspended;

                    _leisureCardRepository.SaveOrUpdate(card);
                }

                var prototype = leisureCards.Any() ? Mapper.Map<Model.LeisureCard>(leisureCards[0]) : null;

                return new CardUpdateResponse
                {
                    CardsUpdated = leisureCards.Count(),
                    Prototype = prototype
                };
            });
        }

        [HttpGet]
        [SessionAuthFilter(true)]
        [Route("LeisureCard/Suspend/{cardNumberOrRef}/{suspended}")]
        [UnitOfWork]
        public CardUpdateResponse Suspend(string cardNumberOrRef, bool suspended)
        {
            return Dispatch(() =>
            {
                var crd = _leisureCardRepository.Get(cardNumberOrRef);
                var cards = crd == null ? _leisureCardRepository.GetByRef(cardNumberOrRef) : new[] {crd};

                var leisureCards = cards as LeisureCard[] ?? cards.ToArray();
                foreach (var card in leisureCards)
                {
                    card.Suspended = suspended;

                    _leisureCardRepository.SaveOrUpdate(card);
                }

                var prototype = leisureCards.Any() ? Mapper.Map<Model.LeisureCard>(leisureCards[0]) : null;

                return new CardUpdateResponse
                {
                    CardsUpdated = leisureCards.Count(),
                    Prototype = prototype
                };
            });
        }

        [HttpGet]
        [SessionAuthFilter(true)]
        [Route("LeisureCard/GetAllCardNumbers")]
        public IEnumerable<Model.LeisureCard> GetAllCardNumbers()
        {
            return Dispatch(()=>_leisureCardRepository.GetAll().Select(Mapper.Map<Model.LeisureCard>));
        }

        [HttpGet]
        [SessionAuthFilter(true)]
        [Route("LeisureCard/GetCardNumbersForUpdate")]
        public IEnumerable<Model.LeisureCard> GetCardNumbersForUpdate()
        {
            return Dispatch(() =>
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
            });
        }

        [HttpGet]
        [SessionAuthFilter(true)]
        [Route("LeisureCard/GenerateCards/{reference}/{numberOfCards}/{renewalPeriodMonths}")]
        public CardGenerationResponse GenerateCards(string reference, int numberOfCards, int renewalPeriodMonths)
        {
            try
            {
                return new CardGenerationResponse
                {
                    CardGenerationLog =
                        Mapper.Map<Model.CardGenerationLog>(_leisureCardService.GenerateCards(
                            reference, 
                            numberOfCards,
                            renewalPeriodMonths,
                            "GRG")),

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
