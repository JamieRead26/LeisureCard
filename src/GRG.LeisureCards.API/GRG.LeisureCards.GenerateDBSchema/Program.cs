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
            Console.Write("Proceed with reset? (Y/n): ");
            if (Console.ReadLine()=="Y")
            {
                try
                {
                    Console.Write("Resetting target db...");
                    DataBootstrap.PrepDb(Assembly.GetAssembly(typeof(LeisureCardClassMap)), Config.DbConnectionDetails);
                    Console.WriteLine("done.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("");
                    Console.WriteLine("ERROR: " + ex.Message);
                    Console.WriteLine("-----------");
                    Console.WriteLine(ex.StackTrace);
                }
            }

#if DEBUG
            Console.WriteLine("Press ENTER to exit");
            Console.ReadLine();
#endif
        }
    }
}
