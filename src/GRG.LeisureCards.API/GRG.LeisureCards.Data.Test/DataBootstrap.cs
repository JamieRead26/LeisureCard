using System.Reflection;

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

                var database = new Bootstrap4NHibernate.Database(Database.GetPersistenceConfigurer(), classMapAssembly, true) ;

                database.Populate(Assembly.GetExecutingAssembly());
            }
        }
    }
}
