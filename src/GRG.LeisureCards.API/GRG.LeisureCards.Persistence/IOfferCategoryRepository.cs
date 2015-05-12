using GRG.LeisureCards.Model;

namespace GRG.LeisureCards.Persistence
{
    public interface IOfferCategoryRepository : IRepository<OfferCategory, int>
    {
        OfferCategory ShortBreaks { get; }
        OfferCategory RedLetter { get; }
        OfferCategory TwoForOne { get; }
    }
}
