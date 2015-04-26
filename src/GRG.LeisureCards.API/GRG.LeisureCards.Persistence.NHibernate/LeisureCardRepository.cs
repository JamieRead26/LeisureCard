using GRG.LeisureCards.Model;

namespace GRG.LeisureCards.Persistence.NHibernate
{
    public class LeisureCardRepository : Repository<LeisureCard, string>, ILeisureCardRepository
    {
    }
}
