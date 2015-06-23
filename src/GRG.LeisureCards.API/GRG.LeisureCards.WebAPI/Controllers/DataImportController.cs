using System;
using System.IO;
using System.Runtime.InteropServices;
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
        [Route("DataImport/ProcessNewUrnsData/{cardDurationMonths}")]
        public Model.DataImportJournalEntry ProcessNewUrnsData(int cardDurationMonths)
        {
            return Dispatch(() => Process(DataImportKey.NewUrns, cardDurationMonths));
        }
        
        [HttpGet]
        [Route("DataImport/ProcessDeactivateUrnsData/")]
        public Model.DataImportJournalEntry ProcessDeactivateUrnsData()
        {
            return Dispatch(()=>Process(DataImportKey.DeactivatedUrns));
        }

        private DataImportJournalEntry Process(DataImportKey key, params object[] args)
        {
            return Dispatch(()=>Mapper.Map<Model.DataImportJournalEntry>(_dataImportService.Import(key, path => HttpContext.Current.Server.MapPath(path), args)));
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
        [Route("DataImport/GetNewUrnsImportJournal")]
        public Model.DataImportJournalEntry GetNewUrnsImportJournal()
        {
            return Dispatch(()=>GetLastImportJournal(DataImportKey.NewUrns));
        }

        [HttpGet]
        [Route("DataImport/GetDeactivateUrnsImportJournal")]
        public Model.DataImportJournalEntry GetDeactivateUrnsImportJournal()
        {
            return Dispatch(()=>GetLastImportJournal(DataImportKey.DeactivatedUrns));
        }

        public Model.DataImportJournalEntry Acquire(DataImportKey key, Func<Stream> getStream, string tenant = null)
        {
            return Dispatch(()=> Mapper.Map<Model.DataImportJournalEntry>(_dataImportJournalEntryRepository.SaveOrUpdate(_fileImportManager.StoreDataFile(key, getStream, tenant))));
        }

        private Model.DataImportJournalEntry GetLastImportJournal(DataImportKey importKey)
        {
            return Dispatch(()=> Mapper.Map<Model.DataImportJournalEntry>(_dataImportJournalEntryRepository.GetLast(importKey)));
        }
    }
}