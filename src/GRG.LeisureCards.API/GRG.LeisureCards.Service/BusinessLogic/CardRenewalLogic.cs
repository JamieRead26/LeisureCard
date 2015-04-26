using System;
using GRG.LeisureCards.Persistence;

namespace GRG.LeisureCards.Service.BusinessLogic
{
    public interface ICardRenewalLogic
    {
        DateTime GetRenewalDate(DateTime registrationDate);
    }
    public class CardRenewalLogic : ICardRenewalLogic
    {
        private readonly ISettingRepository _settingRepository;

        public CardRenewalLogic(ISettingRepository settingRepository)
        {
            _settingRepository = settingRepository;
        }

        public DateTime GetRenewalDate(DateTime registrationDate)
        {
            var period = TimeSpan.FromDays(int.Parse(_settingRepository.Get("RenewalPeriodDays").Value));

            return (registrationDate + period).Date;
        }
    }
}
