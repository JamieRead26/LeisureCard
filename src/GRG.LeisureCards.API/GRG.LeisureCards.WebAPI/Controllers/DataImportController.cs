using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using GRG.LeisureCards.DomainModel;
using GRG.LeisureCards.Persistence;
using GRG.LeisureCards.Service;
using GRG.LeisureCards.WebAPI.Filters;
using log4net;

namespace GRG.LeisureCards.WebAPI.Controllers
{
    [SessionAuthFilter(true)]
    public class DataImportController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(DataImportController));

        private readonly IDataImportService _dataImportService;
        private readonly IDataImportJournalEntryRepository _dataImportJournalEntryRepository;
        private readonly IFileImportManager _fileImportManager;


        public DataImportController(
            IDataImportService dataImportService,
            IDataImportJournalEntryRepository dataImportJournalEntryRepository,
            IFileImportManager fileImportManager)
        {
            _dataImportService = dataImportService;
            _dataImportJournalEntryRepository = dataImportJournalEntryRepository;
            _fileImportManager = fileImportManager;
        }

        [HttpPost]
        [Route("DataImport/RedLetter/")]
        public DataImportJournalEntry ImportRedLetterData()
        {
            return _fileImportManager.ImportDataFile(
                (bytes, key) => _dataImportService.ImportRedLetterOffers(bytes, key),
                _fileImportManager.RedLetterFilePath, _fileImportManager.GetRedLetterData());
        }
  
        [HttpPost]
        [Route("DataImport/TwoForOne/")]
        public DataImportJournalEntry ImportTwoForOneData()
        {
            var httpRequest = HttpContext.Current.Request;

            return _fileImportManager.ImportDataFile((bytes, key) => _dataImportService.ImportTwoForOneOffers(bytes, key), _fileImportManager.TwoForOneFilePath, httpRequest.Files[0].InputStream);
        }

        [HttpGet]
        [Route("DataImport/GetLastGoodRedLetterImportJournal")]
        public DataImportJournalEntry GetLastGoodRedLetterImportJournal()
        {
            return GetLastImportJournal(true, DataImportKey.RedLetter);
        }
        private DataImportJournalEntry GetLastImportJournal(bool good, DataImportKey importKey)
        {
            return _dataImportJournalEntryRepository.GetLast(good, importKey);
        }

        [HttpGet]
        [Route("DataImport/GetLastBadRedLetterImportJournal")]
        public DataImportJournalEntry GetLastBadRedLetterImportJournal()
        {
            return GetLastImportJournal(false,DataImportKey.RedLetter);
        }

        [HttpGet]
        [Route("DataImport/GetLastGoodTwoForOneImportJournal")]
        public DataImportJournalEntry GetLastGoodTwoForOneImportJournal()
        {
            return GetLastImportJournal(true,DataImportKey.TwoForOne);
        }

        [HttpGet]
        [Route("DataImport/GetLastBadTwoForOneImportJournal")]
        public DataImportJournalEntry GetLastBadTwoForOneImportJournal()
        {
            return GetLastImportJournal(false,DataImportKey.TwoForOne);
        }

        [HttpGet]
        [Route("DataImport/GetRedLetterImportJournal/{count}/{toId}")]
        public IEnumerable<DataImportJournalEntry> GetRedLetterImportJournal(int count, int toId)
        {
            return GetImportJournal(DataImportKey.RedLetter, count, toId);
        }

        [HttpGet]
        [Route("DataImport/GetTwoForOneImportJournal/{count}/{toId}")]
        public IEnumerable<DataImportJournalEntry> GetTwoForOneImportJournal(int count, int toId)
        {
            var results = GetImportJournal(DataImportKey.TwoForOne, count, toId);

            return results;
        }

        private IEnumerable<DataImportJournalEntry> GetImportJournal(DataImportKey importKey, int count, int toId)
        {
            return _dataImportJournalEntryRepository.Get(importKey, count, toId);
        }
    }
}