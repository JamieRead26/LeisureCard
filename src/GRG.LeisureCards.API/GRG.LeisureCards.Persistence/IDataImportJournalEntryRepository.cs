using System.Collections.Generic;
using GRG.LeisureCards.DomainModel;

namespace GRG.LeisureCards.Persistence
{
    public interface IDataImportJournalEntryRepository : IRepository<DataImportJournalEntry, int>
    {
        IEnumerable<DataImportJournalEntry> Get(DataImportKey importKey, int count, int toId);
        DataImportJournalEntry GetLast(bool good, DataImportKey importKey);
    }
}
