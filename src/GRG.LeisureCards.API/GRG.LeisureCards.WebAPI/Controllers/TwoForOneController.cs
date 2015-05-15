using System.Collections.Generic;
using System.Web.Http;
using GRG.LeisureCards.Model;
using GRG.LeisureCards.Persistence;
using GRG.LeisureCards.Service;
using GRG.LeisureCards.WebAPI.Authentication;
using GRG.LeisureCards.WebAPI.Filters;

namespace GRG.LeisureCards.WebAPI.Controllers
{
    [SessionAuthFilter]
    public class TwoForOneController : ApiController
    {
        private readonly ITwoForOneRepository _twoForOneRepository;
        private readonly ISelectedOfferRepository _selectedOfferRepository;
        private readonly IOfferCategoryRepository _offerCategoryRepository;
        private readonly IUserSessionService _userSessionService;

        public TwoForOneController(
            ITwoForOneRepository twoForOneRepository, 
            ISelectedOfferRepository selectedOfferRepository,
            IOfferCategoryRepository offerCategoryRepository)
        {
            _twoForOneRepository = twoForOneRepository;
            _selectedOfferRepository = selectedOfferRepository;
            _offerCategoryRepository = offerCategoryRepository;
            _userSessionService = UserSessionService.Instance;
        }

        [HttpGet]
        [Route("TwoForOne/GetAll")]
        public IEnumerable<TwoForOneOffer> GetAll()
        {
            return _twoForOneRepository.GetAll();
        }

        [HttpGet]
        [Route("TwoForOne/Get/{Id}")]
        public TwoForOneOffer Get(int id)
        {
            return _twoForOneRepository.Get(id);
        }

        [HttpGet]
        [Route("TwoForOne/ClaimOffer/{Id}")]
        public void ClaimOffer(int id)
        {
            var sessionInfo =  ((LeisureCardPrincipal)RequestContext.Principal).SessionInfo;
            var card = _userSessionService.GetCard(sessionInfo.SessionToken);
            var offer = _twoForOneRepository.Get(id);

            _selectedOfferRepository.SaveOrUpdate(new SelectedOffer
            {
                LeisureCard = card,
                OfferCategory = _offerCategoryRepository.TwoForOne,
                OfferId = id.ToString(),
                OfferTitle = offer.Description
            });
        }

        [HttpGet]
        [Route("TwoForOne/FindByLocation/{longtitude}/{latitude}/{miles}")]
        public IEnumerable<TwoForOneOffer> FindByLocation(decimal longtitude, decimal latitude, int miles)
        {
            return _twoForOneRepository.GetAll();
        }
    }
}