using System;
using System.Collections.Generic;
using System.Linq;
using GRG.LeisureCards.DomainModel;
using GRG.LeisureCards.Persistence;
using GRG.LeisureCards.Persistence.NHibernate;

namespace GRG.LeisureCards.Service
{
    public interface ISelectedOfferService
    {
        void RecordClaim(string cardCode, OfferCategory twoForOne, string offerId, string description, string sessionToken);
    }

    public class SelectedOfferService : ISelectedOfferService
    {
        private readonly ITwoForOneRepository _twoForOneRepository;
        private readonly ISelectedOfferRepository _selectedOfferRepository;
        private readonly IOfferCategoryRepository _offerCategoryRepository;

        public SelectedOfferService(ISelectedOfferRepository selectedOfferRepository, IOfferCategoryRepository offerCategoryRepository)
        {
            _selectedOfferRepository = selectedOfferRepository;
            _offerCategoryRepository = offerCategoryRepository;
        }

        [UnitOfWork]
        public void RecordClaim(string cardCode, OfferCategory twoForOne, string offerId, string description, string sessionToken)
        {
            if (!_selectedOfferRepository.Find(s => s.OfferId == offerId && s.SessionToken == sessionToken).Any())
            {
                _selectedOfferRepository.Save(new SelectedOffer
                {
                    LeisureCardCode = cardCode,
                    OfferCategory = _offerCategoryRepository.TwoForOne,
                    OfferId = offerId,
                    OfferTitle = description,
                    SelectedDateTime = DateTime.Now,
                    SessionToken = sessionToken
                });
            }
        }
    }
}
