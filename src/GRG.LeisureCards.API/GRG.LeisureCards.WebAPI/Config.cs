using System.Configuration;
using GRG.LeisureCards.Data;

namespace GRG.LeisureCards.WebAPI
{
    public static class Config
    {
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

            if (DbConnectionDetails.DbType.IndexOf("MSSQL")>-1)
            {
                DbConnectionDetails.MsSqlConnectionString = ConfigurationManager.AppSettings["MsSqlConnectionString"];
            }
        }
    }
}