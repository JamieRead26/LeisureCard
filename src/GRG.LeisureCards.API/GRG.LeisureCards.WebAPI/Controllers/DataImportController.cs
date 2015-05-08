using System;
using System.Web.Http;
using GRG.LeisureCards.Model;
using GRG.LeisureCards.Service;
using GRG.LeisureCards.WebAPI.Filters;

namespace GRG.LeisureCards.WebAPI.Controllers
{
    [AdminSessionAuthFilter]
    public class DataImportController : ApiController
    {
        private readonly IDataImportService _dataImportService;

        public DataImportController(IDataImportService dataImportService)
        {
            _dataImportService = dataImportService;
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
    }
}