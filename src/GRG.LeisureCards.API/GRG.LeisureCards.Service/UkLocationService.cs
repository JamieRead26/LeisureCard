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
        MapPoint GetMapPoint(string[] locations, int retries = 3);
    }

    public class UkLocationService : IUkLocationService
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(UkLocationService));

        private readonly ILocationRepository _locationRepository;
        private readonly IGoogleLocationService _googleLocationService;
        
        private IDictionary<string,Location> _locations;
        private readonly object _locationLock = new object();
        private IDictionary<string, Location> Locations
        {
            get
            {
                if (_locations == null)
                {
                    lock (_locationLock)
                    {
                        if (_locations == null)
                        {
                            _locations = _locationRepository.GetAll().ToDictionary(x => x.UkPostcodeOrTown, x => x);
                        }
                    }
                }

                return _locations;
            }
        }

        public UkLocationService(ILocationRepository locationRepository, IGoogleLocationService googleLocationService)
        {
            _locationRepository = locationRepository;
            _googleLocationService = googleLocationService;

            
        }

        public MapPoint GetMapPoint(string ukPostCodeOrTown)
        {
            return GetMapPoint(new []{ukPostCodeOrTown});
        }
        public MapPoint GetMapPoint(string[] locations, int retries = 3)
        {
            if (locations.All(string.IsNullOrWhiteSpace))
                return null;

            var locationKey = locations.Union(new[]{"UK"}).GetCommaSeparatedKey();

            if (Locations.ContainsKey(locationKey))
                return new MapPoint { Latitude = Locations[locationKey].Latitude, Longitude = Locations[locationKey].Longitude }; ;

            try
            {
                var mapPoint = _googleLocationService.GetLatLongFromAddress(locationKey);

                if (mapPoint == null)
                {
                    Log.Error("Unable to get map point for " + locationKey);
                }
                else
                {
                    ThreadPool.QueueUserWorkItem(CacheLocation, new Tuple<MapPoint, string>(mapPoint, locationKey));

                    return mapPoint;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Exception occurred calling google maps API : " + locationKey + " : " + ex);

                if (retries-- > 0)
                {
                    Thread.Sleep(500);
                    return GetMapPoint(locations, retries);
                }
            }

            return null;
        }

        private readonly object _cacheLock = new object();
        private void CacheLocation(object state)
        {
            var tuple = (Tuple<MapPoint, string>) state;

            if (Locations.ContainsKey(tuple.Item2) || tuple.Item1 == null)
                return;

            lock (_cacheLock)
            {
                if (Locations.ContainsKey(tuple.Item2))
                    return;

                var location = new Location
                {
                    UkPostcodeOrTown = tuple.Item2,
                    Latitude = tuple.Item1.Latitude,
                    Longitude = tuple.Item1.Longitude
                };

                Locations.Add(location.UkPostcodeOrTown, location);

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
