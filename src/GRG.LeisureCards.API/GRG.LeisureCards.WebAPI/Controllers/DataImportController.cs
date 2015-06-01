using System;
using System.IO;
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

        [HttpGet]
        [Route("DataImport/RetrieveRedLetter/")]
        public Model.DataImportJournalEntry RetrieveRedLetter()
        {
            return AcquireRedLetter(()=> _fileImportManager.GetRedLetterData());
        }

        [HttpPost]
        [Route("DataImport/UploadRedLetter/")]
        public Model.DataImportJournalEntry UploadRedLetter()
        {
            return AcquireRedLetter(()=> HttpContext.Current.Request.Files[0].InputStream);
        }

        public Model.DataImportJournalEntry AcquireRedLetter(Func<Stream> getStream)
        {
            var journalEntry = _fileImportManager.StoreDataFile(DataImportKey.RedLetter, getStream);

            _dataImportJournalEntryRepository.SaveOrUpdate(journalEntry);

            return Mapper.Map<Model.DataImportJournalEntry>(journalEntry);
        }

        [HttpGet]
        [Route("DataImport/ProcessRedLetter/")]
        public Model.DataImportJournalEntry ProcessRedLetterData()
        {
            var latest = _dataImportJournalEntryRepository.GetLast(DataImportKey.RedLetter);

            _dataImportService.Import(latest, path=>HttpContext.Current.Server.MapPath(path));

            return Mapper.Map<Model.DataImportJournalEntry>(latest);
        }
        
        [HttpGet]
        [Route("DataImport/GetRedLetterImportJournal")]
        public Model.DataImportJournalEntry GetRedLetterImportJournal()
        {
            return GetLastImportJournal(DataImportKey.RedLetter);
        }

        [HttpPost]
        [Route("DataImport/Upload241/")]
        public Model.DataImportJournalEntry Upload241()
        {
            return Acquire241(() => HttpContext.Current.Request.Files[0].InputStream);
        }

        public Model.DataImportJournalEntry Acquire241(Func<Stream> getStream)
        {
            var journalEntry = _fileImportManager.StoreDataFile(DataImportKey.TwoForOne, getStream);

            _dataImportJournalEntryRepository.SaveOrUpdate(journalEntry);

            return Mapper.Map<Model.DataImportJournalEntry>(journalEntry);
        }

        [HttpGet]
        [Route("DataImport/Process241/")]
        public Model.DataImportJournalEntry Process241Data()
        {
            var latest = _dataImportJournalEntryRepository.GetLast(DataImportKey.TwoForOne);

            _dataImportService.Import(latest, path => HttpContext.Current.Server.MapPath(path));

            return Mapper.Map<Model.DataImportJournalEntry>(latest);
        }

        [HttpGet]
        [Route("DataImport/Get241ImportJournal")]
        public Model.DataImportJournalEntry Get241ImportJournal()
        {
            return GetLastImportJournal(DataImportKey.TwoForOne);
        }

        private Model.DataImportJournalEntry GetLastImportJournal(DataImportKey importKey)
        {
            return Mapper.Map<Model.DataImportJournalEntry>(_dataImportJournalEntryRepository.GetLast(importKey));
        }
    }
}