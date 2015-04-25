using System.Reflection;
using Bootstrap4NHibernate;
using FluentNHibernate.Cfg.Db;

namespace GRG.LeisureCards.Service.IntegrationTests
{
    public static class DataBootstrap
    {
        public static object Lock = new object();
        public static bool Done;
        public static void PrepDb()
        {
            if (Done) return;

            lock (Lock)
            {
                if (Done) return;

                var dbConf = PostgreSQLConfiguration.PostgreSQL82.ConnectionString(c => c
                    .Database("LeisureCard")
                    .Host("localhost")
                    .Port(5432)
                    .Username("postgres")
                    .Password(""));

                var database = new Database(dbConf, Assembly.GetExecutingAssembly(), true);

                database.Populate(Assembly.GetExecutingAssembly());
            }
        }
    }
}
