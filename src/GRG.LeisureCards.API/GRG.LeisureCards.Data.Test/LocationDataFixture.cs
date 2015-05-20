using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bootstrap4NHibernate.Data;
using GRG.LeisureCards.DomainModel;

namespace GRG.LeisureCards.Data.Test
{
    public class LocationDataFixture : DataFixture
    {
        public override object[] GetEntities(IFixtureContainer fixtureContainer)
        {
            return new[]
            {
                new Location {UkPostcodeOrTown = "AB42 5FQ", Latitude = -2.0328417, Longitude = 57.521767},
                new Location {UkPostcodeOrTown = "GU11 2LG", Latitude = -0.750587, Longitude = 51.269788},
                new Location {UkPostcodeOrTown = "EH54 7AR", Latitude = -3.5473646, Longitude = 55.8858884},
                new Location {UkPostcodeOrTown = "PO36 0LX", Latitude = -1.2188181, Longitude = 50.655247},
                new Location {UkPostcodeOrTown = "AB42 1QD", Latitude = -1.7820999, Longitude = 57.5067986},
                new Location {UkPostcodeOrTown = "DT2 7LG", Latitude = -2.3266657, Longitude = 50.7465048},
                new Location {UkPostcodeOrTown = "SN11 0NF", Latitude = -1.9947303, Longitude = 51.4242482},
                new Location {UkPostcodeOrTown = "PH6 2JS", Latitude = -4.0083804, Longitude = 56.3716033},
                new Location {UkPostcodeOrTown = "HP9 2PL", Latitude = -0.6440456, Longitude = 51.6130006},
                new Location {UkPostcodeOrTown = "BT93 3FY", Latitude = -8.0948138, Longitude = 54.4800769},
                new Location {UkPostcodeOrTown = "GL7 5NL", Latitude = -8.0948138, Longitude = 51.7595855},
                new Location {UkPostcodeOrTown = "GU10 4LD", Latitude = -0.8418917, Longitude = 51.1816311},
                new Location {UkPostcodeOrTown = "ST13 7QR", Latitude = -1.9236659, Longitude = 53.0610022},
                new Location {UkPostcodeOrTown = "DY11 5SY", Latitude = -2.2836055, Longitude = 52.4302755},
                new Location {UkPostcodeOrTown = "FK21 8XE", Latitude = -4.3206641, Longitude = 56.4632702},
                new Location {UkPostcodeOrTown = "SP6 2DF", Latitude = -1.7839721, Longitude = 50.9706489},
                new Location {UkPostcodeOrTown = "DD9 6RL", Latitude = -2.6910477, Longitude = 56.7284757},
                new Location {UkPostcodeOrTown = "PL32 9TZ", Latitude = -4.6887048, Longitude = 50.6390618},
                new Location {UkPostcodeOrTown = "PR8 5AJ", Latitude = -3.0049434, Longitude = 53.6394135},
                new Location {UkPostcodeOrTown = "SK12 1BY", Latitude = -2.1150463, Longitude = 53.3615038},
            };
        }
    }
}
