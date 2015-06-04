using System.Collections.Generic;
using GRG.LeisureCards.DomainModel;

namespace GRG.LeisureCards.Persistence
{
    public interface ITwoForOneRepository : IRepository<TwoForOneOffer,int>
    {
        IEnumerable<TwoForOneOffer> GetWithNoLatLong();
    }
}
