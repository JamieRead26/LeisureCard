using System;
using System.IO;
using System.Net;
using System.Threading;
using GRG.LeisureCards.DomainModel;
using GRG.LeisureCards.Persistence;
using GRG.LeisureCards.Persistence.NHibernate;
using log4net;

namespace GRG.LeisureCards.WebAPI
{
    public interface IFileImportManager
    {
        Stream GetRedLetterData();

        DataImportJournalEntry StoreDataFile(DataImportKey dataImportKey, Func<Stream> getStream, string tenantKey = null);
        Stream GetAddUrnsData(string tenantKey);
        Stream GetDeactivateUrnsData(string tenantKey);
    }

    public class FileImportConfig
    {
        public string RedLetterFtpPath { get; set; }
        public string RedLetterFtpUid { get; set; }
        public string RedLetterFtpPassword { get; set; }

        public FileImportConfig(string redLetterFtpPath, string redLetterFtpUid, string redLetterFtpPassword)
        {
            RedLetterFtpPath = redLetterFtpPath;
            RedLetterFtpUid = redLetterFtpUid;
            RedLetterFtpPassword = redLetterFtpPassword;
        }
    }

    public class FileImportManager : IFileImportManager
    {
        private static readonly object DirCreateLock = new object(); 

        private readonly string _redLetterFtpPath;
        private readonly string _redLetterUid;
        private readonly string _redLetterPwd;
        private readonly ITenantRepository _tenantRepository;
        private static readonly ILog Log = LogManager.GetLogger(typeof(FileImportManager));

        public FileImportManager(FileImportConfig config, ITenantRepository tenantRepository)
        {
            _redLetterFtpPath = config.RedLetterFtpPath;
            _redLetterUid = config.RedLetterFtpUid;
            _redLetterPwd = config.RedLetterFtpPassword;
            _tenantRepository = tenantRepository;
        }

        [UnitOfWork]
        public DataImportJournalEntry StoreDataFile(DataImportKey dataImportKey, Func<Stream> getStream, string tenantKey = null)
        {
            Tenant tenant = null;
            if (tenantKey != null)
                tenant = _tenantRepository.Get(tenantKey);

            string uploadFolder = string.Empty;
            try
            {
                uploadFolder = System.Web.Hosting.HostingEnvironment.MapPath(dataImportKey.UploadPath);

                if (tenant != null)
                {
                    uploadFolder = uploadFolder + "\\" + tenant.Key;

                    //This mechanism is such that if multiple nodes happen to create the same folder in a race condition one will win, the other will lose but over all both processes
                    //will succeed.
                    if (!Directory.Exists(uploadFolder))
                    {
                        try
                        {
                            lock (DirCreateLock)
                            {
                                if (!Directory.Exists(uploadFolder))
                                    Directory.CreateDirectory(uploadFolder);
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.Error("Unable to create upload folder : " + uploadFolder, ex);  
                            //Ignore this error at this stage an another node may have created the folder
                        }
                    }
                }
                
                var uploadFileName = dataImportKey.Key + "_" + DateTime.Now.ToString("O") + ".csv";
                uploadFileName = uploadFileName.Replace(":", "_").Replace("+", "_").Replace("-", "_");

                using (var stream = getStream())
                using (var fileStream = File.Open(uploadFolder + "\\" + uploadFileName, FileMode.CreateNew))
                {
                    stream.CopyTo(fileStream);
                }
                
                //Best effort clean up, if fails must not disrupt flow
                ThreadPool.QueueUserWorkItem(state =>
                {
                    try
                    {
                        var twoWeeksAgo = DateTime.Now - TimeSpan.FromDays(14);
                        foreach (var fileName in Directory.GetFileSystemEntries(uploadFolder))
                        {
                            if (fileName.IndexOf(uploadFileName) < 0 && fileName.IndexOf("placeholder") < 0 &&
                                new FileInfo(fileName).CreationTime < twoWeeksAgo)
                                File.Delete(fileName);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error("Unable to complete upload file clean up", ex);
                    }
                });

                var now = DateTime.Now;

                return new DataImportJournalEntry
                {
                    Success = true,
                    LastRun = now,
                    UploadKey = dataImportKey.Key,
                    FileName = uploadFileName,
                    Tenant = tenant
                };
            }
            catch (Exception ex)
            {
                Log.Error("Exception occurred importing data file: " + uploadFolder, ex);
                
                return new DataImportJournalEntry
                {
                    Success = false,
                    UploadKey = dataImportKey.Key,
                    LastRun = DateTime.Now,
                    Message = ex.Message,
                    Tenant = tenant
                };
            }
        }

        public Stream GetAddUrnsData(string tenantKey)
        {
            var tenant = _tenantRepository.Get(tenantKey);

            return GetTenantData(tenant, tenant.FtpAddFilePath);
        }

        public Stream GetDeactivateUrnsData(string tenantKey)
        {
            var tenant = _tenantRepository.Get(tenantKey);

            return GetTenantData(tenant, tenant.FtpDeactivateFilePath);
        }

        public Stream GetTenantData(Tenant tenant, string filePath)
        {
            var ftpRequest = (FtpWebRequest)WebRequest.Create(tenant.FtpServer + "/" + filePath);
            ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;

            // This example assumes the FTP site uses anonymous logon.
            ftpRequest.Credentials = new NetworkCredential(tenant.FtpUsername, tenant.FtpPassword);

            return ftpRequest.GetResponse().GetResponseStream();
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