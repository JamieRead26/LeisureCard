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
            var client = new RestClient(BaseUrl);

            var request = new RestRequest("DataImport/RetrieveRedLetter/", Method.GET);
            request.AddHeader("accepts", "application/json");
            request.AddHeader("SessionToken", SessionToken);

            return client.Execute<DataImportJournalEntry>(request).Data;
        }

        public DataImportJournalEntry UploadRedLetterData(Stream stream)
        {
            using (var memStream = new MemoryStream())
            {
                stream.CopyTo(memStream);
                var fileBytes = memStream.ToArray();

                var client = new RestClient(BaseUrl);

                var request = new RestRequest("DataImport/UploadRedLetter/", Method.POST);
                request.AddFile("rldata.csv", fileBytes, "rldata.csv");
                request.AddHeader("accepts", "application/json");
                request.AddHeader("SessionToken", SessionToken);

                return client.Execute<DataImportJournalEntry>(request).Data;
            }
        }

        public DataImportJournalEntry ProcessRedLetterData()
        {
            var client = new RestClient(BaseUrl);

            var request = new RestRequest("DataImport/ProcessRedLetter/", Method.GET);
            request.AddHeader("accepts", "application/json");
            request.AddHeader("SessionToken", SessionToken);

            return client.Execute<DataImportJournalEntry>(request).Data;
        }

        public DataImportJournalEntry GetRedLetterImportJournal()
        {
            var client = new RestClient(BaseUrl);

            var request = new RestRequest("DataImport/GetRedLetterImportJournal/", Method.GET);
            request.AddHeader("accepts", "application/json");
            request.AddHeader("SessionToken", SessionToken);

            return client.Execute<DataImportJournalEntry>(request).Data;
        }

        public DataImportJournalEntry Upload241Data(Stream stream)
        {
            using (var memStream = new MemoryStream())
            {
                stream.CopyTo(memStream);
                var fileBytes = memStream.ToArray();

                var client = new RestClient(BaseUrl);

                var request = new RestRequest("DataImport/Upload241/", Method.POST);
                request.AddFile("rldata.csv", fileBytes, "rldata.csv");
                request.AddHeader("accepts", "application/json");
                request.AddHeader("SessionToken", SessionToken);

                return client.Execute<DataImportJournalEntry>(request).Data;
            }
        }

        public DataImportJournalEntry Process241Data()
        {
            var client = new RestClient(BaseUrl);

            var request = new RestRequest("DataImport/Process241/", Method.GET);
            request.AddHeader("accepts", "application/json");
            request.AddHeader("SessionToken", SessionToken);

            return client.Execute<DataImportJournalEntry>(request).Data;
        }

        public DataImportJournalEntry Get241ImportJournal()
        {
            var client = new RestClient(BaseUrl);

            var request = new RestRequest("DataImport/Get241ImportJournal/", Method.GET);
            request.AddHeader("accepts", "application/json");
            request.AddHeader("SessionToken", SessionToken);

            return client.Execute<DataImportJournalEntry>(request).Data;
        }
    }
}
