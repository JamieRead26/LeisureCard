using System;
using GRG.LeisureCards.Persistence;
using GRG.LeisureCards.Service.BusinessLogic;

namespace GRG.LeisureCards.Service
{
    public enum RegistrationResult
    {
        CodeNotFound,
        CardAlreadyRegistered,
        CardSuspended,
        Ok
    }

    public class LeisureCardRegistrationService
    {
        private readonly ICardRenewalLogic _cardRenewalLogic;
        private readonly ILeisureCardRepository _leisureCardRepository;

        public LeisureCardRegistrationService(
            ICardRenewalLogic cardRenewalLogic, 
            ILeisureCardRepository leisureCardRepository)
        {
            _cardRenewalLogic = cardRenewalLogic;
            _leisureCardRepository = leisureCardRepository;
        }

        public RegistrationResult Register(string cardCode)
        {
            throw new NotImplementedException();
        }
    }
}
