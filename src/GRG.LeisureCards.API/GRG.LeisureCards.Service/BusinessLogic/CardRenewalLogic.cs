using System;
using GRG.LeisureCards.DomainModel;

namespace GRG.LeisureCards.Service.BusinessLogic
{
    public interface ICardExpiryLogic
    {
        void SetExpiryDate(LeisureCard leisureCard);
        void SetExpiryDate(LeisureCard leisureCard, DateTime renewalDate);
    }
    public class CardExpiryLogic : ICardExpiryLogic
    {
        private readonly int _defaultRenewalPeriodMonths;

        public CardExpiryLogic(int defaultCardRenewalPeriodMonths)
        {
            _defaultRenewalPeriodMonths = defaultCardRenewalPeriodMonths;
        }

        public void SetExpiryDate(LeisureCard leisureCard)
        {
            if (!leisureCard.RegistrationDate.HasValue)
                throw new Exception("Can not calculate renewal date of unregistered card ");

            SetExpiryDate(leisureCard, leisureCard.RegistrationDate.Value);
        }

        public void SetExpiryDate(LeisureCard leisureCard, DateTime renewalDate)
        {
            leisureCard.ExpiryDate = AddMonths(renewalDate,
                leisureCard.RenewalPeriodMonths);
        }

        private DateTime AddMonths(DateTime registrationDate, int months)
        {
            var totalMonths = registrationDate.Month + months;
            int newMonth;
            var years = Math.DivRem(totalMonths, 12, out newMonth);

            return new DateTime(registrationDate.Year+years, newMonth == 0 ? 12 : newMonth, registrationDate.Day);
        }
    }
}
