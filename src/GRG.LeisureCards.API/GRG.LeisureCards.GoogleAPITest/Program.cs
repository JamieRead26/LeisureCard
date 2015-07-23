using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRG.LeisureCards.GoogleAPITest
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var googleService = new GoogleLocationService(
                    ConfigurationManager.AppSettings["GoogleApiUrlTemplate"],
                    ConfigurationManager.AppSettings["GoogleCryptoKey"]);

                Console.WriteLine("Looking up Lat/Long for London, UK");

                var mapPoint = googleService.GetLatLongFromAddress("London");

                Console.WriteLine("London is {0}/{1}", mapPoint.Latitude, mapPoint.Longitude);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Occurred: " + ex.Message);
               
               // Console.WriteLine(ex.StackTrace);
            }

            Console.WriteLine();
            Console.WriteLine("Press ENTER to exit");
            Console.ReadLine();
        }
    }
}
