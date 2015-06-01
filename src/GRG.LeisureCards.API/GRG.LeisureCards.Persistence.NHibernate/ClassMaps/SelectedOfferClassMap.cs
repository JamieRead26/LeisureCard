using FluentNHibernate.Mapping;
using GRG.LeisureCards.DomainModel;

namespace GRG.LeisureCards.Persistence.NHibernate.ClassMaps
{
    public class SelectedOfferClassMap : ClassMap<SelectedOffer>
    {
        public SelectedOfferClassMap()
        {
            Id(x => x.Id);
            Map(x => x.OfferId);
            Map(x => x.OfferTitle);
            References(x => x.OfferCategory);
            Map(x => x.LeisureCardCode);
            Map(x => x.SelectedDateTime);
        }
    }
}
