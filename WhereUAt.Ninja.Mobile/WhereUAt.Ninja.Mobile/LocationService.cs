using Plugin.Geolocator;
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
        private static LocationService instance;
        private IGeolocator _locator;
        private Settings settings;
        private WhereUAtNinjaAPI api;

        public static LocationService getInstance()
        {
            if (LocationService.instance == null)
            {
                LocationService.instance = new LocationService();
            }
            return LocationService.instance;
        }

        private LocationService()
        {
            setupSettings();
            setupGeoLocator();
            api = WhereUAtNinjaAPI.getInstance();
        }

        private void setupSettings()
        {
            this.settings = Settings.getInstance();
        }

        private void setupGeoLocator()
        {
            _locator = CrossGeolocator.Current;
            _locator.AllowsBackgroundUpdates = true;
        }
        
        public async void sendCurrentLocation(String message, String twitterUrl)
        {
            long timestamp = epochTimeStamp();
            Position position = await getCurrentLocation();
            Location location = new Location(position.Longitude, position.Latitude, timestamp, message, twitterUrl);
            Debug.WriteLine("Sending location");
            api.sendLocation(location);
        }

        private static long epochTimeStamp()
        {
            var timeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            return (long)timeSpan.TotalSeconds;
        }

        async private Task<Position> getCurrentLocation()
        {
            Position position = null;
            try
            {
                position = await _locator.GetPositionAsync(timeoutMilliseconds: settings.LocationTimeOutInMilliseconds);
                await _locator.StopListeningAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to get location: " + ex.Message);
            }

            return position;
        }
    }
}
