namespace GRG.LeisureCards.Persistence
{
    public interface IRepository { }

    public interface IRepository<TEntity, in TKey> : IRepository
    {
        TEntity SaveOrUpdate(TEntity entity);

        void Delete(TKey entityKey);

        TEntity Get(TKey key);
    }
}
