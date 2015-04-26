using System.Reflection;
using Bootstrap4NHibernate;
using FluentNHibernate.Cfg.Db;

namespace GRG.LeisureCards.Data
{
    public static class DataBootstrap
    {
        public static object Lock = new object();
        public static bool Done;
        public static void PrepDb(Assembly classMapAssembly)
        {
            if (Done) return;

            lock (Lock)
            {
                if (Done) return;

                var dbConf = PostgreSQLConfiguration.PostgreSQL82.ConnectionString(c => c
                    .Database("LeisureCards")
                    .Host("localhost")
                    .Port(5432)
                    .Username("postgres")
                    .Password(""));

                var database = new Database(dbConf, classMapAssembly, true);

                database.Populate(Assembly.GetExecutingAssembly());
            }
        }
    }
}
