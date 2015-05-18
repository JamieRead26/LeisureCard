﻿using System;
using GRG.LeisureCards.Model;
using GRG.LeisureCards.Persistence;
using GRG.LeisureCards.Service.BusinessLogic;

namespace GRG.LeisureCards.Service
{
    public enum RegistrationResult
    {
        CodeNotFound,
        CardAlreadyRegistered,
        CardSuspended,
        Ok,
        CardExpired
    }

    public interface ILeisureCardService
    {
        LeisureCardRegistrationResponse Login(string cardCode);
    }

    public class LeisureCardService : ILeisureCardService
    {
        private readonly ICardRenewalLogic _cardRenewalLogic;
        private readonly ILeisureCardRepository _leisureCardRepository;

        public LeisureCardService(
            ICardRenewalLogic cardRenewalLogic, 
            ILeisureCardRepository leisureCardRepository)
        {
            _cardRenewalLogic = cardRenewalLogic;
            _leisureCardRepository = leisureCardRepository;
        }

        public LeisureCardRegistrationResponse Login(string cardCode)
        {
            var leisureCard = _leisureCardRepository.Get(cardCode);

            if (leisureCard == null)
                return new LeisureCardRegistrationResponse {Status = RegistrationResult.CodeNotFound.ToString()};

            if (leisureCard.Suspended)
                return new LeisureCardRegistrationResponse { Status = RegistrationResult.CardSuspended.ToString() };

            if (leisureCard.RenewalDate!=null && leisureCard.RenewalDate < DateTime.Now)
                return new LeisureCardRegistrationResponse { Status = RegistrationResult.CardExpired.ToString() };

            leisureCard.RegistrationDate = DateTime.Now;
            leisureCard.RenewalDate = _cardRenewalLogic.GetRenewalDate(DateTime.Now);

            _leisureCardRepository.SaveOrUpdate(leisureCard);

            return new LeisureCardRegistrationResponse {Status = RegistrationResult.Ok.ToString(), LeisureCard = leisureCard};
        }
    }
}
