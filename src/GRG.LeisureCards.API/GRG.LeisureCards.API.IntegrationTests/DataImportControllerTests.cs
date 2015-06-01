using System;
using GRG.LeisureCards.TestResources;
using GRG.LeisureCards.WebAPI.ClientContract;
using NUnit.Framework;
using DataImportJournalEntry = GRG.LeisureCards.WebAPI.Model.DataImportJournalEntry;

namespace GRG.LeisureCards.API.IntegrationTests
{
    [TestFixture]
    public class DataImportControllerTests : ControllerTests
    {
        [Test]
        public void RetrieveRedLetterData()
        {
            TestDataImportOp(service => service.RetrieveRedLetterData(), service => service.GetRedLetterImportJournal());
            TestDataImportOp(service => service.ProcessRedLetterData(), service => service.GetRedLetterImportJournal());
        }

        [Test]
        public void UploadRedLetterData()
        {
            TestDataImportOp(service => service.UploadRedLetterData(ResourceStreams.GetRedLetterDataStream()),
                service => service.GetRedLetterImportJournal());
            TestDataImportOp(service => service.ProcessRedLetterData(), service => service.GetRedLetterImportJournal());
        }

        [Test]
        public void Upload241Data()
        {
            TestDataImportOp(service => service.Upload241Data(ResourceStreams.Get241LetterDataStream()),
                service => service.Get241ImportJournal());
            TestDataImportOp(service => service.Process241Data(), service => service.Get241ImportJournal());
        }


        public void TestDataImportOp(Func<IDataImportService, DataImportJournalEntry> op,
            Func<IDataImportService, DataImportJournalEntry> getJournal)
        {
            var service = AdminSession.GetDataImportService();
            var journalEntry = op(service);
            Assert.IsTrue(journalEntry.Success);
            Assert.AreEqual(journalEntry.Id, getJournal(service).Id);
        }
    }
}
