using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NHibernate;
using NHibernate.Linq;

namespace GRG.LeisureCards.Persistence.NHibernate
{
    public class Repository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class
    {
        protected ISession Session { get { return UnitOfWork.Current.Session; } }

        public virtual IEnumerable<TEntity> GetAll()
        {
            var query = Session.QueryOver<TEntity>();
            
            return query.List();
        }

        public TEntity SaveOrUpdate(TEntity entity)
        {
            Session.SaveOrUpdate(entity);

            return entity;
        }

        public TEntity Update(TEntity entity)
        {
            Session.Update(entity);

            return entity;
        }

        public TEntity Save(TEntity entity)
        {
            Session.Save(entity);

            return entity;
        }

        public virtual void Delete(TEntity entity)
        {
            Session.Delete(entity);
        }

        public void Purge()
        {
            foreach(var entity in GetAll())
                Delete(entity);
        }

        public virtual TEntity Get(TKey key)
        {
            return Session.Get<TEntity>(key);
        }

        public ICollection<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return Session.Query<TEntity>().Where(predicate).ToArray();
        }

        public ICollection<TEntity> Find(Expression<Func<TEntity, bool>> predicate, int max)
        {
            return Session.Query<TEntity>()
                .Where(predicate)
                .Take(max)
                .ToArray();
        }
    }
}
