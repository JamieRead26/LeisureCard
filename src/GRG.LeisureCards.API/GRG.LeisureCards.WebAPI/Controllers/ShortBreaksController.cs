using System.Net.Http;
using System.Web.Http;
using GRG.LeisureCards.DomainModel;
using GRG.LeisureCards.Persistence;
using GRG.LeisureCards.Service;
using GRG.LeisureCards.WebAPI.Authentication;
using GRG.LeisureCards.WebAPI.Filters;

namespace GRG.LeisureCards.WebAPI.Controllers
{
    [SessionAuthFilter]
    [RoutePrefix("ShortBreaks")]
    public class ShortBreaksController : ApiController
    {
        private readonly ISelectedOfferRepository _selectedOfferRepository;
        private readonly IOfferCategoryRepository _offerCategoryRepository;
        private readonly IUserSessionService _userSessionService;

        public ShortBreaksController(
            ISelectedOfferRepository selectedOfferRepository, 
            IOfferCategoryRepository offerCategoryRepository,
            IUserSessionService userSessionService)
        {
            _selectedOfferRepository = selectedOfferRepository;
            _offerCategoryRepository = offerCategoryRepository;
            _userSessionService = userSessionService;

        }

        [HttpGet]
        [Route("ClaimOffer/{title}")]
        public IHttpActionResult ClaimOffer(string title)
        {
            var sessionInfo = ((LeisureCardPrincipal)RequestContext.Principal).SessionInfo;
            var card = _userSessionService.GetSession(sessionInfo.SessionToken);

            _selectedOfferRepository.SaveOrUpdate(new SelectedOffer
            {
                LeisureCardCode = card.CardCode,
                OfferCategory = _offerCategoryRepository.ShortBreaks,
                OfferTitle = title
            });

            return Ok();
        }


    }
}