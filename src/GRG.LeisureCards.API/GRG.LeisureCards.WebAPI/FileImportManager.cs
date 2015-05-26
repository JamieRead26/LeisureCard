using System;
using System.IO;
using System.Net;
using GRG.LeisureCards.DomainModel;
using log4net;

namespace GRG.LeisureCards.WebAPI
{
    public interface IFileImportManager
    {
        string RedLetterFilePath { get; }

        string TwoForOneFilePath { get; }

        Stream GetRedLetterData();

        DataImportJournalEntry ImportDataFile(Func<byte[], string, DataImportJournalEntry> importFunc, string filePath, Stream file);
    }

    public class FileImportManager : IFileImportManager
    {
        private readonly string _redLetterFtpPath;
        private readonly string _redLetterUid;
        private readonly string _redLetterPwd;
        private static readonly ILog Log = LogManager.GetLogger(typeof(FileImportManager));

        public string RedLetterFilePath { get; private set; }
        public string TwoForOneFilePath { get; private set; }

        public FileImportManager(string redLetterFtpPath, string redLetterUid, string redLetterPwd)
        {
            _redLetterFtpPath = redLetterFtpPath;
            _redLetterUid = redLetterUid;
            _redLetterPwd = redLetterPwd;
            RedLetterFilePath = "~\\UploadFiles\\RedLetter";
            TwoForOneFilePath = "~\\UploadFiles\\241";
        }

        public DataImportJournalEntry ImportDataFile(Func<byte[], string, DataImportJournalEntry> importFunc, string uploadFolder, Stream file)
        {
            try
            {
                
                uploadFolder = System.Web.Hosting.HostingEnvironment.MapPath(uploadFolder);
                var fileKey = Guid.NewGuid().ToString();

                var memStream = new MemoryStream();
                file.CopyTo(memStream);

                using (var fileStream = File.Open(uploadFolder + "\\" + fileKey + ".csv", FileMode.CreateNew))
                {
                    memStream.CopyTo(fileStream);
                }
               
                var journalEntry = importFunc(ReadFully(memStream), fileKey);

                if (!journalEntry.Success) return journalEntry;
                //Best effort clean up, if fails must not disrupt flow
                try
                {
                    var twoWeeksAgo = DateTime.Now - TimeSpan.FromDays(14);
                    foreach (var fileName in Directory.GetFileSystemEntries(uploadFolder))
                    {
                        if (fileName.IndexOf(fileKey) < 0 && fileName.IndexOf("placeholder") < 0 &&
                            new FileInfo(fileName).CreationTime < twoWeeksAgo)
                            File.Delete(fileName);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("Unable to complete upload file clean up", ex);
                }

                return journalEntry;
            }
            catch (Exception ex)
            {
                Log.Error("Exception occurred importing data file: " + uploadFolder, ex);
                throw ex;
            }
        }

        public static byte[] ReadFully(Stream input)
        {
            input.Position = 0;
            var buffer = new byte[16 * 1024];
            using (var ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                    ms.Write(buffer, 0, read);

                return ms.ToArray();
            }
        }

        public Stream GetRedLetterData()
        {
            var ftpRequest = (FtpWebRequest)WebRequest.Create(_redLetterFtpPath);
            ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;

            // This example assumes the FTP site uses anonymous logon.
            ftpRequest.Credentials = new NetworkCredential(_redLetterUid, _redLetterPwd);

            return ftpRequest.GetResponse().GetResponseStream();
        }
    }
}