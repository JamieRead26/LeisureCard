using FluentNHibernate.Mapping;
using GRG.LeisureCards.DomainModel;

namespace GRG.LeisureCards.Persistence.NHibernate.ClassMaps
{
    public class LocationClassMap : ClassMap<Location>
    {
        public LocationClassMap()
        {
            Id(x => x.UkPostcodeOrTown);
            Map(x => x.Longitude);
            Map(x => x.Latitude);
        }
    }
}
