﻿using System;
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
        /// <param name="commaSeparatedAddress">The address.</param>
        /// <returns></returns>
        MapPoint GetLatLongFromAddress(string commaSeparatedAddress);
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
        /// <param name="commaSeparatedAddress">The address.</param>
        /// <returns></returns>
        /// <exception cref="System.Net.WebException"></exception>
        public MapPoint GetLatLongFromAddress(string commaSeparatedAddress)
        {
            var url = string.Format(APIUrlLatLongFromAddress, Uri.EscapeDataString(commaSeparatedAddress));

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
    }

    public class MapPoint
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
