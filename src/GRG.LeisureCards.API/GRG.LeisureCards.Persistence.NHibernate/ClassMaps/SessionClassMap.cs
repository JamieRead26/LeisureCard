using FluentNHibernate.Mapping;
using GRG.LeisureCards.DomainModel;

namespace GRG.LeisureCards.Persistence.NHibernate.ClassMaps
{
    public class SessionClassMap : ClassMap<Session>
    {
        public SessionClassMap()
        {
            Id(x => x.CardCode);
            Map(x => x.Token);
            Map(x => x.ExpiryUtc);
            Map(x => x.IsAdmin);
            Map(x => x.CardRenewalDate);
        }
    }
}
