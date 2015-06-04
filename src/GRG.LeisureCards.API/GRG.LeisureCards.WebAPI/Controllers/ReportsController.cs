using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using AutoMapper;
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
        private readonly ITwoForOneRepository _twoForOneRepository;

        public ReportsController(
            ILeisureCardUsageRepository leisureCardUsageRepository,
            ISelectedOfferRepository selectedOfferRepository,
            ILeisureCardRepository leisureCardRepository,
            ITwoForOneRepository twoForOneRepository)
        {
            _leisureCardUsageRepository = leisureCardUsageRepository;
            _selectedOfferRepository = selectedOfferRepository;
            _leisureCardRepository = leisureCardRepository;
            _twoForOneRepository = twoForOneRepository;
        }

        [HttpGet]
        [Route("GetLoginHistory/{from}/{to}")]
        public IEnumerable<Model.LeisureCardUsage> GetLoginHistory(DateTime from, DateTime to)
        {
            return _leisureCardUsageRepository.Get(from, to).Select(Mapper.Map<Model.LeisureCardUsage>);
        }
        
        //[UnitOfWork]
        [HttpGet]
        [Route("GetSelectedOfferHistory/{from}/{to}")]
        public IEnumerable<Model.SelectedOffer> GetSelectedOfferHistory(DateTime from, DateTime to)
        {
            var results = _selectedOfferRepository.Get(from, to).Select(Mapper.Map<Model.SelectedOffer>).ToArray();

            return results;
        }

        [HttpGet]
        [Route("GetCardActivationHistory/{from}/{to}")]
        public IEnumerable<Model.LeisureCard> GetCardActivationHistory(DateTime from, DateTime to)
        {
            return _leisureCardRepository.GetRegistrationHistory(from, to).Select(Mapper.Map<Model.LeisureCard>);
        }

        [HttpGet]
        [Route("GetTwoForOneMissingLocation/")]
        public IEnumerable<Model.TwoForOneOffer> GetTwoForOneMissingLocation()
        {
            return _twoForOneRepository.GetWithNoLatLong().Select(Mapper.Map<Model.TwoForOneOffer>);
        }
    }
}