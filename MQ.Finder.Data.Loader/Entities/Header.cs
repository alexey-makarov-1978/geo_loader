using System.Runtime.InteropServices;

namespace MQ.Finder.Data.Loader.Entities
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct Header
    {
        public int Version;
        public fixed sbyte Name[32];
        public ulong Timestamp;
        public int Records;
        public uint OffsetRanges;
        public uint OffsetCities;
        public uint OffsetLocations;
    }
}
