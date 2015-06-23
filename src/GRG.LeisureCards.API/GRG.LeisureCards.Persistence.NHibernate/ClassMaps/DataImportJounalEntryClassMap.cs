using FluentNHibernate.Mapping;
using GRG.LeisureCards.DomainModel;

namespace GRG.LeisureCards.Persistence.NHibernate.ClassMaps
{
    public class 
        DataImportJounalEntryClassMap : ClassMap<DataImportJournalEntry>
    {
        public DataImportJounalEntryClassMap()
        {
            Id(x => x.Id);
            Map(x => x.LastRun);
            Map(x => x.UploadKey);
            Map(x => x.Message);
            Map(x => x.FileName);
            Map(x => x.Success);
            References(x => x.Tenant);
            Map(x => x.Supplemental);
        }
    }
}
