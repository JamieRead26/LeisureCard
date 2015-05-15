using Bootstrap4NHibernate.Data;
using GRG.LeisureCards.Model;

namespace GRG.LeisureCards.Data.Test
{
    public class SettingDataFixture : DataFixture
    {
        public override object[] GetEntities(IFixtureContainer fixtureContainer)
        {
            return new []{new Setting{SettingKey = "RenewalPeriodDays", Value="365"}};
        }
    }
}
