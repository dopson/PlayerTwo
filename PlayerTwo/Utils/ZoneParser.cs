using System;

namespace PlayerTwo.Utils
{
    public static class ZoneParser
    {
        public static Zone GetZone(string zone)
        {
            if (string.IsNullOrWhiteSpace(zone))
            {
                return Zone.INVALID;
            }

            var parsedZone = Zone.INVALID;
            Enum.TryParse<Zone>(zone, false, out parsedZone);

            return parsedZone;
        }
    }
}
