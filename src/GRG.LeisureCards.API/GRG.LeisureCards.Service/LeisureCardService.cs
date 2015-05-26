using System;
using System.Linq;
using GRG.LeisureCards.DomainModel;
using GRG.LeisureCards.Persistence;
using GRG.LeisureCards.Persistence.NHibernate;
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
        CardGenerationLog GenerateCards(string reference, int numberOfCards, int renewalPeriodMonths);
    }

    public class LeisureCardService : ILeisureCardService
    {
        private readonly ICardRenewalLogic _cardRenewalLogic;
        private readonly ILeisureCardRepository _leisureCardRepository;
        private readonly ICardGenerationLogRepository _cardGenerationLogRepository;
        private readonly IAdminCodeProvider _adminCodeProvider;

        public LeisureCardService(
            ICardRenewalLogic cardRenewalLogic, 
            ILeisureCardRepository leisureCardRepository,
            ICardGenerationLogRepository cardGenerationLogRepository,
            IAdminCodeProvider adminCodeProvider)
        {
            _cardRenewalLogic = cardRenewalLogic;
            _leisureCardRepository = leisureCardRepository;
            _cardGenerationLogRepository = cardGenerationLogRepository;
            _adminCodeProvider = adminCodeProvider;
        }

        public LeisureCardRegistrationResponse Login(string cardCode)
        {
            if (_adminCodeProvider.IsAdminCode(cardCode))
                return new LeisureCardRegistrationResponse { Status = RegistrationResult.Ok.ToString(), LeisureCard = AdminLeisureCard.Instance };

            var leisureCard = _leisureCardRepository.Get(cardCode);

            if (leisureCard == null)
                return new LeisureCardRegistrationResponse {Status = RegistrationResult.CodeNotFound.ToString()};

            if (leisureCard.Suspended)
                return new LeisureCardRegistrationResponse { Status = RegistrationResult.CardSuspended.ToString() };

            if (leisureCard.RenewalDate != null && leisureCard.RenewalDate < DateTime.Now)
                return new LeisureCardRegistrationResponse { Status = RegistrationResult.CardExpired.ToString() };

            leisureCard.RegistrationDate = DateTime.Now;
             _cardRenewalLogic.SetRenewalDate(leisureCard);

            _leisureCardRepository.SaveOrUpdate(leisureCard);

            return new LeisureCardRegistrationResponse {Status = RegistrationResult.Ok.ToString(), LeisureCard = leisureCard};
        }

        [UnitOfWork]
        public CardGenerationLog GenerateCards(string reference, int numberOfCards, int renewalPeriodMonths)
        {
            if (_cardGenerationLogRepository.Get(reference)!=null)
                throw new Exception("Card generation reference is not unique : " + reference);

            var allCardCodes = _leisureCardRepository.GetAllIncludingDeleted().Select(c=>c.Code).ToArray();
            
            for (var i = 0; i < numberOfCards; i++)
            {
                string newCode;
                do
                {
                    newCode = Guid.NewGuid().ToString().Substring(0, 20);
                } 
                while (allCardCodes.Contains(newCode));

                _leisureCardRepository.SaveOrUpdate( new LeisureCard{Code = newCode, Reference = reference, RenewalPeriodMonths = renewalPeriodMonths});
            }

            var cardGenLog = new CardGenerationLog {GeneratedDate = DateTime.Now, Ref = reference};

            _cardGenerationLogRepository.SaveOrUpdate(cardGenLog);

            return cardGenLog;
        }
    }
}
