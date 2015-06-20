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
                new Location {UkPostcodeOrTown = "AB42 5FQ",    Longitude = -2.0328417, Latitude = 57.521767},
                new Location {UkPostcodeOrTown = "GU11 2LG",    Longitude = -0.750587,  Latitude = 51.269788},
                new Location {UkPostcodeOrTown = "EH54 7AR",    Longitude = -3.5473646, Latitude = 55.8858884},
                new Location {UkPostcodeOrTown = "PO36 0LX",    Longitude = -1.2188181, Latitude = 50.655247},
                new Location {UkPostcodeOrTown = "AB42 1QD",    Longitude = -1.7820999, Latitude = 57.5067986},
                new Location {UkPostcodeOrTown = "DT2 7LG",     Longitude = -2.3266657, Latitude = 50.7465048},
                new Location {UkPostcodeOrTown = "SN11 0NF",    Longitude = -1.9947303, Latitude = 51.4242482},
                new Location {UkPostcodeOrTown = "PH6 2JS",     Longitude = -4.0083804, Latitude = 56.3716033},
                new Location {UkPostcodeOrTown = "HP9 2PL",     Longitude = -0.6440456, Latitude = 51.6130006},
                new Location {UkPostcodeOrTown = "BT93 3FY",    Longitude = -8.0948138, Latitude = 54.4800769},
                new Location {UkPostcodeOrTown = "GL7 5NL",     Longitude = -8.0948138, Latitude = 51.7595855},
                new Location {UkPostcodeOrTown = "GU10 4LD",    Longitude = -0.8418917, Latitude = 51.1816311},
                new Location {UkPostcodeOrTown = "ST13 7QR",    Longitude = -1.9236659, Latitude = 53.0610022},
                new Location {UkPostcodeOrTown = "DY11 5SY",    Longitude = -2.2836055, Latitude = 52.4302755},
                new Location {UkPostcodeOrTown = "FK21 8XE",    Longitude = -4.3206641, Latitude = 56.4632702},
                new Location {UkPostcodeOrTown = "SP6 2DF",     Longitude = -1.7839721, Latitude = 50.9706489},
                new Location {UkPostcodeOrTown = "DD9 6RL",     Longitude = -2.6910477, Latitude = 56.7284757},
                new Location {UkPostcodeOrTown = "PL32 9TZ",    Longitude = -4.6887048, Latitude = 50.6390618},
                new Location {UkPostcodeOrTown = "PR8 5AJ",     Longitude = -3.0049434, Latitude = 53.6394135},
                new Location {UkPostcodeOrTown = "SK12 1BY",    Longitude = -2.1150463, Latitude = 53.3615038},
            };
        }

       
    }
}
