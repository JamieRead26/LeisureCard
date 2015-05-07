using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using GRG.LeisureCards.Model;

namespace GRG.LeisureCards.Persistence
{
    public interface IRedLetterProductRepository : IRepository<RedLetterProduct, int>
    {
        ICollection<RedLetterProduct> FindByKeyword(string keyword);
        ICollection<RedLetterProduct> Find(Expression<Func<RedLetterProduct, bool>> predicate);
    }
}
