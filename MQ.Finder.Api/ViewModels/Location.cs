namespace MQ.Finder.Api.Models
{
    public record Location
    {
        public string Country { get; init; }
        public string Region { get; init; }
        public string Postal { get; init; }
        public string City { get; init; }
        public string Organization { get; init; }
        public float Latitude { get; init; }
        public float Longitude { get; init; }

        public Location(Data.Loader.Entities.Location location)
        {
            Country = location.GetCountry();
            Region = location.GetRegion();
            Postal = location.GetPostal();
            City = location.GetCity();
            Organization = location.GetOrganization();
            Latitude = location.Latitude;
            Longitude = location.Longitude;
        }
    }
}
