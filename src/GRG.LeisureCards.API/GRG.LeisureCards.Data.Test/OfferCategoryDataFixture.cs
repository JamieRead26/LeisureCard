using Bootstrap4NHibernate.Data;
using GRG.LeisureCards.Model;

namespace GRG.LeisureCards.Data.Test
{
    public class OfferCategoryDataFixture : DataFixture
    {
        public readonly OfferCategory RedLetter = new OfferCategory {OfferCategoryKey = "RL", Name = "Red Letter"};
        public readonly OfferCategory TwoForOne = new OfferCategory { OfferCategoryKey = "241", Name = "2-4-1" };
        public readonly OfferCategory ShortBreaks = new OfferCategory { OfferCategoryKey = "SB", Name = "Short Breaks" };
        
        public OfferCategory[] All 
        {
            get
            {
                return new[] {RedLetter, TwoForOne, ShortBreaks};
                
            } 
        }

        public override object[] GetEntities(IFixtureContainer fixtureContainer)
        {
            return new[]
            {
                RedLetter,
                TwoForOne,
                ShortBreaks
            };
        }
    }
}
