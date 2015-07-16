using System;
using System.Reflection;

namespace GRG.LeisureCards.Data
{
    public static class DataBootstrap
    {
        public static void PrepDb(Assembly classMapAssembly, DbConnectionDetails connectionDetails = null, Assembly dataFixtureAssembly = null, bool resetSchema = true, int retries = 3)
        {
            try
            {
                var database = new Bootstrap4NHibernate.Database(
                    Database.GetPersistenceConfigurer(connectionDetails),
                    classMapAssembly,
                    configuration => { },
                    resetSchema);

                if (dataFixtureAssembly != null)
                    database.Populate(dataFixtureAssembly);
            }
            catch (Exception ex)
            {
                if (retries>0)
                    PrepDb(classMapAssembly, connectionDetails, dataFixtureAssembly, resetSchema, --retries);
                else
                    throw ex;
            }
        }
    }
}
