using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using GRG.LeisureCards.DomainModel;
using GRG.LeisureCards.Persistence;

namespace GRG.LeisureCards.Service
{
    public interface IUkLocationService
    {IEnumerable<Tuple<TDestination, double>> Filter<TDestination>(string ukPostCodeOrTown, int radiusMiles,
            IEnumerable<TDestination> set) where TDestination : ILatLong;

        MapPoint GetMapPoint(string ukPostCodeOrTown);
    }

    public class UkLocationService : IUkLocationService
    {
        private readonly ILocationRepository _locationRepository;
        private readonly GoogleLocationService _mapsApi;
        private readonly IDictionary<string,Location> _locations;

        public UkLocationService(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
            _mapsApi = new GoogleLocationService();

            _locations = _locationRepository.GetAll().ToDictionary(x=>x.UkPostcodeOrTown, x=>x);
        }

        public MapPoint GetMapPoint(string ukPostCodeOrTown)
        {
            if (string.IsNullOrWhiteSpace(ukPostCodeOrTown))
                return new MapPoint {Latitude = 1000, Longitude = 1000};

            ukPostCodeOrTown = ukPostCodeOrTown.Trim().ToUpper();

            if (_locations.ContainsKey(ukPostCodeOrTown))
            {
                var location = _locations[ukPostCodeOrTown];
                return new MapPoint {Latitude = location.Latitude, Longitude = location.Longitude};
            }

            try
            {
                var mapPoint = _mapsApi.GetLatLongFromAddress(new AddressData
                {
                    UkPostCodeOrTown = ukPostCodeOrTown,
                });

                ThreadPool.QueueUserWorkItem(CacheLocation, new Tuple<MapPoint, string>(mapPoint, ukPostCodeOrTown));

                return mapPoint;
            }
            catch (Exception)
            {
                //TOFO: Log4net
                return new MapPoint { Latitude = 1000, Longitude = 1000 };
            }
        }

        private readonly object _cacheLock = new object();
        private void CacheLocation(object state)
        {
            var tuple = (Tuple<MapPoint, string>) state;

            if (_locations.ContainsKey(tuple.Item2))
                return;

            lock (_cacheLock)
            {
                if (_locations.ContainsKey(tuple.Item2))
                    return;

                var location = new Location
                {
                    UkPostcodeOrTown = tuple.Item2,
                    Latitude = tuple.Item1.Latitude,
                    Longitude = tuple.Item1.Longitude
                };

                _locations.Add(location.UkPostcodeOrTown, location);

                _locationRepository.SaveOrUpdate(location);
            }
        }

        public IEnumerable<Tuple<TDestination, double>> Filter<TDestination>(string ukPostCodeOrTown, int radiusMiles,
            IEnumerable<TDestination> set) where TDestination : ILatLong
        {
            var from = GetMapPoint(ukPostCodeOrTown);

            var results = new List<Tuple<TDestination, double>>();

            foreach (var destination in set)
            {
                try
                {
                    var distance = CalcDistanceMiles(from, new MapPoint { Latitude = destination.Latitude, Longitude = destination.Longitude });

                    if (distance<=radiusMiles)
                        results.Add(new Tuple<TDestination, double>(destination, distance));
                }
                catch (Exception ex)
                {
                    //TODO: Log4Net
                }
            }

            return results;
        }
        
        public double CalcDistanceMiles(MapPoint from, MapPoint to)
        {
            /*
                The Haversine formula according to Dr. Math.
                http://mathforum.org/library/drmath/view/51879.html
                
                dlon = lon2 - lon1
                dlat = lat2 - lat1
                a = (sin(dlat/2))^2 + cos(lat1) * cos(lat2) * (sin(dlon/2))^2
                c = 2 * atan2(sqrt(a), sqrt(1-a)) 
                d = R * c
                
                Where
                    * dlon is the change in longitude
                    * dlat is the change in latitude
                    * c is the great circle distance in Radians.
                    * R is the radius of a spherical Earth.
                    * The locations of the two points in 
                        spherical coordinates (longitude and 
                        latitude) are lon1,lat1 and lon2, lat2.
            */
            double dDistance = Double.MinValue;
            double dLat1InRad = from.Latitude * (Math.PI / 180.0);
            double dLong1InRad = from.Longitude * (Math.PI / 180.0);
            double dLat2InRad = to.Latitude * (Math.PI / 180.0);
            double dLong2InRad = to.Longitude * (Math.PI / 180.0);

            double dLongitude = dLong2InRad - dLong1InRad;
            double dLatitude = dLat2InRad - dLat1InRad;

            // Intermediate result a.
            double a = Math.Pow(Math.Sin(dLatitude / 2.0), 2.0) +
                       Math.Cos(dLat1InRad) * Math.Cos(dLat2InRad) *
                       Math.Pow(Math.Sin(dLongitude / 2.0), 2.0);

            // Intermediate result c (great circle distance in Radians).
            double c = 2.0 * Math.Asin(Math.Sqrt(a));

            // Distance.
            // const Double kEarthRadiusMiles = 3956.0;
            const Double kEarthRadiusKms = 6376.5;
            dDistance = kEarthRadiusKms * c;

            return (dDistance/8)*5;//convert to miles
        }
    }
}
