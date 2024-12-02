using Microsoft.Extensions.Logging;
using MQ.Finder.Data.Finder.Cache;
using MQ.Finder.Data.Loader;
using MQ.Finder.Data.Loader.Entities;

namespace MQ.Finder.Data.Finder
{
    /// <summary>
    /// Caching for search engine since there is requirement
    /// to serve 100m queries per day
    /// </summary>
    public class CachedDataFinder : DataFinder
    {
        private readonly ICacheService _cache;

        public CachedDataFinder(ILogger<DataFinder> logger, IDataLoader dataLoader, ICacheService cache) 
            : base(logger, dataLoader)
        {
            _cache = cache;
        }

        public override IEnumerable<Location> FindLocationByIp(string ip)
        {
            return Find(base.FindLocationByIp, ip);
        }

        public override IEnumerable<Location> FindLocationsByCity(string city)
        {
            return Find(base.FindLocationsByCity, city);
        }

        private IEnumerable<Location> Find(Func<string, IEnumerable<Location>> search, string searchParam)
        {
            var cacheKey = $"{nameof(search)}_{searchParam}";
            var result = _cache.Get<IEnumerable<Location>>(cacheKey);

            if (result != null)
            {
                _logger.LogInformation($"Getting data from cache for search by {searchParam}");
                return result;
            }

            result = search(searchParam);
            _cache.Set(cacheKey, result, TimeSpan.FromMinutes(5));

            return result;
        }
    }
}
