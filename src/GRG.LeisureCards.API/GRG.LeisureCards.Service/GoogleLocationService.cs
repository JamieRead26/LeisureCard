using System;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace GRG.LeisureCards.Service
{
    public interface IGoogleLocationService
    {
        /// <summary>
        /// Gets the latitude and longitude that belongs to an address.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns></returns>
        MapPoint GetLatLongFromAddress(AddressData address);
    }

    public class GoogleLocationService : IGoogleLocationService
    {
        private readonly string _apiKey;

        #region Constants
        const string API_LATLONG_FROM_ADDRESS = "maps.googleapis.com/maps/api/geocode/xml?address={0}&sensor=false";
        #endregion


        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="GoogleLocationService"/> class.
        /// </summary>
        /// <param name="useHttps">Indicates whether to call the Google API over HTTPS or not.</param>
        public GoogleLocationService(bool useHttps, string apiKey)
        {
            _apiKey = apiKey;
            UseHttps = useHttps;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GoogleLocationService"/> class. Default calling the API over regular
        /// HTTP (not HTTPS).
        /// </summary>
        public GoogleLocationService(string apiKey)
            : this(false, apiKey)
        { }
        #endregion


        #region Properties
        /// <summary>
        /// Gets a value indicating whether to use the Google API over HTTPS.
        /// </summary>
        /// <value>
        ///   <c>true</c> if using the API over HTTPS; otherwise, <c>false</c>.
        /// </value>
        public bool UseHttps { get; private set; }


        private string UrlProtocolPrefix
        {
            get
            {
                return UseHttps ? "https://" : "http://";
            }
        }

        protected string APIUrlLatLongFromAddress
        {
            get
            {
                var url = (string.IsNullOrWhiteSpace(_apiKey))
                    ? UrlProtocolPrefix + API_LATLONG_FROM_ADDRESS
                    : UrlProtocolPrefix + API_LATLONG_FROM_ADDRESS + "&key=" + _apiKey;

                return url;

            }
        }
        #endregion


        /// <summary>
        /// Gets the latitude and longitude that belongs to an address.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns></returns>
        /// <exception cref="System.Net.WebException"></exception>
        public MapPoint GetLatLongFromAddress(string address)
        {
            XDocument doc = XDocument.Load(string.Format(APIUrlLatLongFromAddress, Uri.EscapeDataString(address)));

            string status = doc.Descendants("status").FirstOrDefault().Value;
            if (status == "OVER_QUERY_LIMIT" || status == "REQUEST_DENIED")
            {
                throw new System.Net.WebException("Request Not Authorized or Over QueryLimit");
            }

            var els = doc.Descendants("result").Descendants("geometry").Descendants("location").FirstOrDefault();
            if (null != els)
            {
                var latitude = ParseUS((els.Nodes().First() as XElement).Value);
                var longitude = ParseUS((els.Nodes().ElementAt(1) as XElement).Value);
                return new MapPoint() { Latitude = latitude, Longitude = longitude };
            }
            return null;
        }

        /// <summary>
        /// Gets the latitude and longitude that belongs to an address.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns></returns>
        public MapPoint GetLatLongFromAddress(AddressData address)
        {
            return GetLatLongFromAddress(address.ToString());
        }


        double ParseUS(string value)
        {
            return Double.Parse(value, new CultureInfo("en-US"));
        }
    }

    public class MapPoint
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class AddressData
    {
        public string UkPostCodeOrTown { get; set; }

        public override string ToString()
        {
            return string.Format(
                "{0},United Kingdom",
                UkPostCodeOrTown);
        }
    }
}
