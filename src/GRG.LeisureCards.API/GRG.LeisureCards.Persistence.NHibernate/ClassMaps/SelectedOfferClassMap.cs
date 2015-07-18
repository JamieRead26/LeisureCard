using FluentNHibernate.Mapping;
using GRG.LeisureCards.Data;
using GRG.LeisureCards.DomainModel;

namespace GRG.LeisureCards.Persistence.NHibernate.ClassMaps
{
    public class SelectedOfferClassMap : ClassMap<SelectedOffer>
    {
        public SelectedOfferClassMap()
        {
            Id(x => x.Id);
            Map(x => x.OfferId).UniqueKey("OfferId_SessionToken");
            Map(x => x.OfferTitle).CustomSqlType(Database.GetCustomSqlTypeString(CustomSqlType.NText));
            References(x => x.OfferCategory);
            Map(x => x.LeisureCardCode);
            Map(x => x.SelectedDateTime);
            Map(x => x.SessionToken).UniqueKey("OfferId_SessionToken");
        }
    }
}
