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
        [Ignore("Too long as pull large file")]
        public void RetrieveRedLetterData()
        {
            TestDataImportOp(service => service.RetrieveRedLetterData(), service => service.GetRedLetterImportJournal());
            TestDataImportOp(service => service.ProcessRedLetterData(), service => service.GetRedLetterImportJournal());
        }

        [Test]
        public void RetrieveDeactivateUrns()
        {
            TestDataImportOp(service => service.RetrieveDeactivateUrns("GRG"), service => service.GetDeactivateUrnImportJournal());
            TestDataImportOp(service => service.ProcessDeactivateUrnData("GRG"), service => service.GetDeactivateUrnImportJournal());
        }

        [Test]
        public void RetrieveNewUrns()
        {
            TestDataImportOp(service => service.RetrieveNewUrnsData("GRG"), service => service.GetNewUrnImportJournal());
            TestDataImportOp(service => service.ProcessNewUrnData("GRG", 12, "TEST"), service => service.GetNewUrnImportJournal());
        }

        [Test]
        public void UploadRedLetterData()
        {
            TestDataImportOp(service => service.UploadRedLetterData(ResourceStreams.GetStream(ResourceStreams.RedLetterName)),
                service => service.GetRedLetterImportJournal());
            TestDataImportOp(service => service.ProcessRedLetterData(), service => service.GetRedLetterImportJournal());
        }

        [Test]
        public void UploadNewUrnData()
        {
            TestDataImportOp(service => service.UploadNewUrnData("GRG", ResourceStreams.GetStream(ResourceStreams.NewUrns)),
                service => service.GetNewUrnImportJournal());
            TestDataImportOp(service => service.ProcessNewUrnData("GRG", 12, "TEST" ), service => service.GetNewUrnImportJournal());
        }

        [Test]
        public void UploadDeactivateUrnsData()
        {
            TestDataImportOp(service => service.UploadDeactivateUrnData("GRG", ResourceStreams.GetStream(ResourceStreams.DeactivateUrns)),
                service => service.GetDeactivateUrnImportJournal());
            TestDataImportOp(service => service.ProcessDeactivateUrnData("GRG"), service => service.GetDeactivateUrnImportJournal());
        }

        [Test]
        [Ignore("Too long as hits google API, will reduce test data")]
        public void Upload241Data()
        {
            TestDataImportOp(service => service.Upload241Data(ResourceStreams.GetStream(ResourceStreams.TwoForOneName)),
                service => service.Get241ImportJournal());
            TestDataImportOp(service => service.Process241Data(), service => service.Get241ImportJournal());
        }

        public void TestDataImportOp(Func<IDataImportService, DataImportJournalEntry> op,
            Func<IDataImportService, DataImportJournalEntry> getJournal)
        {
            var service = AdminSession.GetDataImportService();
            var journalEntry = op(service);
            var journalEntry2 = getJournal(service);

            Assert.IsTrue(journalEntry.Success);
            Assert.AreEqual(journalEntry.Id, journalEntry2.Id);
        }
    }
}
