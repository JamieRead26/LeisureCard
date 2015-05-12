using System;
using System.Collections.Generic;
using GRG.LeisureCards.Model;

namespace GRG.LeisureCards.Persistence
{
    public interface ISelectedOfferRepository : IRepository<SelectedOffer, int>
    {
        IEnumerable<SelectedOffer> Get(DateTime @from, DateTime to);
    }
}
