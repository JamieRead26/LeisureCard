using System;
using System.Reflection;

namespace GRG.LeisureCards.Data
{
    public static class DataBootstrap
    {
        public static object Lock = new object();
        public static bool Done;
        public static void PrepDb(Assembly classMapAssembly, DbConnectionDetails connectionDetails = null, bool runTestFixtures = true)
        {
            if (Done) return;

            lock (Lock)
            {
                try
                {
                    if (Done) return;

                    var database = new Bootstrap4NHibernate.Database(
                        Database.GetPersistenceConfigurer(connectionDetails),
                        classMapAssembly,
                        configuration => { },
                        true);

                    if (runTestFixtures)
                        database.Populate(Assembly.GetExecutingAssembly());

                    Done = true;
                }
                catch (Exception ex)
                {
                    
                    throw ex;
                }
               
            }
        }
    }
}
