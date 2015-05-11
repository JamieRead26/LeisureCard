using System;
using System.Collections.Generic;
using System.Web.Http;
using GRG.LeisureCards.Model;
using GRG.LeisureCards.Persistence;
using GRG.LeisureCards.Service;
using GRG.LeisureCards.WebAPI.Filters;

namespace GRG.LeisureCards.WebAPI.Controllers
{
    [AdminSessionAuthFilter]
    public class DataImportController : ApiController
    {
        private readonly IDataImportService _dataImportService;
        private readonly IDataImportJournalEntryRepository _dataImportJournalEntryRepository;

        public DataImportController(IDataImportService dataImportService, IDataImportJournalEntryRepository dataImportJournalEntryRepository)
        {
            _dataImportService = dataImportService;
            _dataImportJournalEntryRepository = dataImportJournalEntryRepository;
        }

        [HttpPost]
        [Route("DataImport/RedLetter/")]
        public DataImportJournalEntry ImportRedLetterData([FromBody] string base64)
        {
            return _dataImportService.ImportRedLetterOffers(Convert.FromBase64String(base64));
        }

        [HttpPost]
        [Route("DataImport/TwoForOne/")]
        public DataImportJournalEntry ImportTwoForOneData([FromBody] string base64)
        {
            return _dataImportService.ImportTwoForOneOffers(Convert.FromBase64String(base64));
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