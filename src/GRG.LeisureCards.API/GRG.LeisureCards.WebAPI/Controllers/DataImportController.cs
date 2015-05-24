using System;
using System.Collections.Generic;
using System.IO;
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

        private readonly string _redLetterFilePath;
        private readonly string _twoForOneFilePath;
        private readonly string _leisureCardFilePath;

        public DataImportController(IDataImportService dataImportService, IDataImportJournalEntryRepository dataImportJournalEntryRepository)
        {
            _dataImportService = dataImportService;
            _dataImportJournalEntryRepository = dataImportJournalEntryRepository;

            _redLetterFilePath = "~\\UploadFiles\\RedLetter";
            _twoForOneFilePath = "~\\UploadFiles\\241";
            _leisureCardFilePath = "~\\UploadFiles\\LeisureCards";
        }

        //[HttpPost]
        //[Route("DataImport/LeisureCards/")]
        //public DataImportJournalEntry ImportLeisureCards()
        //{
        //    return ImportDataFile((bytes, key) => _dataImportService.ImportLeisureCards(bytes, key), _leisureCardFilePath);
        //}

        [HttpPost]
        [Route("DataImport/RedLetter/")]
        public DataImportJournalEntry ImportRedLetterData()
        {
            return ImportDataFile((bytes, key) => _dataImportService.ImportRedLetterOffers(bytes, key), _redLetterFilePath);
        }

        private DataImportJournalEntry ImportDataFile(Func<byte[], string,DataImportJournalEntry> importFunc, string filePath)
        {
            try
            {
                var httpRequest = HttpContext.Current.Request;
                filePath = System.Web.Hosting.HostingEnvironment.MapPath(filePath);
                if (httpRequest.Files.Count > 0)
                {
                    var fileKey = Guid.NewGuid().ToString();
                    httpRequest.Files[0].SaveAs(filePath + "\\" + fileKey + ".csv");
                    var journalEntry = importFunc(ReadFully(httpRequest.Files[0].InputStream), fileKey);

                    if (!journalEntry.Success) return journalEntry;
                    //Best effort clean up, if fails must not disrupt flow
                    try
                    {
                        foreach (var fileName in Directory.GetFileSystemEntries(filePath))
                        {
                            if (fileName.IndexOf(fileKey) < 0 && fileName.IndexOf("placeholder") < 0)
                                File.Delete(fileName);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error("Unable to complete upload file clean up", ex);
                    }

                    return journalEntry;
                }

                throw new Exception("No file present in request");
            }
            catch (Exception ex)
            {
                Log.Error("Exception occurred importing data file: " + filePath, ex);
                throw ex;
            }
            
        }

        [HttpPost]
        [Route("DataImport/TwoForOne/")]
        public DataImportJournalEntry ImportTwoForOneData()
        {
            return ImportDataFile((bytes, key) => _dataImportService.ImportTwoForOneOffers(bytes, key), _twoForOneFilePath);

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

        //[HttpGet]
        //[Route("DataImport/GetLastGoodLeisureCardImportJournal")]
        //public DataImportJournalEntry GetLastGoodLeisureCardImportJournal()
        //{
        //    return GetLastImportJournal(true, DataImportKey.LeisureCards);
        //}

        //[HttpGet]
        //[Route("DataImport/GetLastBadLeisureCardImportJournal")]
        //public DataImportJournalEntry GetLastBadLeisureCardImportJournal()
        //{
        //    return GetLastImportJournal(false, DataImportKey.LeisureCards);
        //}

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

        //[HttpGet]
        //[Route("DataImport/GetLeisureCardImportJournal/{count}/{toId}")]
        //public IEnumerable<DataImportJournalEntry> GetLeisureCardImportJournal(int count, int toId)
        //{
        //    var results = GetImportJournal(DataImportKey.LeisureCards, count, toId);

        //    return results;
        //}

        private IEnumerable<DataImportJournalEntry> GetImportJournal(DataImportKey importKey, int count, int toId)
        {
            return _dataImportJournalEntryRepository.Get(importKey, count, toId);
        }

        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}