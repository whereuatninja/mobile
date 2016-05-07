using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using System.Threading;
using AdvancedTimer.Forms.Plugin.Abstractions;
using Plugin.Geolocator.Abstractions;

namespace WhereUAt.Ninja.Mobile
{
    public partial class MainPage : BaseContentPage
    {
        private ListView locationListView;
        private ObservableCollection<string> locationList;
        private IGeolocator _locator;

        private Settings settings;

        public MainPage()
        {
            setupView();
            setupSettings();
            setupGeoLocator();
            getLocationLoop();
        }

        private void setupView()
        {
            locationList = new ObservableCollection<string>();

            Label headerLabel = new Label
            {
                Text = "Whereu@Ninja",
                Font = Font.SystemFontOfSize(50),
                HorizontalOptions = LayoutOptions.Center
            };

            locationListView = new ListView
            {
                ItemsSource = locationList
            };

            this.Content = new StackLayout
            {
                Children =
                {
                    headerLabel,
                    locationListView
                }
            };
        }

        private void setupSettings()
        {
            this.settings = new Settings();
        }

        private async void getLocationLoop()
        {
            while(settings.IsLocationTrackerOn)
            {
                await Task.Factory.StartNew(async () =>
                {
                    Position position = await getCurrentLocation();
                    addCurrentLocationToList(position);
                });
                await Task.Delay(settings.LocationTimeIntervalInMilliseconds);
            }
        }

        private void setupGeoLocator()
        {
            _locator = CrossGeolocator.Current;
            _locator.AllowsBackgroundUpdates = true;
        }

        async private Task<Position> getCurrentLocation()
        {
            Position position = null;
            try
            {
                position = await _locator.GetPositionAsync(timeoutMilliseconds: settings.LocationTimeOutInMilliseconds);
                await _locator.StopListeningAsync();
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Unable to get location: "+ex.Message);
            }
            
            return position;
        }

        private void addCurrentLocationToList(Position position)
        {
            if (position != null)
            {
                locationList.Add(String.Format("Longitude: {0} Latitude: {1}", position.Longitude, position.Latitude));
                Debug.WriteLine(String.Format("Longitude: {0} Latitude: {1}", position.Longitude, position.Latitude));
            }
        }
    }
}

