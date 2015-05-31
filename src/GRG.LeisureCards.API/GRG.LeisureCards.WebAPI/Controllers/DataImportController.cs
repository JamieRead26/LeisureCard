using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using AutoMapper;
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
        public void ImportRedLetterData()
        {
            ThreadPool.QueueUserWorkItem(state=>
                _dataImportJournalEntryRepository.SaveOrUpdate(
                    _fileImportManager.StoreDataFile(DataImportKey.RedLetter, DataImportKey.RedLetter.UploadPath, _fileImportManager.GetRedLetterData())));
        }

        [HttpPost]
        [Route("DataImport/ProcessRedLetter/")]
        public Model.DataImportJournalEntry ProcessRedLetterData()
        {
            var latest = _dataImportJournalEntryRepository.GetLast(true, DataImportKey.RedLetter);

            _dataImportService.Import(latest);

            return Mapper.Map<Model.DataImportJournalEntry>(latest);
        }
  
        [HttpPost]
        [Route("DataImport/TwoForOne/")]
        public Model.DataImportJournalEntry ImportTwoForOneData()
        {
            var httpRequest = HttpContext.Current.Request;

            return Mapper.Map<Model.DataImportJournalEntry>(_dataImportJournalEntryRepository.SaveOrUpdate(
                _fileImportManager.StoreDataFile(DataImportKey.TwoForOne,
                DataImportKey.TwoForOne.UploadPath, httpRequest.Files[0].InputStream)));
        }


        [HttpPost]
        [Route("DataImport/ProcessTwoForOne/")]
        public Model.DataImportJournalEntry ProcessTwoForOne()
        {
            var latest = _dataImportJournalEntryRepository.GetLast(true, DataImportKey.TwoForOne);

            _dataImportService.Import(latest);

            return Mapper.Map<Model.DataImportJournalEntry>(latest);
        }

        [HttpGet]
        [Route("DataImport/GetLastGoodRedLetterImportJournal")]
        public Model.DataImportJournalEntry GetLastGoodRedLetterImportJournal()
        {
            return GetLastImportJournal(true, DataImportKey.RedLetter);
        }
        private Model.DataImportJournalEntry GetLastImportJournal(bool good, DataImportKey importKey)
        {
            return Mapper.Map<Model.DataImportJournalEntry>(_dataImportJournalEntryRepository.GetLast(good, importKey));
        }

        [HttpGet]
        [Route("DataImport/GetLastBadRedLetterImportJournal")]
        public Model.DataImportJournalEntry GetLastBadRedLetterImportJournal()
        {
            return GetLastImportJournal(false,DataImportKey.RedLetter);
        }

        [HttpGet]
        [Route("DataImport/GetLastGoodTwoForOneImportJournal")]
        public Model.DataImportJournalEntry GetLastGoodTwoForOneImportJournal()
        {
            return GetLastImportJournal(true,DataImportKey.TwoForOne);
        }

        [HttpGet]
        [Route("DataImport/GetLastBadTwoForOneImportJournal")]
        public Model.DataImportJournalEntry GetLastBadTwoForOneImportJournal()
        {
            return GetLastImportJournal(false,DataImportKey.TwoForOne);
        }

        [HttpGet]
        [Route("DataImport/GetRedLetterImportJournal/{count}/{toId}")]
        public IEnumerable<Model.DataImportJournalEntry> GetRedLetterImportJournal(int count, int toId)
        {
            return GetImportJournal(DataImportKey.RedLetter, count, toId);
        }

        [HttpGet]
        [Route("DataImport/GetTwoForOneImportJournal/{count}/{toId}")]
        public IEnumerable<Model.DataImportJournalEntry> GetTwoForOneImportJournal(int count, int toId)
        {
            var results = GetImportJournal(DataImportKey.TwoForOne, count, toId);

            return results;
        }

        private IEnumerable<Model.DataImportJournalEntry> GetImportJournal(DataImportKey importKey, int count, int toId)
        {
            return _dataImportJournalEntryRepository.Get(importKey, count, toId).Select(Mapper.Map<Model.DataImportJournalEntry>);
        }
    }
}