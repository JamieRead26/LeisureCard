using System.Collections.Generic;
using GRG.LeisureCards.Model;

namespace GRG.LeisureCards.Persistence
{
    public interface IRedLetterProductRepository : IRepository<RedLetterProduct, int>
    {
        ICollection<RedLetterProduct> Find(string keyword);
    }
}
