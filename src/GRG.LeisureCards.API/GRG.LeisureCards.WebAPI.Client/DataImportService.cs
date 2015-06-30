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

        public DataImportJournalEntry RetrieveNewUrnsData(string tenantKey)
        {
            return new RestClient(BaseUrl).Execute<DataImportJournalEntry>(GetRestRequest("DataImport/RetrieveNewUrns/" + tenantKey, Method.GET)).Data;
        }

        public DataImportJournalEntry RetrieveDeactivateUrns(string tenantKey)
        {
            return new RestClient(BaseUrl).Execute<DataImportJournalEntry>(GetRestRequest("DataImport/RetrieveDeactivateUrns/" + tenantKey, Method.GET)).Data;
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

        public DataImportJournalEntry GetNewUrnImportJournal(string tenantKey)
        {
            return new RestClient(BaseUrl).Execute<DataImportJournalEntry>(GetRestRequest("DataImport/GetNewUrnsImportJournal/" + tenantKey, Method.GET)).Data;
        }

        public DataImportJournalEntry GetDeactivateUrnImportJournal(string tenantKey)
        {
            return new RestClient(BaseUrl).Execute<DataImportJournalEntry>(GetRestRequest("DataImport/GetDeactivateUrnsImportJournal/" + tenantKey, Method.GET)).Data;
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

        public DataImportJournalEntry ProcessNewUrnData(string tenantKey, int renewalPeriodMonths, string reference)
        {
            return new RestClient(BaseUrl).Execute<DataImportJournalEntry>(GetRestRequest(string.Format("DataImport/ProcessNewUrnsData/{0}/{1}/{2}",renewalPeriodMonths,reference, tenantKey), Method.GET)).Data;
        }

        public DataImportJournalEntry ProcessDeactivateUrnData(string tenantKey)
        {
            return new RestClient(BaseUrl).Execute<DataImportJournalEntry>(GetRestRequest("DataImport/ProcessDeactivateUrnsData/" + tenantKey, Method.GET)).Data;
        }
    }
}