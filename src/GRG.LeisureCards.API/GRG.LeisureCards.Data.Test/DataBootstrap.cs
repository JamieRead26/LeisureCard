using System.Reflection;

namespace GRG.LeisureCards.Data
{
    public static class DataBootstrap
    {
        public static void PrepDb(Assembly classMapAssembly, DbConnectionDetails connectionDetails = null, Assembly dataFixtureAssembly = null)
        {
            var database = new Bootstrap4NHibernate.Database(
                Database.GetPersistenceConfigurer(connectionDetails),
                classMapAssembly,
                configuration => { },
                true);

            if (dataFixtureAssembly!=null)
                database.Populate(dataFixtureAssembly);
        }
    }
}
