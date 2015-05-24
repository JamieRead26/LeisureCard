using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using AutoMapper;
using GRG.LeisureCards.DomainModel;
using GRG.LeisureCards.Persistence;
using GRG.LeisureCards.Service;
using GRG.LeisureCards.WebAPI.Authentication;
using GRG.LeisureCards.WebAPI.Filters;
using GRG.LeisureCards.WebAPI.Model;
using ApiModel = GRG.LeisureCards.WebAPI.Model;
using SelectedOffer = GRG.LeisureCards.DomainModel.SelectedOffer;

namespace GRG.LeisureCards.WebAPI.Controllers
{
    [SessionAuthFilter]
    public class TwoForOneController : ApiController
    {
        private readonly ITwoForOneRepository _twoForOneRepository;
        private readonly ISelectedOfferRepository _selectedOfferRepository;
        private readonly IOfferCategoryRepository _offerCategoryRepository;
        private readonly IUserSessionService _userSessionService;
        private readonly IUkLocationService _locationService;

        public TwoForOneController(
            ITwoForOneRepository twoForOneRepository, 
            ISelectedOfferRepository selectedOfferRepository,
            IOfferCategoryRepository offerCategoryRepository,
            IUkLocationService locationService)
        {
            _twoForOneRepository = twoForOneRepository;
            _selectedOfferRepository = selectedOfferRepository;
            _offerCategoryRepository = offerCategoryRepository;
            _userSessionService = UserSessionService.Instance;
            _locationService = locationService;
        }

        [HttpGet]
        [Route("TwoForOne/GetAll")]
        public IEnumerable<ApiModel.TwoForOneOffer> GetAll()
        {
            return _twoForOneRepository.GetAll().Select(Mapper.Map<ApiModel.TwoForOneOffer>);
        }

        [HttpGet]
        [Route("TwoForOne/Get/{Id}")]
        public ApiModel.TwoForOneOffer Get(int id)
        {
            return Mapper.Map<ApiModel.TwoForOneOffer>(_twoForOneRepository.Get(id));
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
        [Route("TwoForOne/FindByLocation/{postCodeOrTown}/{radiusMiles}")]
        public IEnumerable<TwoForOneOfferGeoSearchResult> FindByLocation(string postCodeOrTown, int radiusMiles)
        {
            var results = _locationService.Filter(postCodeOrTown, radiusMiles, _twoForOneRepository.GetAll(), twoForOneOffer => _twoForOneRepository.SaveOrUpdate(twoForOneOffer));

            return results.Select(i => new TwoForOneOfferGeoSearchResult { TwoForOneOffer = Mapper.Map<ApiModel.TwoForOneOffer>(i.Item1), Distance = Math.Round(i.Item2,1) });
        }
    }
}