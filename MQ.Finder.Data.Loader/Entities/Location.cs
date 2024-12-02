using System.Runtime.InteropServices;
using System.Text;

namespace MQ.Finder.Data.Loader.Entities
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct Location
    {
        public fixed sbyte Country[8];
        public fixed sbyte Region[12];
        public fixed sbyte Postal[12];
        public fixed sbyte City[24];
        public fixed sbyte Organization[32];
        public float Latitude;
        public float Longitude;

        public string GetCountry()
        {
            fixed (sbyte* countryPtr = Country)
            {
                return ConvertToString(countryPtr, 8);
            }
        }

        public string GetRegion()
        {
            fixed (sbyte* regionPtr = Region)
            {
                return ConvertToString(regionPtr, 12);
            }
        }

        public string GetPostal()
        {
            fixed (sbyte* postalPtr = Postal)
            {
                return ConvertToString(postalPtr, 12);
            }
        }

        public string GetCity()
        {
            fixed (sbyte* cityPtr = City)
            {
                return ConvertToString(cityPtr, 24);
            }
        }

        public string GetOrganization()
        {
            fixed (sbyte* orgPtr = Organization)
            {
                return ConvertToString(orgPtr, 32);
            }
        }

        private static string ConvertToString(sbyte* field, int length)
        {
            byte* bytePtr = (byte*)field;
            int actualLength = 0;

            while (actualLength < length && bytePtr[actualLength] != 0)
            {
                actualLength++;
            }

            return Encoding.ASCII.GetString(bytePtr, actualLength).Trim();
        }
    }
}
