using System.IO;
using System.Linq;
using Bootstrap4NHibernate.Data;
using GRG.LeisureCards.CSV;
using GRG.LeisureCards.DomainModel;
using GRG.LeisureCards.TestResources;

namespace GRG.LeisureCards.Data.Test
{
    public class TwoForOneOfferDataFixture : DataFixture
    {
        public override object[] GetEntities(IFixtureContainer fixtureContainer)
        {
            using (var csvReader = CsvReader.Create(new StreamReader(ResourceStreams.GetStream(ResourceStreams.TwoForOneName))))
            {
                return csvReader.GetRecords<TwoForOneOffer>().ToArray();
            }
        }
    }
}
