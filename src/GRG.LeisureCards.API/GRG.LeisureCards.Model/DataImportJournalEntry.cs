
using System;

namespace GRG.LeisureCards.Model
{
    public class DataImportJournalEntry
    {
        public virtual int Id { get; set; }

        public virtual DateTime ImportedUtc { get; set; }

        //E.G. Hotels, RedLetter, 2-4-1 offers
        public virtual string Key { get; set; }
    }
}
