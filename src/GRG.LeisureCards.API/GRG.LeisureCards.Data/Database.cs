using System;
using FluentNHibernate.Cfg.Db;

namespace GRG.LeisureCards.Data
{
    public static class Database
    {
        public static IPersistenceConfigurer GetPersistenceConfigurer()
        {
            return PostgreSQLConfiguration.PostgreSQL82.ConnectionString(c => c
                    .Database("LeisureCards")
                    .Host("localhost")
                    .Port(5432)
                    .Username("postgres")
                    .Password("tripod26"));

            //return MsSqlConfiguration.MsSql2008
            //    .ConnectionString(c => c
            //    .FromConnectionStringWithKey("connectionStringKey"));
        }
    }
}
