using System;
using System.Reflection;
using GRG.LeisureCards.Data;
using GRG.LeisureCards.Persistence.NHibernate.ClassMaps;

namespace GRG.LeisureCards.GenerateDBSchema
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Do you want to completely reset the schema on the following target DB:");
            Console.WriteLine();
            Console.WriteLine("{0}", Config.DbConnectionDetails);
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("WARNING: THIS WILL DELETE ALL DATA AND RESET TARGET DATABASE SCHEMA");
            Console.ForegroundColor = ConsoleColor.White;
            if (args.Length == 0 || args[0]!="NOCONF")
            {
                Console.Write("Proceed with reset? (y/n): ");
                if (Console.ReadLine().ToUpper() != "Y")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("DB RESET CANCELLED");
                    Console.ForegroundColor = ConsoleColor.White;
                    return;
                }
            }

            try
            {
                Console.Write("Resetting target db...");
                DataBootstrap.PrepDb(Assembly.GetAssembly(typeof(LeisureCardClassMap)), Config.DbConnectionDetails, Assembly.GetAssembly(typeof(TenantDataFixture)));
                Console.WriteLine("done.");
            }
            catch (Exception ex)
            {
                do
                {
                    Console.WriteLine("");
                    Console.WriteLine("ERROR: " + ex.Message);
                    Console.WriteLine("-----------");
                    Console.WriteLine(ex.StackTrace);
                } while ((ex=ex.InnerException)!=null);
            }

            if (args.Length == 0)
            {
#if DEBUG
                Console.WriteLine("Press ENTER to exit");
                Console.ReadLine();
#endif
            }
        }
    }
}
