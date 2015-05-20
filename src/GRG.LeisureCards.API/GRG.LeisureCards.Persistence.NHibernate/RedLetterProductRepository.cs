using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using GRG.LeisureCards.DomainModel;
using NHibernate;
using NHibernate.Linq;

namespace GRG.LeisureCards.Persistence.NHibernate
{
    public class RedLetterProductRepository : Repository<RedLetterProduct, int>, IRedLetterProductRepository
    {
        public ICollection<RedLetterProduct> FindByKeyword(string keyword)
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

        public ICollection<RedLetterProduct> Find(Expression<Func<RedLetterProduct, bool>> predicate)
        {
            return Session.Query<RedLetterProduct>().Where(predicate).ToArray();
        }

        public ICollection<RedLetterKeyword> GetAllKeywords()
        {
            return Session.QueryOver<RedLetterKeyword>().List();
        }
    }
}
