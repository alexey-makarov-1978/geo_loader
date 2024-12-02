namespace MQ.Finder.Data.Loader.Entities
{
    public record GeoBase
    {
        public Header Header { get; set; }
        public IpRange[] IpRanges { get; set; }
        public Location[] Locations { get; set; }
        public uint[] CityIndices { get; set; }
    }
}
