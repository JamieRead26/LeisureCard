
using System;

namespace GRG.LeisureCards.Model
{
    public class DataImportJournalEntry
    {
        public virtual int Id { get; set; }

        public virtual DateTime ImportedDateTime { get; set; }

        //E.G. Hotels, RedLetter, 2-4-1 offers
        public virtual string OfferTypeKey { get; set; }

        public virtual bool Success { get; set; }

        public virtual string Message { get; set; }

        public virtual string StackTrace { get; set; }

        public virtual string FileKey { get; set; }
    }
}
