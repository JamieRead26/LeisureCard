using System.Collections.Generic;
using System.Linq;
using GRG.LeisureCards.Model;
using NHibernate.Linq;

namespace GRG.LeisureCards.Persistence.NHibernate
{
    public class DataImportJournalEntryRepository : Repository<DataImportJournalEntry, int>, IDataImportJournalEntryRepository
    {
        public IEnumerable<DataImportJournalEntry> Get(DataImportKey importKey, int count, int toId)
        {
            var query = Session.Query<DataImportJournalEntry>()
                .Take(count)
                .Where(j => j.Key == importKey.Key);

            if (toId > 0)
                query.Where(j => j.Id <= toId);

            var result = query.ToArray();

            return result;
        }
    }
}
