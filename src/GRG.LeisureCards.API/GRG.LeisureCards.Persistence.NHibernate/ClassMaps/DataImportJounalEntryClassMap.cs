using FluentNHibernate.Mapping;
using GRG.LeisureCards.Data;
using GRG.LeisureCards.Model;

namespace GRG.LeisureCards.Persistence.NHibernate.ClassMaps
{
    public class DataImportJounalEntryClassMap : ClassMap<DataImportJournalEntry>
    {
        public DataImportJounalEntryClassMap()
        {
            Id(x => x.Id);
            Map(x => x.ImportedDateTime);
            Map(x => x.Key);
            Map(x => x.Message);
            Map(x => x.StackTrace).CustomSqlType(Database.GetCustomSqlTypeString(CustomSqlType.NText)); 
        }
    }
}
