using System;
using System.IO;
using System.Web;
using System.Web.Http;
using AutoMapper;
using GRG.LeisureCards.DomainModel;
using GRG.LeisureCards.Persistence;
using GRG.LeisureCards.Service;
using GRG.LeisureCards.WebAPI.Filters;
using DataImportJournalEntry = GRG.LeisureCards.WebAPI.Model.DataImportJournalEntry;

namespace GRG.LeisureCards.WebAPI.Controllers
{
    [SessionAuthFilter(true)]
    public class DataImportController : LcApiController
    {
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
            return Dispatch(()=>Acquire(DataImportKey.RedLetter, ()=> _fileImportManager.GetRedLetterData()));
        }

        [HttpGet]
        [Route("DataImport/RetrieveNewUrns/{tenantKey}")]
        public Model.DataImportJournalEntry RetrieveNewUrns(string tenantKey)
        {
            return Dispatch(() => Acquire(DataImportKey.NewUrns, () => _fileImportManager.GetAddUrnsData(tenantKey), tenantKey));
        }

        [HttpGet]
        [Route("DataImport/RetrieveDeactivateUrns/{tenantKey}")]
        public Model.DataImportJournalEntry RetrieveDeactivateUrns(string tenantKey)
        {
            return Dispatch(() => Acquire(DataImportKey.DeactivatedUrns, () => _fileImportManager.GetDeactivateUrnsData(tenantKey), tenantKey));
        }

        [HttpPost]
        [Route("DataImport/UploadRedLetter/")]
        public Model.DataImportJournalEntry UploadRedLetter()
        {
            return Dispatch(()=>Acquire(DataImportKey.RedLetter, () => HttpContext.Current.Request.Files[0].InputStream));
        }

        [HttpPost]
        [Route("DataImport/Upload241/")]
        public Model.DataImportJournalEntry Upload241()
        {
            return Dispatch(()=>Acquire(DataImportKey.TwoForOne, () => HttpContext.Current.Request.Files[0].InputStream));
        }

        [HttpPost]
        [Route("DataImport/UploadNewUrns/{tenantKey}")]
        public Model.DataImportJournalEntry UploadNewUrns(string tenantKey)
        {
            return Dispatch(()=>Acquire(DataImportKey.NewUrns, () => HttpContext.Current.Request.Files[0].InputStream, tenantKey));
        }

        [HttpPost]
        [Route("DataImport/UploadDeactivateUrns/{tenantKey}")]
        public Model.DataImportJournalEntry UploadDeactivateUrns(string tenantKey)
        {
            return Dispatch(()=>Acquire(DataImportKey.DeactivatedUrns, () => HttpContext.Current.Request.Files[0].InputStream, tenantKey));
        }

        [HttpGet]
        [Route("DataImport/ProcessRedLetter/")]
        public Model.DataImportJournalEntry ProcessRedLetterData()
        {
            return Dispatch(()=>Process(DataImportKey.RedLetter));
        }
        
        [HttpGet]
        [Route("DataImport/Process241/")]
        public Model.DataImportJournalEntry Process241Data()
        {
            return Dispatch(()=>Mapper.Map<Model.DataImportJournalEntry>(_dataImportService.Import(DataImportKey.TwoForOne, path => HttpContext.Current.Server.MapPath(path))));
        }
        
        [HttpGet]
        [Route("DataImport/ProcessNewUrnsData/{cardDurationMonths}/{reference}/{tenantKey}")]
        public Model.DataImportJournalEntry ProcessNewUrnsData(int cardDurationMonths, string reference, string tenantKey)
        {
            return Dispatch(() => ProcessForTenant(DataImportKey.NewUrns, tenantKey, cardDurationMonths, reference));
        }
        
        [HttpGet]
        [Route("DataImport/ProcessDeactivateUrnsData/{tenantKey}")]
        public Model.DataImportJournalEntry ProcessDeactivateUrnsData(string tenantKey)
        {
            return Dispatch(() => ProcessForTenant(DataImportKey.DeactivatedUrns, tenantKey));
        }

        private DataImportJournalEntry Process(DataImportKey key, params object[] args)
        {
            return Dispatch(()=>Mapper.Map<Model.DataImportJournalEntry>(_dataImportService.Import(key, path => HttpContext.Current.Server.MapPath(path), args)));
        }

        private DataImportJournalEntry ProcessForTenant(DataImportKey key, string tenantKey, params object[] args)
        {
            return Dispatch(() => Mapper.Map<Model.DataImportJournalEntry>(_dataImportService.ImportForTenant(key, path => HttpContext.Current.Server.MapPath(path), tenantKey, args)));
        }

        [HttpGet]
        [Route("DataImport/GetRedLetterImportJournal")]
        public Model.DataImportJournalEntry GetRedLetterImportJournal()
        {
            return Dispatch(()=>GetLastImportJournal(DataImportKey.RedLetter));
        }

        [HttpGet]
        [Route("DataImport/Get241ImportJournal")]
        public Model.DataImportJournalEntry Get241ImportJournal()
        {
            return Dispatch(()=>GetLastImportJournal(DataImportKey.TwoForOne));
        }

        [HttpGet]
        [Route("DataImport/GetNewUrnsImportJournal/{tenantKey}")]
        public Model.DataImportJournalEntry GetNewUrnsImportJournal(string tenantKey)
        {
            return Dispatch(()=>GetLastImportJournal(DataImportKey.NewUrns, tenantKey));
        }

        [HttpGet]
        [Route("DataImport/GetDeactivateUrnsImportJournal/{tenantKey}")]
        public Model.DataImportJournalEntry GetDeactivateUrnsImportJournal(string tenantKey)
        {
            return Dispatch(()=>GetLastImportJournal(DataImportKey.DeactivatedUrns, tenantKey));
        }

        public Model.DataImportJournalEntry Acquire(DataImportKey key, Func<Stream> getStream, string tenant = null)
        {
            return Mapper.Map<Model.DataImportJournalEntry>(_dataImportJournalEntryRepository.SaveOrUpdate(_fileImportManager.StoreDataFile(key, getStream, tenant)));
        }

        private Model.DataImportJournalEntry GetLastImportJournal(DataImportKey importKey)
        {
            return Mapper.Map<Model.DataImportJournalEntry>(_dataImportJournalEntryRepository.GetLast(importKey));
        }

        private Model.DataImportJournalEntry GetLastImportJournal(DataImportKey importKey, string tenantKey)
        {
            return Mapper.Map<Model.DataImportJournalEntry>(_dataImportJournalEntryRepository.GetLast(importKey, tenantKey));
        }
    }
}