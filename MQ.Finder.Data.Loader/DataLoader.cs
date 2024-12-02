using Microsoft.Extensions.Logging;
using MQ.Finder.Data.Loader.Entities;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MQ.Finder.Data.Loader
{
    public class DataLoader : IDataLoader
    {
        private readonly GeoBase _data;
        private readonly ILogger<DataLoader> _logger;

        public DataLoader(ILogger<DataLoader> logger) 
        {
            _logger = logger;
            _data = LoadData(@"data\geobase.dat");
        }

        public GeoBase Data 
        { 
            get
            {
                return _data;
            }
        }

        public virtual GeoBase LoadData(string filePath)
        {
            var data = File.ReadAllBytes(filePath);

            // Start measure from here because we don't count data load from drive to memory
            // as the task description states
            var stopwatch = Stopwatch.StartNew();

            var span = new Span<byte>(data);
            var header = MemoryMarshal.Read<Header>(span);

            var db = new GeoBase
            {
                IpRanges = ReadCollection<IpRange>(span, header.Records, header.OffsetRanges),
                Locations = ReadCollection<Location>(span, header.Records, header.OffsetLocations),
                CityIndices = ReadCollection<uint>(span, header.Records, header.OffsetCities)
            };

            // convert memory offset to array index
            for (var i = 0; i < header.Records; i++)
            {
                db.CityIndices[i] = (uint) (db.CityIndices[i] / Unsafe.SizeOf<Location>());
            }

            stopwatch.Stop();
            _logger.LogInformation($"Database parsed in: {stopwatch.ElapsedMilliseconds} ms");

            return db;
        }

        private static T[] ReadCollection<T>(Span<byte> span, int recordCount, uint offset) where T : struct
        {
            var structSpan = MemoryMarshal.Cast<byte, T>(span.Slice((int)offset, recordCount * Unsafe.SizeOf<T>()));
            return structSpan.ToArray();
        }
    }
}
