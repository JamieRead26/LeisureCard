namespace GRG.LeisureCards.Persistence
{
    public interface IRepository { }

    public interface IRepository<TEntity, in TKey> : IRepository
    {
        TEntity SaveOrUpdate(TEntity entity);

        void Delete(TKey entityKey);

        void Delete(TEntity entity);

        TEntity Get(TKey key);
    }
}
