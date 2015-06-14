using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Threading;
using GRG.LeisureCards.DomainModel;
using GRG.LeisureCards.Persistence;
using log4net;

namespace GRG.LeisureCards.Service
{
    public interface IUkLocationService
    {
        IEnumerable<Tuple<TDestination, double>> Filter<TDestination>(
            string ukPostCodeOrTown,
            int radiusMiles,
            IEnumerable<TDestination> set,
            Action<TDestination> updateLatLong) where TDestination : ILatLong;

        MapPoint GetMapPoint(string ukPostCodeOrTown);
        MapPoint GetMapPoint(params string[] locations);
    }

    public class UkLocationService : IUkLocationService
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(UkLocationService));

        private readonly ILocationRepository _locationRepository;
        private readonly IGoogleLocationService _googleLocationService;
        private readonly IDictionary<string,Location> _locations;

        public UkLocationService(ILocationRepository locationRepository, IGoogleLocationService googleLocationService)
        {
            _locationRepository = locationRepository;
            _googleLocationService = googleLocationService;

            _locations = _locationRepository.GetAll().ToDictionary(x=>x.UkPostcodeOrTown, x=>x);
        }

        public MapPoint GetMapPoint(string ukPostCodeOrTown)
        {
            return GetMapPoint(new[] { ukPostCodeOrTown });
        }
        public MapPoint GetMapPoint(params string[] locations)
        {
            if (locations.All(string.IsNullOrWhiteSpace))
                return null;

            locations = locations.Where(l=>!string.IsNullOrWhiteSpace(l)).Select(l=>l.Trim().ToUpper()).ToArray();

            foreach (var location in from loc in locations where _locations.ContainsKey(loc) select _locations[loc])
            {
                return new MapPoint { Latitude = location.Latitude, Longitude = location.Longitude };
            }
            
            foreach (var location in locations.Where(location => !string.IsNullOrWhiteSpace(location)))
            {
                try
                {
                    var mapPoint = _googleLocationService.GetLatLongFromAddress(new AddressData
                    {
                        UkPostCodeOrTown = location,
                    });

                    if (mapPoint == null)
                    {
                        Log.Error("Unable to get map point for " + location);
                        continue;
                    }

                    ThreadPool.QueueUserWorkItem(CacheLocation, new Tuple<MapPoint, string>(mapPoint, location));

                    return mapPoint;
                }
                catch (Exception ex)
                {
                    Log.Error("Exception occurred calling google maps API : " + location + " : " + ex);
                }
            }

            return null;
        }

        private readonly object _cacheLock = new object();
        private void CacheLocation(object state)
        {
            var tuple = (Tuple<MapPoint, string>) state;

            if (_locations.ContainsKey(tuple.Item2) || tuple.Item1==null)
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
        
        public IEnumerable<Tuple<TDestination, double>> Filter<TDestination>(
            string ukPostCodeOrTown, 
            int radiusMiles,
            IEnumerable<TDestination> set,
            Action<TDestination> updateLatLong) where TDestination : ILatLong
        {
            var from = GetMapPoint(ukPostCodeOrTown);
            var results = new List<Tuple<TDestination, double>>();

            if (from == null)
            {
                Log.Error("Unable to filter location results as no coordinates available for from point: " + ukPostCodeOrTown);
                return set.Select(s=>new Tuple<TDestination, double>(s, -1));
            }
            
            foreach (var destination in set)
            {
                try
                {
                    if (!destination.Latitude.HasValue || !destination.Longitude.HasValue)
                    {
                        var mapPoint = GetMapPoint(destination.Locations);

                        if (mapPoint == null)
                            continue;

                        destination.Latitude = mapPoint.Latitude;
                        destination.Longitude = mapPoint.Longitude;

                        updateLatLong(destination);
                    }
                    
                    var distanceMetres = new GeoCoordinate(from.Latitude, from.Longitude).GetDistanceTo(
                        new GeoCoordinate(destination.Latitude.Value, destination.Longitude.Value));

                    var distanceMiles = ((distanceMetres/1000)/8)*5;

                    if (distanceMiles <= radiusMiles)
                        results.Add(new Tuple<TDestination, double>(destination, distanceMiles));
                }
                catch (Exception ex)
                {
                    Log.Error("Unable to calc distance for destination  " + destination.Locations.Aggregate(string.Empty, (a,s)=>a+":"+s), ex);
                }
            }

            return results.OrderBy(i=>i.Item2);
        }
    }
}
