using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WhereUAt.Ninja.Mobile
{
    public class LocationTimerService
    {
        private IGeolocator _locator;
        private Settings settings;

        public LocationTimerService()
        {
            setupSettings();
            setupGeoLocator();
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

        public async Task startService(CancellationToken token)
        {
            await Task.Run(async () => {

                while(true)
                {
                    token.ThrowIfCancellationRequested();

                    await Task.Delay(settings.LocationTimeIntervalInMilliseconds);

                    Debug.WriteLine("LocationTimerService ping");
                    if (settings.IsLocationTrackerOn)
                    {
                        await Task.Factory.StartNew(async () =>
                        {
                            Position position = await getCurrentLocation();
                            if (position != null)
                            {
                                sendLocation(position);
                            }
                        });
                    }
                }
            }, token);
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

        private void sendLocation(Position position)
        {
            Debug.WriteLine("sendLocation");
            LocationService.sendLocation(position);
        }
    }
}
