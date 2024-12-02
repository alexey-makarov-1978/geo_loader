using MQ.Finder.Api.Models;

namespace MQ.Finder.Api.Helpers
{
    public class ViewModelHelper
    {
        public static IEnumerable<Location> ConvertLocations(IEnumerable<Data.Loader.Entities.Location> locations)
        {
            return locations.Select(l => new Location(l));
        }
    }
}
