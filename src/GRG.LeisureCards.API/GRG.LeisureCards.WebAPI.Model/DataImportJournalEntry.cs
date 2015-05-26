using System;

namespace GRG.LeisureCards.WebAPI.Model
{
    public class DataImportJournalEntry
    {
        public int Id { get; set; }

        public DateTime ImportedDateTime { get; set; }

        //E.G. RedLetter, 2-4-1 offers
        public string UploadKey { get; set; }

        public bool Success { get; set; }

        public string Message { get; set; }

        public string StackTrace { get; set; }

        public string FileKey { get; set; }
    }
}
