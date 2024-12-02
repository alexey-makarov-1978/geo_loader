using MQ.Finder.Data.Loader.Entities;

namespace MQ.Finder.Data.Finder.Tests
{
    public static class LocationExtensions
    {
        public static unsafe void SetCity(ref Location location, string city)
        {
            if (city.Length > 24)
                throw new ArgumentException("City name must not exceed 24 characters.", nameof(city));

            fixed (sbyte* cityPtr = location.City)
            {
                SetFixedField(city, cityPtr, 24);
            }
        }

        private static unsafe void SetFixedField(string value, sbyte* fieldPtr, int maxLength)
        {
            byte* bytePtr = (byte*)fieldPtr;
            int length = Math.Min(value.Length, maxLength);

            for (int i = 0; i < length; i++)
            {
                bytePtr[i] = (byte)value[i];
            }

            // Null-terminate the field if there is remaining space
            if (length < maxLength)
            {
                bytePtr[length] = 0;
            }
        }
    }
}
