using System;

namespace GRG.LeisureCards.Service.BusinessLogic
{
    public interface ICardRenewalLogic
    {
        DateTime GetRenewalDate(DateTime registrationDate);
    }
    public class CardRenewalDateLogic : ICardRenewalLogic
    {
        public DateTime GetRenewalDate(DateTime registrationDate)
        {
            throw new NotImplementedException();
        }
    }
}
