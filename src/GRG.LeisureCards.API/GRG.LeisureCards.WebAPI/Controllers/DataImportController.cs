using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using GRG.LeisureCards.Model;
using GRG.LeisureCards.Persistence;
using GRG.LeisureCards.Service;
using GRG.LeisureCards.WebAPI.Filters;

namespace GRG.LeisureCards.WebAPI.Controllers
{
    [SessionAuthFilter(true)]
    public class DataImportController : ApiController
    {
        private readonly IDataImportService _dataImportService;
        private readonly IDataImportJournalEntryRepository _dataImportJournalEntryRepository;

        private readonly string _redLetterFilePath;
        private readonly string _twoForOneFilePath;

        public DataImportController(IDataImportService dataImportService, IDataImportJournalEntryRepository dataImportJournalEntryRepository)
        {
            _dataImportService = dataImportService;
            _dataImportJournalEntryRepository = dataImportJournalEntryRepository;

            _redLetterFilePath = "~\\UploadFiles\\RedLetter";
            _twoForOneFilePath = "~\\UploadFiles\\TwoForOne";
        }

        [HttpPost]
        [Route("DataImport/RedLetter/")]
        public DataImportJournalEntry ImportRedLetterData()
        {
            return ImportDateFile((bytes, key) => _dataImportService.ImportRedLetterOffers(bytes, key), _redLetterFilePath);
        }

        private DataImportJournalEntry ImportDateFile(Func<byte[], string,DataImportJournalEntry> importFunc, string filePath)
        {
            var httpRequest = HttpContext.Current.Request;
            filePath = System.Web.Hosting.HostingEnvironment.MapPath(filePath);
            if (httpRequest.Files.Count > 0)
            {
                var fileKey = Guid.NewGuid().ToString();
                httpRequest.Files[0].SaveAs(filePath + "\\" + fileKey);
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
                catch (Exception)
                {
                    //TODO : LOG4NET
                }

                return journalEntry;
            }

            throw new Exception("No file present in request");
        }

        [HttpPost]
        [Route("DataImport/TwoForOne/")]
        public DataImportJournalEntry ImportTwoForOneData()
        {
            return ImportDateFile((bytes, key) => _dataImportService.ImportTwoForOneOffers(bytes, key), _twoForOneFilePath);

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