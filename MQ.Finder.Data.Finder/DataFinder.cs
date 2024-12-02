using Microsoft.Extensions.Logging;
using MQ.Finder.Data.Finder.Helpers;
using MQ.Finder.Data.Loader;
using MQ.Finder.Data.Loader.Entities;
using System.Diagnostics;

namespace MQ.Finder.Data.Finder
{
    /// <summary>
    /// Search engine for database. Search by Ip or City
    /// </summary>
    public class DataFinder : IDataFinder
    {
        protected readonly ILogger<DataFinder> _logger;
        private readonly IDataLoader _dataLoader;

        public DataFinder(ILogger<DataFinder> logger, IDataLoader dataLoader)
        {
            _logger = logger;
            _dataLoader = dataLoader;
        }

        public virtual IEnumerable<Location> FindLocationByIp(string ip)
        {
            _logger.LogInformation($"FindLocationByIp starts searching by {ip}");
            var stopwatch = Stopwatch.StartNew();

            var ipRanges = _dataLoader.Data.IpRanges;
            var ipNumber = IpHelper.IpStringToNumber(ip);
            
            var lo = 0;
            var hi = ipRanges.Length - 1;

            while (lo <= hi)
            {
                var mid = lo + (hi - lo) / 2;
                var ipRange = ipRanges[mid];

                if (ipRange.IpFrom <= ipNumber && ipRange.IpTo >= ipNumber)
                {
                    stopwatch.Stop();
                    _logger.LogInformation($"FindLocationByIp found location for ip={ip}. Search time {stopwatch.ElapsedMilliseconds} ms");

                    return [_dataLoader.Data.Locations[ipRange.LocationIndex]];
                }

                if (ipRange.IpFrom > ipNumber)
                {
                    hi = mid - 1;
                }
                else
                {
                    lo = mid + 1;
                }
            }

            stopwatch.Stop();
            _logger.LogInformation($"FindLocationByIp found nothing for ip={ip}. Search time {stopwatch.ElapsedMilliseconds} ms");

            return [];
        }

        public virtual IEnumerable<Location> FindLocationsByCity(string city)
        {
            _logger.LogInformation($"FindLocationsByCity starts searching by {city}");
            var stopwatch = Stopwatch.StartNew();

            var cityIndices = _dataLoader.Data.CityIndices;

            var lo = 0;
            var hi = cityIndices.Length - 1;
            var result = -1;

            while (lo <= hi)
            {
                var mid = lo + (hi - lo) / 2;
                
                var location = _dataLoader.Data.Locations[cityIndices[mid]];

                var comparisonResult = string.Compare(city, location.GetCity(), StringComparison.Ordinal);

                if (comparisonResult == 0)
                {
                    result = mid;
                    break;
                }
                else if (comparisonResult < 0)
                {
                    hi = mid - 1;
                }
                else
                {
                    lo = mid + 1;
                }
            }

            if (result == -1)
            {
                stopwatch.Stop();
                _logger.LogInformation($"FindLocationsByCity found nothing for city={city}. Search time {stopwatch.ElapsedMilliseconds} ms");

                return [];
            }
            else
            {
                var foundLocations = new List<Location>() { _dataLoader.Data.Locations[cityIndices[result]] };

                var previous = result - 1;
                var next = result + 1;

                // since city indexes are sorted we just go forward and back
                // to check if the nearby indexes fit our search criteria
                while (previous >= 0)
                {
                    var location = _dataLoader.Data.Locations[cityIndices[previous--]];

                    var comparisonResult = string.Compare(city, location.GetCity(), StringComparison.Ordinal);

                    if (comparisonResult != 0)
                    {
                        break;
                    }

                    foundLocations.Add(location);
                }

                while (next < cityIndices.Length)
                {
                    var location = _dataLoader.Data.Locations[cityIndices[next++]];

                    var comparisonResult = string.Compare(city, location.GetCity(), StringComparison.Ordinal);

                    if (comparisonResult != 0)
                    {
                        break;
                    }

                    foundLocations.Add(location);
                }

                stopwatch.Stop();
                _logger.LogInformation($"FindLocationsByCity found {foundLocations.Count} locations for city={city}. Search time {stopwatch.ElapsedMilliseconds} ms");

                return foundLocations;
            }
        }
    }
}
