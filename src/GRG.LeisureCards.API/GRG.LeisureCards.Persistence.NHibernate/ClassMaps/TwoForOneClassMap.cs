using FluentNHibernate.Mapping;
using GRG.LeisureCards.Data;
using GRG.LeisureCards.DomainModel;

namespace GRG.LeisureCards.Persistence.NHibernate.ClassMaps
{
    public class TwoForOneClassMap : ClassMap<TwoForOneOffer>
    {
        public TwoForOneClassMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();
            Map(x => x.OutletName).CustomSqlType(Database.GetCustomSqlTypeString(CustomSqlType.NText));
            Map(x => x.Address1).CustomSqlType(Database.GetCustomSqlTypeString(CustomSqlType.NText));
            Map(x => x.Address2).CustomSqlType(Database.GetCustomSqlTypeString(CustomSqlType.NText));
            Map(x => x.TownCity);
            Map(x => x.County);
            Map(x => x.PostCode);
            Map(x => x.Phone);
            Map(x => x.Website);
            Map(x => x.Description).CustomSqlType(Database.GetCustomSqlTypeString(CustomSqlType.NText));
            Map(x => x.DisabledAccess).CustomSqlType(Database.GetCustomSqlTypeString(CustomSqlType.NText)); 
            Map(x => x.BookingInstructions1).CustomSqlType(Database.GetCustomSqlTypeString(CustomSqlType.NText));
            Map(x => x.BookingInstructions2).CustomSqlType(Database.GetCustomSqlTypeString(CustomSqlType.NText));
            Map(x => x.BookingInstructions3).CustomSqlType(Database.GetCustomSqlTypeString(CustomSqlType.NText));
            Map(x => x.BookingInstructions4).CustomSqlType(Database.GetCustomSqlTypeString(CustomSqlType.NText));
            Map(x => x.BookingInstructions5).CustomSqlType(Database.GetCustomSqlTypeString(CustomSqlType.NText));
            Map(x => x.BookingInstructions6).CustomSqlType(Database.GetCustomSqlTypeString(CustomSqlType.NText));
            Map(x => x.BookingInstructions7).CustomSqlType(Database.GetCustomSqlTypeString(CustomSqlType.NText));
            Map(x => x.ClaimCode);
            Map(x => x.Latitude);
            Map(x => x.Longitude);
            Map(x => x.CategoryKey);
        }
    }
}
