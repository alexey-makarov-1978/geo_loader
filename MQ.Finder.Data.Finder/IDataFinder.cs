using MQ.Finder.Data.Loader.Entities;

namespace MQ.Finder.Data.Finder
{
    public interface IDataFinder
    {
        IEnumerable<Location> FindLocationByIp(string ip);
        IEnumerable<Location> FindLocationsByCity(string city);
    }
}
