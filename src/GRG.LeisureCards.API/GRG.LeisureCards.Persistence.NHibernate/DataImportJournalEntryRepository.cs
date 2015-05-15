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

            query.Where(j => j.OfferTypeKey == importKey.Key)
                .OrderBy(x => x.Id).Desc
                .Take(count);

            var result = query.List();

            return result;
        }

        public DataImportJournalEntry GetLast(bool good, DataImportKey importKey)
        {
            var query = Session.QueryOver<DataImportJournalEntry>();

            query.Where(j => j.OfferTypeKey == importKey.Key)
                .Where(j => j.Success == good)
                .OrderBy(x => x.Id).Desc
                .Take(1);

            var result = query.List();

            return result.Count==0 ? null : result[0];
        }
    }
}
