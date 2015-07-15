using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using AutoMapper;
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
    public class TwoForOneController : LcApiController
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
            IUkLocationService locationService,
            IUserSessionService userSessionService)
        {
            _twoForOneRepository = twoForOneRepository;
            _selectedOfferRepository = selectedOfferRepository;
            _offerCategoryRepository = offerCategoryRepository;
            _userSessionService = userSessionService;
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
            return Dispatch(()=>  Mapper.Map<TwoForOneOffer>(_twoForOneRepository.Get(id)));
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

                _selectedOfferRepository.SaveOrUpdate(new SelectedOffer
                {
                    LeisureCardCode = session.CardCode,
                    OfferCategory = _offerCategoryRepository.TwoForOne,
                    OfferId = id.ToString(),
                    OfferTitle = offer.Description,
                    SelectedDateTime = DateTime.Now
                });

                //var pdfWriter = new TwoForOneVoucherPDFWriter(
                //    ConfigurationManager.AppSettings["UiWebRootUrl"],
                //    session.TenantKey,
                //    (DateTime.Now+TimeSpan.FromDays(14)).ToString("dd MMMM yyyy"),
                //    offer.BookingInstructions,
                //    offer.ClaimCode,
                //    offer.OutletName,
                //    _htmlTemplateFactory.GetHtmlTemplates(session.TenantKey).VoucherContent);

                //var stream = new MemoryStream();

                //pdfWriter.Write(stream);

                //stream.Position = 0;

                //var result = new HttpResponseMessage(HttpStatusCode.OK)
                //{
                //    Content = new StreamContent(stream)
                //};
               
                //result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
                //return result;
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