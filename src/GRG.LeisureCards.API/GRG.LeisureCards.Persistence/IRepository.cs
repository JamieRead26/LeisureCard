using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace GRG.LeisureCards.Persistence
{
    public interface IRepository {
        void Purge();
    }

    public interface IRepository<TEntity, in TKey> : IRepository
    {
        IEnumerable<TEntity> GetAll();

        TEntity SaveOrUpdate(TEntity entity);

        TEntity Update(TEntity entity);

        TEntity Save(TEntity entity);

        void Delete(TEntity entity);

        TEntity Get(TKey key);

        ICollection<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

        ICollection<TEntity> Find(Expression<Func<TEntity, bool>> predicate, int max);
    }
}
