using System.Collections.Generic;
using GRG.LeisureCards.Model;

namespace GRG.LeisureCards.Persistence.NHibernate
{
    public class DataImportJournalEntryRepository : Repository<DataImportJournalEntry, int>, IDataImportJournalEntryRepository
    {
        public IEnumerable<DataImportJournalEntry> Get(DataImportKey importKey, int count, int toId)
        {
            var query = Session.QueryOver<DataImportJournalEntry>();

            if (toId > 0)
                query.Where(j => j.Id < toId);

            query.Where(j => j.Key == importKey.Key)
                .OrderBy(x => x.Id).Desc
                .Take(count);

            var result = query.List();

            return result;
        }
    }
}
