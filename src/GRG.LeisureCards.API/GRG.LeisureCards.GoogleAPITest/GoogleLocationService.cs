using System;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;

namespace GRG.LeisureCards.GoogleAPITest
{
    public class GoogleLocationService 
    {
        private readonly string _googleApiUrlTemplate;
        private readonly string _cryptoKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="GoogleLocationService"/> class.
        /// </summary>
        /// <param name="useHttps">Indicates whether to call the Google API over HTTPS or not.</param>
        /// <param name="cryptoKey"></param>
        public GoogleLocationService(string googleApiUrlTemplate, string cryptoKey)
        {
            _googleApiUrlTemplate = googleApiUrlTemplate;
            _cryptoKey = cryptoKey;
        }


        /// <summary>
        /// Gets a value indicating whether to use the Google API over HTTPS.
        /// </summary>
        /// <value>
        ///   <c>true</c> if using the API over HTTPS; otherwise, <c>false</c>.
        /// </value>
        public bool UseHttps { get; private set; }



        /// <summary>
        /// Gets the latitude and longitude that belongs to an address.
        /// </summary>
        /// <param name="commaSeparatedAddress">The address.</param>
        /// <returns></returns>
        /// <exception cref="System.Net.WebException"></exception>
        public MapPoint GetLatLongFromAddress(string commaSeparatedAddress)
        {
            var url = string.Format(_googleApiUrlTemplate, Uri.EscapeDataString(commaSeparatedAddress));

            url = Sign(url, _cryptoKey);

            XDocument doc = XDocument.Load(url);

            string status = doc.Descendants("status").FirstOrDefault().Value;
            if (status == "OVER_QUERY_LIMIT" || status == "REQUEST_DENIED")
            {
                throw new System.Net.WebException("Request Not Authorized or Over QueryLimit");
            }

            var els = doc.Descendants("result").Descendants("geometry").Descendants("location").FirstOrDefault();
            if (null != els)
            {
                var latitude = ParseUK((els.Nodes().First() as XElement).Value);
                var longitude = ParseUK((els.Nodes().ElementAt(1) as XElement).Value);
                return new MapPoint() { Latitude = latitude, Longitude = longitude };
            }
            return null;
        }

        double ParseUK(string value)
        {
            return Double.Parse(value, new CultureInfo("en-gb"));
        }

        public static string Sign(string url, string keyString)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();

            // converting key to bytes will throw an exception, need to replace '-' and '_' characters first.
            string usablePrivateKey = keyString.Replace("-", "+").Replace("_", "/");
            byte[] privateKeyBytes = Convert.FromBase64String(usablePrivateKey);

            Uri uri = new Uri(url);
            byte[] encodedPathAndQueryBytes = encoding.GetBytes(uri.LocalPath + uri.Query);

            // compute the hash
            HMACSHA1 algorithm = new HMACSHA1(privateKeyBytes);
            byte[] hash = algorithm.ComputeHash(encodedPathAndQueryBytes);

            // convert the bytes to string and make url-safe by replacing '+' and '/' characters
            string signature = Convert.ToBase64String(hash).Replace("+", "-").Replace("/", "_");

            // Add the signature to the existing URI.
            return uri.Scheme + "://" + uri.Host + uri.LocalPath + uri.Query + "&signature=" + signature;
        }
    }

    public class MapPoint
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
