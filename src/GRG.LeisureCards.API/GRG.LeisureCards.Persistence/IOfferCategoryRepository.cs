using GRG.LeisureCards.DomainModel;

namespace GRG.LeisureCards.Persistence
{
    public interface IOfferCategoryRepository : IRepository<OfferCategory, int>
    {
        OfferCategory ShortBreaks { get; }
        OfferCategory RedLetter { get; }
        OfferCategory TwoForOne { get; }
    }
}
