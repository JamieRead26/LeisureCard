using GRG.LeisureCards.DomainModel;

namespace GRG.LeisureCards.Persistence.NHibernate
{
    public class RedLetterProductRepository : Repository<RedLetterProduct, int>, IRedLetterProductRepository
    {}
}
