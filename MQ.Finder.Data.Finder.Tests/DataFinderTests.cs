using FakeItEasy;
using Microsoft.Extensions.Logging;
using MQ.Finder.Data.Finder.Helpers;
using MQ.Finder.Data.Loader;
using MQ.Finder.Data.Loader.Entities;

namespace MQ.Finder.Data.Finder.Tests
{
    public class DataFinderTests
    {
        private readonly IDataLoader _fakeLoader;

        public DataFinderTests()
        {
            _fakeLoader = A.Fake<IDataLoader>();

            var db = new GeoBase()
            {
                IpRanges = [
                    new IpRange() {
                        IpFrom = IpHelper.IpStringToNumber("127.0.0.1"),
                        IpTo = IpHelper.IpStringToNumber("127.0.0.9"),
                        LocationIndex = 0
                    },
                    new IpRange() {
                        IpFrom = IpHelper.IpStringToNumber("127.0.0.10"),
                        IpTo = IpHelper.IpStringToNumber("127.0.0.11"),
                        LocationIndex = 1
                    },
                    new IpRange() {
                        IpFrom = IpHelper.IpStringToNumber("127.0.0.12"),
                        IpTo = IpHelper.IpStringToNumber("127.0.0.100"),
                        LocationIndex = 2
                    },
                    new IpRange() {
                        IpFrom = IpHelper.IpStringToNumber("127.0.0.101"),
                        IpTo = IpHelper.IpStringToNumber("127.0.0.102"),
                        LocationIndex = 3
                    },
                    new IpRange() {
                        IpFrom = IpHelper.IpStringToNumber("127.0.0.103"),
                        IpTo = IpHelper.IpStringToNumber("127.0.0.107"),
                        LocationIndex = 4
                    },
                    new IpRange() {
                        IpFrom = IpHelper.IpStringToNumber("127.0.0.108"),
                        IpTo = IpHelper.IpStringToNumber("127.0.0.199"),
                        LocationIndex = 5
                    },
                    new IpRange() {
                        IpFrom = IpHelper.IpStringToNumber("127.0.0.200"),
                        IpTo = IpHelper.IpStringToNumber("127.0.0.203"),
                        LocationIndex = 6
                    },
                    new IpRange() {
                        IpFrom = IpHelper.IpStringToNumber("127.0.0.204"),
                        IpTo = IpHelper.IpStringToNumber("127.0.0.213"),
                        LocationIndex = 7
                    },
                ],
                Locations = [
                    new Location() { Latitude = 1, Longitude = 2 },
                    new Location() { Latitude = 3, Longitude = 4 },
                    new Location() { Latitude = 5, Longitude = 6 },
                    new Location() { Latitude = 7, Longitude = 8 },
                    new Location() { Latitude = 9, Longitude = 10 },
                    new Location() { Latitude = 11, Longitude = 12 },
                    new Location() { Latitude = 13, Longitude = 14 },
                    new Location() { Latitude = 15, Longitude = 16 }
                ],
                CityIndices = [
                    1, 3, 4, 6, 5, 7, 0, 2
                ]
            };

            LocationExtensions.SetCity(ref db.Locations[0], "aA");
            LocationExtensions.SetCity(ref db.Locations[1], "Bb");
            LocationExtensions.SetCity(ref db.Locations[2], "d");
            LocationExtensions.SetCity(ref db.Locations[3], "D");
            LocationExtensions.SetCity(ref db.Locations[4], "D");
            LocationExtensions.SetCity(ref db.Locations[5], "F");
            LocationExtensions.SetCity(ref db.Locations[6], "Dd");
            LocationExtensions.SetCity(ref db.Locations[7], "H");

            A.CallTo(() => _fakeLoader.Data).Returns(db);

        }

        [Theory]
        [InlineData("127.0.0.8", 1, 2)]
        [InlineData("127.0.0.205", 15, 16)]
        [InlineData("127.0.0.108", 11, 12)]
        [InlineData("127.0.0.100", 5, 6)]
        [InlineData("127.0.0.1", 1, 2)]
        public void When_FindLocationByIp_IpInDb_Returns_Found(string ip, float latitude, float longitude)
        {
            // Arrange
            var fakeLogger = A.Fake<ILogger<DataFinder>>();
            var finder = new DataFinder(fakeLogger, _fakeLoader);

            // Act
            var locations = finder.FindLocationByIp(ip);

            // Assert
            Assert.NotNull(locations);
            Assert.Single(locations);
            Assert.Equal(latitude, locations.ToArray()[0].Latitude);
            Assert.Equal(longitude, locations.ToArray()[0].Longitude);
        }

        [Theory]
        [InlineData("127.0.0.0")]
        [InlineData("127.0.0.222")]
        [InlineData("255.0.0.8")]
        [InlineData("1.0.0.222")]
        public void When_FindLocationByIp_IpNotInDb_Returns_Empty(string ip)
        {
            // Arrange
            var fakeLogger = A.Fake<ILogger<DataFinder>>();
            var finder = new DataFinder(fakeLogger, _fakeLoader);

            // Act
            var locations = finder.FindLocationByIp(ip);

            // Assert
            Assert.NotNull(locations);
            Assert.Empty(locations);
        }

        [Theory]
        [InlineData("aA", 1)]
        [InlineData("Bb", 1)]
        [InlineData("D", 2)]
        [InlineData("Dd", 1)]
        [InlineData("F", 1)]
        [InlineData("H", 1)]
        [InlineData("d", 1)]
        public void When_FindLocationsByCity_CityInDb_Returns_Found(string city, int foundCount)
        {
            // Arrange
            var fakeLogger = A.Fake<ILogger<DataFinder>>();
            var finder = new DataFinder(fakeLogger, _fakeLoader);

            // Act
            var locations = finder.FindLocationsByCity(city);

            // Assert
            Assert.NotNull(locations);
            Assert.Equal(foundCount, locations.Count());
        }

        [Theory]
        [InlineData("aaAAA")]
        [InlineData("city_3fg")]
        [InlineData("z1dfs")]
        [InlineData("DDD")]
        public void When_FindLocationsByCity_CityNotInDb_Returns_Empty(string city)
        {
            // Arrange
            var fakeLogger = A.Fake<ILogger<DataFinder>>();
            var finder = new DataFinder(fakeLogger, _fakeLoader);

            // Act
            var locations = finder.FindLocationsByCity(city);

            // Assert
            Assert.NotNull(locations);
            Assert.Empty(locations);
        }

    }
}