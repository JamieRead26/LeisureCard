namespace GRG.LeisureCards.Persistence
{
    public interface IRepository<TEntity, in TKey>
    {
        TEntity SaveOrUpdate(TEntity entity);

        void Delete(TKey entityKey);

        TEntity Get(TKey key);
    }
}
