using System;
using GRG.LeisureCards.DomainModel;
using GRG.LeisureCards.Persistence;

namespace GRG.LeisureCards.Service.BusinessLogic
{
    public interface ICardRenewalLogic
    {
        void SetExpiryDate(LeisureCard leisureCard);
    }
    public class CardRenewalLogic : ICardRenewalLogic
    {
        private readonly int _defaultRenewalPeriodMonths;

        public CardRenewalLogic(int defaultCardRenewalPeriodMonths)
        {
            _defaultRenewalPeriodMonths = defaultCardRenewalPeriodMonths;
        }

        public void SetExpiryDate(LeisureCard leisureCard)
        {
            if (!leisureCard.RegistrationDate.HasValue)
                throw new Exception("Can not calculate renewal date of unregistered card ");

            leisureCard.ExpiryDate = AddMonths(leisureCard.RegistrationDate.Value,
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
