using System.IO;
using GRG.LeisureCards.WebAPI.ClientContract;
using GRG.LeisureCards.WebAPI.Model;
using RestSharp;

namespace GRG.LeisureCards.WebAPI.Client
{
    public class DataImportService : Service, IDataImportService
    {
        public DataImportService(string baseUrl, string sessionToken) : base(baseUrl, sessionToken)
        {}

        public DataImportJournalEntry RetrieveRedLetterData()
        {
            return new RestClient(BaseUrl).Execute<DataImportJournalEntry>(GetRestRequest("DataImport/RetrieveRedLetter/", Method.GET)).Data;
        }

        public DataImportJournalEntry UploadRedLetterData(Stream stream)
        {
            using (var memStream = new MemoryStream())
            {
                stream.CopyTo(memStream);
                var fileBytes = memStream.ToArray();

                var request = GetRestRequest("DataImport/UploadRedLetter/", Method.POST);
                request.AddFile("rldata.csv", fileBytes, "rldata.csv");

                return new RestClient(BaseUrl).Execute<DataImportJournalEntry>(request).Data;
            }
        }

        public DataImportJournalEntry ProcessRedLetterData()
        {
            return new RestClient(BaseUrl).Execute<DataImportJournalEntry>(GetRestRequest("DataImport/ProcessRedLetter/", Method.GET)).Data;
        }

        public DataImportJournalEntry GetRedLetterImportJournal()
        {
            return new RestClient(BaseUrl).Execute<DataImportJournalEntry>(GetRestRequest("DataImport/GetRedLetterImportJournal/", Method.GET)).Data;
        }

        public DataImportJournalEntry Upload241Data(Stream stream)
        {
            using (var memStream = new MemoryStream())
            {
                stream.CopyTo(memStream);
                var fileBytes = memStream.ToArray();

                var request = GetRestRequest("DataImport/Upload241/", Method.POST);
                
                request.AddFile("rldata.csv", fileBytes, "rldata.csv");

                return new RestClient(BaseUrl).Execute<DataImportJournalEntry>(request).Data;
            }
        }

        public DataImportJournalEntry Process241Data()
        {
            return new RestClient(BaseUrl).Execute<DataImportJournalEntry>(GetRestRequest("DataImport/Process241/", Method.GET)).Data;
        }

        public DataImportJournalEntry Get241ImportJournal()
        {
            return new RestClient(BaseUrl).Execute<DataImportJournalEntry>(GetRestRequest("DataImport/Get241ImportJournal/", Method.GET)).Data;
        }

      

        public DataImportJournalEntry UploadNewUrnData(string tenantKey, Stream stream)
        {
            using (var memStream = new MemoryStream())
            {
                stream.CopyTo(memStream);
                var fileBytes = memStream.ToArray();

                var request = GetRestRequest(string.Format("DataImport/UploadNewUrns/{0}", tenantKey), Method.POST);

                request.AddParameter("tenantKey", tenantKey);
                request.AddFile("rldata.csv", fileBytes, "rldata.csv");

                return new RestClient(BaseUrl).Execute<DataImportJournalEntry>(request).Data;
            }
        }

        public DataImportJournalEntry GetNewUrnImportJournal()
        {
            return new RestClient(BaseUrl).Execute<DataImportJournalEntry>(GetRestRequest("DataImport/GetNewUrnsImportJournal/", Method.GET)).Data;
        }

        public DataImportJournalEntry GetDeactivateUrnImportJournal()
        {
            return new RestClient(BaseUrl).Execute<DataImportJournalEntry>(GetRestRequest("DataImport/GetDeactivateUrnsImportJournal/", Method.GET)).Data;
        }

        public DataImportJournalEntry UploadDeactivateUrnData(string tenantKey, Stream stream)
        {
            using (var memStream = new MemoryStream())
            {
                stream.CopyTo(memStream);
                var fileBytes = memStream.ToArray();

                var request = GetRestRequest(string.Format("DataImport/UploadDeactivateUrns/{0}", tenantKey), Method.POST);

                request.AddFile("rldata.csv", fileBytes, "rldata.csv");

                return new RestClient(BaseUrl).Execute<DataImportJournalEntry>(request).Data;
            }
        }

        public DataImportJournalEntry ProcessNewUrnData(int renewalPeriodMonths)
        {
            return new RestClient(BaseUrl).Execute<DataImportJournalEntry>(GetRestRequest("DataImport/ProcessNewUrnsData/" + renewalPeriodMonths, Method.GET)).Data;
        }

        public DataImportJournalEntry ProcessDeactivateUrnData()
        {
            return new RestClient(BaseUrl).Execute<DataImportJournalEntry>(GetRestRequest("DataImport/ProcessDeactivateUrnsData/", Method.GET)).Data;
        }
    }
}