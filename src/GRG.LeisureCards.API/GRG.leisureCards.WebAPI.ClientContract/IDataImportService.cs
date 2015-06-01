using System.IO;
using GRG.LeisureCards.WebAPI.Model;

namespace GRG.LeisureCards.WebAPI.ClientContract
{
    public interface IDataImportService
    {
        DataImportJournalEntry RetrieveRedLetterData();
        DataImportJournalEntry UploadRedLetterData(Stream stream);
        DataImportJournalEntry ProcessRedLetterData();
        DataImportJournalEntry GetRedLetterImportJournal();
        DataImportJournalEntry Upload241Data(Stream stream);
        DataImportJournalEntry Process241Data();
        DataImportJournalEntry Get241ImportJournal();
    }
}
