﻿using System.Web.Http;
using GRG.LeisureCards.Model;
using GRG.LeisureCards.Persistence;
using GRG.LeisureCards.Service;
using GRG.LeisureCards.WebAPI.Authentication;

namespace GRG.LeisureCards.WebAPI.Controllers
{
    [RoutePrefix("ShortBreaks")]
    public class ShortBreaksController : ApiController
    {
        private readonly ISelectedOfferRepository _selectedOfferRepository;
        private readonly IOfferCategoryRepository _offerCategoryRepository;
        private readonly UserSessionService _userSessionService;

        public ShortBreaksController(ISelectedOfferRepository selectedOfferRepository, IOfferCategoryRepository offerCategoryRepository)
        {
            _selectedOfferRepository = selectedOfferRepository;
            _offerCategoryRepository = offerCategoryRepository;
            _userSessionService = UserSessionService.Instance;

        }

        [HttpGet]
        [Route("ClaimOffer/{title}")]
        public void ClaimOffer(string title)
        {
            var sessionInfo = ((LeisureCardPrincipal)RequestContext.Principal).SessionInfo;
            var card = _userSessionService.GetCard(sessionInfo.SessionToken);

            _selectedOfferRepository.SaveOrUpdate(new SelectedOffer
            {
                LeisureCard = card,
                OfferCategory = _offerCategoryRepository.ShortBreaks,
                OfferTitle = title
            });
        }
    }
}