using System.Collections.Generic;
using System.Linq;
using GRG.LeisureCards.Model;
using NHibernate;
using NHibernate.Linq;

namespace GRG.LeisureCards.Persistence.NHibernate
{
    public class RedLetterRepository : Repository<RedLetterProduct, int>, IRedLetterProductRepository
    {
        public ICollection<RedLetterProduct> Find(string keyword)
        {
            var rlKeyword = Session.Query<RedLetterKeyword>().SingleOrDefault(x => x.Keyword == keyword);

            return rlKeyword == null ? new RedLetterProduct[0] : rlKeyword.Products;
        }

        public override RedLetterProduct Get(int key)
        {
            var product = base.Get(key);

            if (product == null)
                return null;

            NHibernateUtil.Initialize(product.Facts);
            NHibernateUtil.Initialize(product.Venues);
            
            return product;
        }
    }
}
