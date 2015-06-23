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
        CardExpired,
        ClientInactive
    }

    public interface ILeisureCardService
    {
        LeisureCardRegistrationResponse Login(string cardCode);
        CardGenerationLog GenerateCards(string reference, int numberOfCards, int renewalPeriodMonths, string tenantKey);
        void AcceptMembershipTerms(string cardCode);
    }

    public class LeisureCardService : ILeisureCardService
    {
        private readonly ICardExpiryLogic _cardExpiryLogic;
        private readonly ILeisureCardRepository _leisureCardRepository;
        private readonly ICardGenerationLogRepository _cardGenerationLogRepository;
        private readonly IAdminCodeProvider _adminCodeProvider;
        private readonly ILeisureCardUsageRepository _leisureCardUsageRepository;
        private readonly ITenantRepository _tenantRepository;

        public LeisureCardService(
            ICardExpiryLogic cardExpiryLogic, 
            ILeisureCardRepository leisureCardRepository,
            ICardGenerationLogRepository cardGenerationLogRepository,
            IAdminCodeProvider adminCodeProvider,
            ILeisureCardUsageRepository leisureCardUsageRepository,
            ITenantRepository tenantRepository)
        {
            _cardExpiryLogic = cardExpiryLogic;
            _leisureCardRepository = leisureCardRepository;
            _cardGenerationLogRepository = cardGenerationLogRepository;
            _adminCodeProvider = adminCodeProvider;
            _leisureCardUsageRepository = leisureCardUsageRepository;
            _tenantRepository = tenantRepository;
        }

        [UnitOfWork]
        public LeisureCardRegistrationResponse Login(string cardCode)
        {
            if (_adminCodeProvider.IsAdminCode(cardCode))
                return new LeisureCardRegistrationResponse { 
                    Status = RegistrationResult.Ok.ToString(), 
                    LeisureCard = AdminLeisureCard.Instance};

            var leisureCard = _leisureCardRepository.Get(cardCode);

            if (leisureCard == null)
                return new LeisureCardRegistrationResponse {Status = RegistrationResult.CodeNotFound.ToString()};

            var tenant = _tenantRepository.Get(leisureCard.TenantKey);
            
            if (!tenant.Active)
                return new LeisureCardRegistrationResponse { Status = RegistrationResult.ClientInactive.ToString() };

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
                LeisureCard = leisureCard,
                DisplayMemberLoginPopup = !leisureCard.MembershipTermsAccepted.HasValue && tenant.MemberLoginPopupDisplayed,
                MemberLoginPopupAcceptanceMandatory = tenant.MemberLoginPopupMandatory
            };
        }

        public void AcceptMembershipTerms(string cardCode)
        {
            var card = _leisureCardRepository.Get(cardCode);

            card.MembershipTermsAccepted = DateTime.Now;

            _leisureCardRepository.Update(card);
        }

        [UnitOfWork]
        public CardGenerationLog GenerateCards(string reference, int numberOfCards, int renewalPeriodMonths, string tenantKey)
        {
            if (_cardGenerationLogRepository.Get(reference)!=null)
                throw new Exception("Card generation reference is not unique : " + reference);

            var tenant = _tenantRepository.Get(tenantKey);

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
                    UploadedDate = now,
                    TenantKey = tenant.Key
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

        public bool DisplayMemberLoginPopup { get; set; }

        public bool MemberLoginPopupAcceptanceMandatory { get; set; }

    }
}
