using System.Reflection;

namespace GRG.LeisureCards.Data
{
    public static class DataBootstrap
    {
        public static void PrepDb(Assembly classMapAssembly, DbConnectionDetails connectionDetails = null, Assembly dataFixtureAssembly = null, bool resetSchema = true)
        {
            var database = new Bootstrap4NHibernate.Database(
                Database.GetPersistenceConfigurer(connectionDetails),
                classMapAssembly,
                configuration => { },
                resetSchema);

            if (dataFixtureAssembly!=null)
                database.Populate(dataFixtureAssembly);
        }
    }
}
