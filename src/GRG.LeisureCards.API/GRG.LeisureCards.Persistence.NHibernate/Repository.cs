using System;
using NHibernate;

namespace GRG.LeisureCards.Persistence.NHibernate
{
    public class Repository<TEntity, TKey> : IRepository<TEntity, TKey>
    {
        protected ISession Session { get { return UnitOfWork.Current.Session; } }

        public TEntity SaveOrUpdate(TEntity entity)
        {
            Session.SaveOrUpdate(entity);

            return entity;
        }

        public void Delete(TKey entityKey)
        {
            throw new NotImplementedException();
        }

        public TEntity Get(TKey key)
        {
            return Session.Get<TEntity>(key);
        }
    }
}
