using System;

namespace GRG.LeisureCards.DomainModel
{
    public class DataImportJournalEntry
    {
        public virtual int Id { get; set; }

        public virtual DateTime LastRun { get; set; }

        //E.G. Hotels, RedLetter, 2-4-1 offers
        public virtual string UploadKey { get; set; }

        public virtual bool Success { get; set; }

        public virtual string Message { get; set; }

        public virtual string FileName { get; set; }

        public virtual string Status { get; set; }
    }
}
