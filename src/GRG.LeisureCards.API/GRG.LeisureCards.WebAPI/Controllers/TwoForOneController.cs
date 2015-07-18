using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GRG.LeisureCards.Persistence;
using GRG.LeisureCards.Service;
using GRG.LeisureCards.WebAPI.Authentication;
using GRG.LeisureCards.WebAPI.Filters;
using GRG.LeisureCards.WebAPI.Model;
using ApiModel = GRG.LeisureCards.WebAPI.Model;

namespace GRG.LeisureCards.WebAPI.Controllers
{
    [SessionAuthFilter]
    public class TwoForOneController : LcApiController
    {
        private readonly ITwoForOneRepository _twoForOneRepository;
        private readonly IOfferCategoryRepository _offerCategoryRepository;
        private readonly IUserSessionService _userSessionService;
        private readonly ISelectedOfferService _selectedOfferService;
        private readonly IUkLocationService _locationService;

        public TwoForOneController(
            ITwoForOneRepository twoForOneRepository, 
            IOfferCategoryRepository offerCategoryRepository,
            IUkLocationService locationService,
            IUserSessionService userSessionService,
            ISelectedOfferService selectedOfferService)
        {
            _twoForOneRepository = twoForOneRepository;
            _offerCategoryRepository = offerCategoryRepository;
            _userSessionService = userSessionService;
            _selectedOfferService = selectedOfferService;
            _locationService = locationService;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("TwoForOne/GetAll")]
        public IEnumerable<TwoForOneOffer> GetAll()
        {
            return Dispatch(()=>  _twoForOneRepository.GetAll().Select(Mapper.Map<TwoForOneOffer>));
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("TwoForOne/Get/{Id}")]
        public TwoForOneOffer Get(int id)
        {
            return Dispatch(()=>Mapper.Map<TwoForOneOffer>(_twoForOneRepository.Get(id)));
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("TwoForOne/ClaimOffer/{Id}")]
        public void ClaimOffer(int id)
        {
            Dispatch(() =>
            {
                var sessionInfo = ((LeisureCardPrincipal) RequestContext.Principal).SessionInfo;
                var session = _userSessionService.GetSession(sessionInfo.SessionToken);
                var offer = _twoForOneRepository.Get(id);

                _selectedOfferService.RecordClaim(
                    session.CardCode,
                    _offerCategoryRepository.TwoForOne,
                    id.ToString(),
                    offer.Description,
                    session.Token);
            });
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("TwoForOne/FindByLocation/{postCodeOrTown}/{radiusMiles}")]
        public IEnumerable<TwoForOneOfferGeoSearchResult> FindByLocation(string postCodeOrTown, int radiusMiles)
        {
            return Dispatch(() =>
            {
                var results = _locationService.Filter(postCodeOrTown, radiusMiles,
                    _twoForOneRepository.GetAllWithLocation(),
                    twoForOneOffer => _twoForOneRepository.SaveOrUpdate(twoForOneOffer));

                return
                    results.Select(
                        i =>
                            new TwoForOneOfferGeoSearchResult
                            {
                                TwoForOneOffer = Mapper.Map<TwoForOneOffer>(i.Item1),
                                Distance = Math.Round(i.Item2, 1)
                            });
            });
        }
    }
}