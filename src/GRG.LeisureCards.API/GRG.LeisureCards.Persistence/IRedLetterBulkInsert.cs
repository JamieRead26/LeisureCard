using System.Collections.Generic;
using GRG.LeisureCards.DomainModel;

namespace GRG.LeisureCards.Persistence
{
    public interface IRedLetterBulkInsert
    {
        void Insert(IEnumerable<RedLetterProduct> inserts, IEnumerable<RedLetterProduct> updates,
            IEnumerable<RedLetterProduct> deletes, DataImportJournalEntry journalEntry);

        IEnumerable<RedLetterProduct> GetAll();
    }
}
