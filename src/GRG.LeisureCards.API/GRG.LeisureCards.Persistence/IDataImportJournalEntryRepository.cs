using System.Collections.Generic;
using GRG.LeisureCards.Model;

namespace GRG.LeisureCards.Persistence
{
    public interface IDataImportJournalEntryRepository : IRepository<DataImportJournalEntry, int>
    {
        IEnumerable<DataImportJournalEntry> Get(DataImportKey importKey, int count, int toId);
    }
}
