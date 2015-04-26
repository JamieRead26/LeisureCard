using Bootstrap4NHibernate.Data;
using GRG.LeisureCards.Model;

namespace GRG.LeisureCards.Data.Test
{
    public class SettingDataFixture : DataFixture
    {
        public override object[] GetEntities(IFixtureContainer fixtureContainer)
        {
            return new object[]{new Setting{Key = "RenewalPeriodDays", Value="365"}};
        }
    }
}
