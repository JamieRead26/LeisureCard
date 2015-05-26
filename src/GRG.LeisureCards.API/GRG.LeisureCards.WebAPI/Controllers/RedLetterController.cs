﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using AutoMapper;
using GRG.LeisureCards.DomainModel;
using GRG.LeisureCards.Persistence;
using GRG.LeisureCards.Service;
using GRG.LeisureCards.WebAPI.Authentication;
using GRG.LeisureCards.WebAPI.Filters;
using GRG.LeisureCards.WebAPI.Mappings;
using GRG.LeisureCards.WebAPI.Model;
using SelectedOffer = GRG.LeisureCards.DomainModel.SelectedOffer;

namespace GRG.LeisureCards.WebAPI.Controllers
{
    [RoutePrefix("RedLetter")]
    [SessionAuthFilter]
    public class RedLetterController : ApiController
    {
        private readonly IRedLetterProductRepository _redLetterProductRepository;
        private readonly ISelectedOfferRepository _selectedOfferRepository;
        private readonly IOfferCategoryRepository _offerCategoryRepository;
        private readonly UserSessionService _userSessionService;

        public RedLetterController(IRedLetterProductRepository redLetterProductRepository,
            ISelectedOfferRepository selectedOfferRepository,
            IOfferCategoryRepository offerCategoryRepository)
        {
            _redLetterProductRepository = redLetterProductRepository;
            _selectedOfferRepository = selectedOfferRepository;
            _offerCategoryRepository = offerCategoryRepository;
            _userSessionService = UserSessionService.Instance;
        }

        [HttpGet]
        [Route("FindByKeyword/{keyword}")]
        public List<RedLetterProductSummary> Find(string keyword)
        {
            return _redLetterProductRepository.FindByKeyword(keyword).Select(Mapper.Map<RedLetterProductSummary>).ToList();
        }
        
        [HttpGet]
        [Route("Get/{id}")]
        public RedLetterProduct Get(int id)
        {
            var result =  _redLetterProductRepository.Get(id);

            return result;
        }

        [HttpGet]
        [Route("GetRandomSpecialOffers/{count}")]
        public IEnumerable<RedLetterProductSummary> GetRandomSpecialOffers(int count)
        {
            var products = _redLetterProductRepository.Find(product => product.IsSpecialOffer);

            var shuffled = Shuffle(products.ToArray(), count);

            var summaries = shuffled.Select(Mapper.Map<RedLetterProductSummary>);

            return summaries;
        }

        [HttpGet]
        [Route("ClaimOffer/{Id}")]
        public void ClaimOffer(int id)
        {
            var sessionInfo = ((LeisureCardPrincipal)RequestContext.Principal).SessionInfo;
            var card = _userSessionService.GetSession(sessionInfo.SessionToken);
            var offer = _redLetterProductRepository.Get(id);

            _selectedOfferRepository.SaveOrUpdate(new SelectedOffer
            {
                LeisureCard = card.LeisureCard,
                OfferCategory = _offerCategoryRepository.RedLetter,
                OfferId = id.ToString(),
                OfferTitle = offer.Title
            });
        }

        [HttpGet]
        [Route("LogClickThrough/{category}")]
        public void ClaimOffer(string category)
        {
            var sessionInfo = ((LeisureCardPrincipal)RequestContext.Principal).SessionInfo;
            var card = _userSessionService.GetSession(sessionInfo.SessionToken);

            _selectedOfferRepository.SaveOrUpdate(new SelectedOffer
            {
                LeisureCard = card.LeisureCard,
                OfferCategory = _offerCategoryRepository.RedLetter,
                OfferTitle = category
            });
        }

        static T[] Shuffle<T>(T[] array, int max = -1)
        {
            var random = new Random();
            var n = array.Length;
            for (var i = 0; i < n; i++)
            {
                if (i == max)
                    break;
                // NextDouble returns a random number between 0 and 1.
                // ... It is equivalent to Math.random() in Java.
                var r = i + (int)(random.NextDouble() * (n - i));
                var t = array[r];
                array[r] = array[i];
                array[i] = t;
            }

            return (max > -1) ? array.Take(Math.Min(max, array.Length)).ToArray() : array;
        }
    }
}