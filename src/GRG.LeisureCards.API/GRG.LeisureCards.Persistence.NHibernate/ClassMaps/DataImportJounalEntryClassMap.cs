using FluentNHibernate.Mapping;
using GRG.LeisureCards.Model;

namespace GRG.LeisureCards.Persistence.NHibernate.ClassMaps
{
    public class DataImportJounalEntryClassMap : ClassMap<DataImportJournalEntry>
    {
        public DataImportJounalEntryClassMap()
        {
            Id(x => x.Id);
            Map(x => x.ImportedUtc);
            Map(x => x.Key);
        }
    }
}
