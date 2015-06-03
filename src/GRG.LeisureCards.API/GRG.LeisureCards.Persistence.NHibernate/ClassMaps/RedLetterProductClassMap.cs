using FluentNHibernate.Mapping;
using GRG.LeisureCards.Data;
using GRG.LeisureCards.DomainModel;

namespace GRG.LeisureCards.Persistence.NHibernate.ClassMaps
{
    public class RedLetterProductClassMap : ClassMap<RedLetterProduct>
    {
        public RedLetterProductClassMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();
            Map(x => x.Title).CustomSqlType(Database.GetCustomSqlTypeString(CustomSqlType.NText)); 
            Map(x => x.InspirationalDescription).CustomSqlType(Database.GetCustomSqlTypeString(CustomSqlType.NText));
            Map(x => x.VoucherText).CustomSqlType(Database.GetCustomSqlTypeString(CustomSqlType.NText)); 
            Map(x => x.ExpRef);
            Map(x => x.Type);
            Map(x => x.GeneralPrice);
            Map(x => x.PriceBeforeVAT);
           // HasMany(x => x.Venues).Inverse().Cascade.All();
            Map(x => x.Territory).CustomSqlType(Database.GetCustomSqlTypeString(CustomSqlType.NText)); 
            Map(x => x.DisplayLocations).CustomSqlType(Database.GetCustomSqlTypeString(CustomSqlType.NText));
            Map(x => x.MainSectionName).CustomSqlType(Database.GetCustomSqlTypeString(CustomSqlType.NText));
            Map(x => x.SectionName).CustomSqlType(Database.GetCustomSqlTypeString(CustomSqlType.NText)); 
            Map(x => x.Priority);
           // HasMany(x => x.Facts).Inverse().Cascade.All();
            Map(x => x.WhatsIncluded).CustomSqlType(Database.GetCustomSqlTypeString(CustomSqlType.NText)); 
            Map(x => x.Availability).CustomSqlType(Database.GetCustomSqlTypeString(CustomSqlType.NText)); 
            Map(x => x.Weather).CustomSqlType(Database.GetCustomSqlTypeString(CustomSqlType.NText));
            Map(x => x.Duration).CustomSqlType(Database.GetCustomSqlTypeString(CustomSqlType.NText)); 
            Map(x => x.ShortDuration).CustomSqlType(Database.GetCustomSqlTypeString(CustomSqlType.NText)); 
            Map(x => x.HowManyPeople).CustomSqlType(Database.GetCustomSqlTypeString(CustomSqlType.NText));
            Map(x => x.FriendsAndFamily).CustomSqlType(Database.GetCustomSqlTypeString(CustomSqlType.NText));
            Map(x => x.DressCode).CustomSqlType(Database.GetCustomSqlTypeString(CustomSqlType.NText)); 
            Map(x => x.AnyOtherInfo).CustomSqlType(Database.GetCustomSqlTypeString(CustomSqlType.NText)); 
            Map(x => x.WhoCanTakePart).CustomSqlType(Database.GetCustomSqlTypeString(CustomSqlType.NText)); 
            Map(x => x.WhereIsItHeld).CustomSqlType(Database.GetCustomSqlTypeString(CustomSqlType.NText)); 
            Map(x => x.HowToGetThere).CustomSqlType(Database.GetCustomSqlTypeString(CustomSqlType.NText)); 
            Map(x => x.PermaLink);
            Map(x => x.Url);
            Map(x => x.ImageUrl);
            Map(x => x.ThumbnailUrl);
            Map(x => x.LargeImageName);
            Map(x => x.IsSpecialOffer);
            Map(x => x.DeliveryTime).CustomSqlType(Database.GetCustomSqlTypeString(CustomSqlType.NText)); 
            Map(x => x.DeliveryCost).CustomSqlType(Database.GetCustomSqlTypeString(CustomSqlType.NText));

            //HasManyToMany(x => x.Keywords)
            //    .Cascade.All()
            //    .Table("ProductKeywords");
        }
    }
}
