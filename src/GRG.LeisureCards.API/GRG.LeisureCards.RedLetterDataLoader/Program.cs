using System;
using System.Configuration;
using System.IO;
using System.Net;
using GRG.LeisureCards.WebAPI.Model;
using log4net;
using log4net.Config;
using RestSharp;

namespace GRG.LeisureCards.RedLetterDataLoader
{
    public class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Program));

        static void Main(string[] args)
        {
            try
            {
                XmlConfigurator.Configure();

                var config = new Config();
           
                if (args.Length == 0)
                {
                    config.FtpPath = ConfigurationManager.AppSettings["FtpPath"];
                    config.Uid = ConfigurationManager.AppSettings["Uid"];
                    config.Password = ConfigurationManager.AppSettings["Password"];
                    config.ApiBaseAddress = ConfigurationManager.AppSettings["ApiBaseAddress"];
                    config.ApiAdminCode = ConfigurationManager.AppSettings["ApiAdminCode"];
                }

                if (!config.IsValid && args.Length==3)
                {
                    //load from args
                }

                if (!config.IsValid)
                {
                    //show usage
                }

                var ftpRequest = (FtpWebRequest)WebRequest.Create(config.FtpPath);
                ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;

                // This example assumes the FTP site uses anonymous logon.
                ftpRequest.Credentials = new NetworkCredential(config.Uid, config.Password);

                using (var ftpResponse = (FtpWebResponse)ftpRequest.GetResponse())
                using (var dataStream = ftpResponse.GetResponseStream())
                using (var memStream = new MemoryStream())
                {
                    dataStream.CopyTo(memStream);
                    var fileBytes = memStream.ToArray();
                    var sessionToken = GetAdminSessionToken(config.ApiBaseAddress, config.ApiAdminCode);

                    var client = new RestClient(config.ApiBaseAddress);

                    var request = new RestRequest("DataImport/RedLetter/", Method.POST);
                    request.AddFile("RedLetterData.csv", fileBytes, "RedLetterData.csv");
                    request.AddHeader("accepts", "application/json");
                    request.AddHeader("SessionToken", sessionToken);

                    var response = client.Execute<DataImportJournalEntry>(request);

                    if (response.Data.Success)
                    {
                        Console.WriteLine("File uploaded successfully");
                        Log.Info("File uploaded successfully");
                    }
                    else
                    {
                    
                        Log.Error("File uploaded failed - " + response.Data.Message);
                        Log.Error(response.Data.StackTrace);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("File uploaded failed @ " + ex.Message);
                Log.Error(ex);
                throw;
            }
        }

        private static string GetAdminSessionToken(string apiBaseAddress, string code)
        {
            var client = new RestClient(apiBaseAddress);

            var request = new RestRequest("LeisureCard/Login/{code}", Method.GET);
            request.AddParameter("code", code);
            request.AddHeader("accepts", "application/json");

            var response = client.Execute<LeisureCardRegistrationResponse>(request);

            return response.Data.SessionInfo.SessionToken;
        }
    }
}