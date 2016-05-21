using Plugin.Geolocator.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhereUAt.Ninja.Mobile
{
    class LocationService
    {
        public static bool sendLocation(Position position)
        {
            Debug.WriteLine("LocationService.sendLocation");
            WhereUAtNinjaAPI api = WhereUAtNinjaAPI.getInstance();
            long timestamp = epochTimeStamp();
            Debug.WriteLine("timestamp: {0}", timestamp);
            Location location = new Location(position.Longitude, position.Latitude, timestamp);
            Task<bool> taskStatus = api.sendLocation(location);
            Debug.WriteLine("LocationService.sendLocation status:" + taskStatus.Result);
            return taskStatus.Result;
        }

        private static long epochTimeStamp()
        {
            var timeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            return (long)timeSpan.TotalSeconds;
        }
    }
}
