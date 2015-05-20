using FluentNHibernate.Mapping;
using GRG.LeisureCards.DomainModel;

namespace GRG.LeisureCards.Persistence.NHibernate.ClassMaps
{
    public class RedLetterVenueClassMap: ClassMap<RedLetterVenue>
    {
        public RedLetterVenueClassMap()
        {
            Id(x => x.Id);
            Map(x => x.RedLetterId);
            Map(x => x.Name);
            Map(x => x.County);
            Map(x => x.Town);
            Map(x => x.PostCode);
            Map(x => x.Latitude);
            Map(x => x.Longitude);
            References(x => x.RedLetterProduct);
        }
    }
}
