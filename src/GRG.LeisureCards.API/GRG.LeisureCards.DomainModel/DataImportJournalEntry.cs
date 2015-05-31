using System;

namespace GRG.LeisureCards.DomainModel
{
    public class DataImportJournalEntry
    {
        public virtual int Id { get; set; }

        public virtual DateTime? ImportedDateTime { get; set; }

        public virtual DateTime? FileAcquiredDateTime { get; set; }

        //E.G. Hotels, RedLetter, 2-4-1 offers
        public virtual string UploadKey { get; set; }

        public virtual bool Success { get; set; }

        public virtual string Message { get; set; }

        public virtual string StackTrace { get; set; }

        public virtual string FileName { get; set; }

        public virtual bool FileImported { get { return ImportedDateTime.HasValue; } }

        public virtual bool FileAcquired { get { return FileAcquiredDateTime.HasValue; } }
        
        public virtual DateTime ExecutedDateTime { get; set; }
    }
}
