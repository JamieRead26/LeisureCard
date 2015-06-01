using System.Collections.Generic;
using GRG.LeisureCards.WebAPI.Model;

namespace GRG.LeisureCards.WebAPI.ClientContract
{
    public interface IRedLetterService
    {
        IEnumerable<RedLetterProductSummary> GetSpecialOffers(int count);
    }
}
