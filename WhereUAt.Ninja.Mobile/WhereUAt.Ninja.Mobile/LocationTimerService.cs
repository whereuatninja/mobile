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

        public void startService(CancellationToken token)
        {
            Task.Run(async () => {

                while(true)
                {
                    token.ThrowIfCancellationRequested();

                    await Task.Delay(settings.LocationTimeIntervalInMilliseconds);

                    Debug.WriteLine("LocationTimerService ping");
                    if (settings.IsLocationTrackerOn)
                    {
                        await Task.Factory.StartNew(() =>
                         {
                             LocationService.getInstance().sendCurrentLocation("", "");
                         });
                    }
                }
            }, token);
        }
    }
}
