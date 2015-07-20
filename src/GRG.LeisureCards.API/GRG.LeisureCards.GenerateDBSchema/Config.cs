using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GRG.LeisureCards.Data;

namespace GRG.LeisureCards.GenerateDBSchema
{
    public enum Release
    {
        Prod,
        UAT
    }

    public static class Config
    {
        public static readonly DbConnectionDetails DbConnectionDetails;
        public static readonly Release Release;

        static Config()
        {
            Release = (Release) Enum.Parse(typeof (Release), ConfigurationManager.AppSettings["Release"]);

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
