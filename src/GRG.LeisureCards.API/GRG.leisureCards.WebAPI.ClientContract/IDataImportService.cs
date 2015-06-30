using System.IO;
using GRG.LeisureCards.WebAPI.Model;

namespace GRG.LeisureCards.WebAPI.ClientContract
{
    public interface IDataImportService
    {
        DataImportJournalEntry RetrieveRedLetterData();

        DataImportJournalEntry RetrieveNewUrnsData(string tenantKey);

        DataImportJournalEntry RetrieveDeactivateUrns(string tenantKey);

        DataImportJournalEntry UploadRedLetterData(Stream stream);
        DataImportJournalEntry ProcessRedLetterData();
        DataImportJournalEntry GetRedLetterImportJournal();
        DataImportJournalEntry Upload241Data(Stream stream);
        DataImportJournalEntry Process241Data();
        DataImportJournalEntry Get241ImportJournal();
        DataImportJournalEntry GetNewUrnImportJournal(string tenantKey);
        DataImportJournalEntry UploadNewUrnData(string tenantKey, Stream stream);
        DataImportJournalEntry ProcessNewUrnData(string tenantKey, int renewalPeriodMonths, string reference);
        DataImportJournalEntry GetDeactivateUrnImportJournal(string tenantKey);
        DataImportJournalEntry UploadDeactivateUrnData(string tenantKey, Stream stream);
        DataImportJournalEntry ProcessDeactivateUrnData(string tenantKey);
    }
}
