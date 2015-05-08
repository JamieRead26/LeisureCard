using System.IO;
using System.Linq;
using System.Reflection;
using Bootstrap4NHibernate.Data;
using CsvHelper;
using CsvHelper.Configuration;
using GRG.LeisureCards.Model;

namespace GRG.LeisureCards.Data.Test
{
    public class TwoForOneOfferDataFixture : DataFixture
    {
        public override object[] GetEntities(IFixtureContainer fixtureContainer)
        {
            var config = new CsvConfiguration();
            config.RegisterClassMap<TwoForOneOfferClassMap>();
            config.TrimFields = true;

            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("GRG.LeisureCards.Data.Test.241_Sample_Data_v1.0.csv"))
            using (var csvReader = new CsvReader(new StreamReader(stream), config))
            {
                return csvReader.GetRecords<TwoForOneOffer>().ToArray();
            }
        }
    }

    public sealed class TwoForOneOfferClassMap : CsvClassMap<TwoForOneOffer>
    {
        public TwoForOneOfferClassMap()
        {
            Map(m => m.Id).Ignore(true);
            Map( m => m.OutletName ).Name( "Outlet Name" );
            Map(m => m.Address1).Name("Address line 1");
            Map(m => m.Address2).Name("Address line 2");
            Map(m => m.TownCity).Name("Town/city");
            Map(m => m.County).Name("County");
            Map(m => m.PostCode).Name("Postcode");
            Map(m => m.Phone).Name("Phone  ");
            Map(m => m.Website).Name("Website");
            Map(m => m.Description).Name("Description");
            Map(m => m.DisabledAccess).Name("Disabled access");
        }
    }
}
