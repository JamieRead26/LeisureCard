using System;
using System.Security.AccessControl;
using FluentNHibernate.Cfg.Db;

namespace GRG.LeisureCards.Data
{
    public static class Database
    {
        private static readonly ICustomSqlTypeStrings CustomSqlTypeStrings = new PostGresCustomSqlTypeStrings();

        public static IPersistenceConfigurer GetPersistenceConfigurer(DbConnectionDetails connectionDetails = null)
        {
            if (connectionDetails==null)
            {
                return PostgreSQLConfiguration.PostgreSQL82.ConnectionString(c => c
                        .Database("LeisureCards")
                        .Host("localhost")
                        .Port(5432)
                        .Username("postgres")
                        .Password("tripod26"));
            }
            
            if (connectionDetails.DbType.ToUpper().Trim() == "POSTGRES")
                return PostgreSQLConfiguration.PostgreSQL82.ConnectionString(c => c
                        .Database(connectionDetails.PostGresDatabase)
                        .Host(connectionDetails.PostGresHost)
                        .Port(connectionDetails.PostGresPort)
                        .Username(connectionDetails.PostGresUserName)
                        .Password(connectionDetails.PostGresPassword));

            if (connectionDetails.DbType.ToUpper().Trim() == "MSSQL2008")
                return MsSqlConfiguration.MsSql2008
                    .ConnectionString(c => c
                    .FromConnectionStringWithKey(connectionDetails.MsSqlConnectionString));

            if (connectionDetails.DbType.ToUpper().Trim() == "MSSQL2012")
                return MsSqlConfiguration.MsSql2012.ConnectionString(connectionDetails.MsSqlConnectionString);

            throw new Exception("Invalid DB connection details");
        }

        public static string GetCustomSqlTypeString(CustomSqlType customSqlType)
        {
            return CustomSqlTypeStrings.Get(customSqlType);
        }
    }

    public class DbConnectionDetails
    {
        public string DbType { get; set; }
        public string PostGresDatabase { get; set; }
        public string PostGresHost { get; set; }
        public int PostGresPort { get; set; }
        public string PostGresUserName { get; set; }
        public string PostGresPassword { get; set; }

        public string MsSqlConnectionString { get; set; }
    }
}
