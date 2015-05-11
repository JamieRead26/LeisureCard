using Bootstrap4NHibernate.Data;
using GRG.LeisureCards.Model;

namespace GRG.LeisureCards.Data.Test
{
    public class OfferCategoryDataFixture : DataFixture
    {
        public readonly OfferCategory RedLetter = new OfferCategory {Key = "RL", Name = "Red Letter"};
        public readonly OfferCategory TwoForOne = new OfferCategory { Key = "241", Name = "2-4-1" };
        public readonly OfferCategory ShortBreaks = new OfferCategory { Key = "SB", Name = "Short Breaks" };
        
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
