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
        private readonly ICardRenewalLogic _cardExpiryLogic;
        private readonly ILeisureCardRepository _leisureCardRepository;
        private readonly ICardGenerationLogRepository _cardGenerationLogRepository;
        private readonly IAdminCodeProvider _adminCodeProvider;
        private readonly ILeisureCardUsageRepository _leisureCardUsageRepository;

        public LeisureCardService(
            ICardRenewalLogic cardExpiryLogic, 
            ILeisureCardRepository leisureCardRepository,
            ICardGenerationLogRepository cardGenerationLogRepository,
            IAdminCodeProvider adminCodeProvider,
            ILeisureCardUsageRepository leisureCardUsageRepository)
        {
            _cardExpiryLogic = cardExpiryLogic;
            _leisureCardRepository = leisureCardRepository;
            _cardGenerationLogRepository = cardGenerationLogRepository;
            _adminCodeProvider = adminCodeProvider;
            _leisureCardUsageRepository = leisureCardUsageRepository;
        }

        public LeisureCardRegistrationResponse Login(string cardCode)
        {
            if (_adminCodeProvider.IsAdminCode(cardCode))
                return new LeisureCardRegistrationResponse { 
                    Status = RegistrationResult.Ok.ToString(), 
                    LeisureCard = AdminLeisureCard.Instance};

            var leisureCard = _leisureCardRepository.Get(cardCode);

            if (leisureCard == null)
                return new LeisureCardRegistrationResponse {Status = RegistrationResult.CodeNotFound.ToString()};

            switch (leisureCard.StatusEnum)
            {
                case LeisureCardStatus.Suspended:
                    return new LeisureCardRegistrationResponse { Status = RegistrationResult.CardSuspended.ToString() };

                case LeisureCardStatus.Expired:
                    return new LeisureCardRegistrationResponse { Status = RegistrationResult.CardExpired.ToString() };

                case LeisureCardStatus.Inactive:
                     leisureCard.RegistrationDate = DateTime.Now;
                     _cardExpiryLogic.SetExpiryDate(leisureCard);
                     _leisureCardRepository.SaveOrUpdate(leisureCard);
                    break;
            }

            _leisureCardUsageRepository.SaveOrUpdate(
                new LeisureCardUsage
                {
                    LeisureCard = leisureCard,
                    LoginDateTime = DateTime.Now
                });

            return new LeisureCardRegistrationResponse
            {
                Status = RegistrationResult.Ok.ToString(), 
                LeisureCard = leisureCard
            };
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
                    newCode = Guid.NewGuid().ToString().Substring(0, 20).ToUpper();
                } 
                while (allCardCodes.Contains(newCode));

                var now = DateTime.Now;

                _leisureCardRepository.SaveOrUpdate( new LeisureCard
                {
                    Code = newCode,
                    Reference = reference,
                    RenewalPeriodMonths = renewalPeriodMonths,
                    UploadedDate = now
                });
            }

            var cardGenLog = new CardGenerationLog {GeneratedDate = DateTime.Now, Ref = reference};

            _cardGenerationLogRepository.SaveOrUpdate(cardGenLog);

            return cardGenLog;
        }
    }

    public class LeisureCardRegistrationResponse
    {
        public string Status { get; set; }
        public LeisureCard LeisureCard { get; set; }
        public SessionInfo SessionInfo { get; set; }
    }
}
