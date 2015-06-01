using System;
using System.Collections.Generic;
using GRG.LeisureCards.WebAPI.Model;

namespace GRG.LeisureCards.WebAPI.ClientContract
{
    public interface IReportService
    {
        IEnumerable<LeisureCardUsage> GetloginHistory(DateTime from, DateTime to);
        IEnumerable<SelectedOffer> GetSelectedOfferHistory(DateTime from, DateTime to);
        IEnumerable<LeisureCard> GetCardActivationHistory(DateTime from, DateTime to);
    }
}
