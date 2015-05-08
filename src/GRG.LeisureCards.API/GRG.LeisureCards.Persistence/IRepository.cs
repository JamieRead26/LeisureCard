using System.Collections.Generic;

namespace GRG.LeisureCards.Persistence
{
    public interface IRepository { }

    public interface IRepository<TEntity, in TKey> : IRepository
    {
        IEnumerable<TEntity> GetAll();

        TEntity SaveOrUpdate(TEntity entity);

        void Delete(TKey entityKey);

        void Delete(TEntity entity);

        TEntity Get(TKey key);
    }
}
