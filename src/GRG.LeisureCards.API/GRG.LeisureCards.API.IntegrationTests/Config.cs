using System.Configuration;
using GRG.LeisureCards.Data;

namespace GRG.LeisureCards.API.IntegrationTests
{
    public static class Config
    {
        public const string BaseAddress = "http://leisurecardsapi";//  "http://LeisureCards";//    "http://52.17.232.144:1623/";//  // "http://52.17.166.61/LeisureCardAPI"; 
        public const string AdminCode = "Admin";
        public const string UserCode = "Registered1";
        public const string TenantKey = "GRG";

        private static string SessionToken = null;
        private static string AdminSessionToken = null;


        public static readonly DbConnectionDetails DbConnectionDetails;

        static Config()
        {
            DbConnectionDetails = new DbConnectionDetails
            {
                DbType = ConfigurationManager.AppSettings["DbType"].ToUpper().Trim(),

            };

            if (DbConnectionDetails.DbType == "POSTGRES")
            {
                DbConnectionDetails.PostGresUserName = ConfigurationManager.AppSettings["PostGresUserName"];
                DbConnectionDetails.PostGresDatabase = ConfigurationManager.AppSettings["PostGresDatabase"];
                DbConnectionDetails.PostGresPassword = ConfigurationManager.AppSettings["PostGresPassword"];
                DbConnectionDetails.PostGresPort = int.Parse(ConfigurationManager.AppSettings["PostGresPort"]);
                DbConnectionDetails.PostGresHost = ConfigurationManager.AppSettings["PostGresHost"];
            }

            if (DbConnectionDetails.DbType.IndexOf("MSSQL") > -1)
            {
                DbConnectionDetails.MsSqlConnectionString = ConfigurationManager.AppSettings["MsSqlConnectionString"];
            }
        }
    }
}
