﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using GRG.LeisureCards.Model;
using GRG.LeisureCards.Persistence;
using GRG.LeisureCards.WebAPI.Filters;

namespace GRG.LeisureCards.WebAPI.Controllers
{
    [SessionAuthFilter(true)]
    [RoutePrefix("Reports")]
    public class ReportsController : ApiController
    {
        private readonly ILeisureCardUsageRepository _leisureCardUsageRepository;
        private readonly ISelectedOfferRepository _selectedOfferRepository;
        private readonly ILeisureCardRepository _leisureCardRepository;

        public ReportsController(
            ILeisureCardUsageRepository leisureCardUsageRepository,
            ISelectedOfferRepository selectedOfferRepository,
            ILeisureCardRepository leisureCardRepository)
        {
            _leisureCardUsageRepository = leisureCardUsageRepository;
            _selectedOfferRepository = selectedOfferRepository;
            _leisureCardRepository = leisureCardRepository;
        }

        [HttpGet]
        [Route("GetLoginHistory/{from}/{to}")]
        public IEnumerable<LeisureCardUsage> GetLoginHistory(DateTime from, DateTime to)
        {
            return _leisureCardUsageRepository.Get(from, to);
        }


        [HttpGet]
        [Route("GetSelectedOfferHistory/{from}/{to}")]
        public IEnumerable<SelectedOffer> GetSelectedOfferHistory(DateTime from, DateTime to)
        {
            return _selectedOfferRepository.Get(from, to);
        }

        [HttpGet]
        [Route("GetCardActivationHistory/{from}/{to}")]
        public IEnumerable<LeisureCardInfo> GetCardActivationHistory(DateTime from, DateTime to)
        {
            return _leisureCardRepository.GetRegistrationHistory(from, to).Select(c=>new LeisureCardInfo(c));
        }
    }
}