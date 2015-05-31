using FluentNHibernate.Mapping;
using GRG.LeisureCards.Data;
using GRG.LeisureCards.DomainModel;

namespace GRG.LeisureCards.Persistence.NHibernate.ClassMaps
{
    public class DataImportJounalEntryClassMap : ClassMap<DataImportJournalEntry>
    {
        public DataImportJounalEntryClassMap()
        {
            Id(x => x.Id);
            Map(x => x.ImportedDateTime);
            Map(x => x.UploadKey);
            Map(x => x.Message);
            Map(x => x.StackTrace).CustomSqlType(Database.GetCustomSqlTypeString(CustomSqlType.NText));
            Map(x => x.FileName);
            Map(x => x.Success);
            Map(x => x.FileAcquiredDateTime);
            Map(x => x.ExecutedDateTime);
        }
    }
}
