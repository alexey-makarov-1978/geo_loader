using System.Net;

namespace MQ.Finder.Data.Finder.Helpers
{
    public class IpHelper
    {
        public static uint IpStringToNumber(string ip)
        {
            IPAddress ipAddress = IPAddress.Parse(ip);
            var bytes = ipAddress.GetAddressBytes();

            Array.Reverse(bytes);
            return BitConverter.ToUInt32(bytes, 0);
        }

        public static string IpNumberToString(uint ip)
        {
            var bytes = BitConverter.GetBytes(ip);

            Array.Reverse(bytes);
            var ipAddress = new IPAddress(bytes);

            return ipAddress.ToString();
        }
    }
}
