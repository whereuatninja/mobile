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
    public partial class MainPage : ContentPage
    {
        private ListView locationListView;
        private ObservableCollection<string> locationList;
        private int minTimeInterval = 5000;
        private double minDistance = 0;
        private IGeolocator _locator;

        public MainPage()
        {
            locationList = new ObservableCollection<string>();

            Label headerLabel = new Label
            {
                Text = "Whereu@Ninja",
                Font = Font.SystemFontOfSize(50),
                HorizontalOptions = LayoutOptions.Center
            };

            Button button = new Button
            {
                Text = "What is my location?",
                HorizontalOptions = LayoutOptions.Center
            };
            button.Clicked += OnGetLocation;

            locationListView = new ListView
            {
                ItemsSource = locationList
            };

            this.Content = new StackLayout
            {
                Children =
                {
                    headerLabel,
                    button,
                    locationListView
                }
            };
            
            setupGeoLocator();
            setupTimer();
        }

        private void setupGeoLocator()
        {
            _locator = CrossGeolocator.Current;
            _locator.AllowsBackgroundUpdates = true;
        }

        async void OnGetLocation(object sender, EventArgs e)
        {
            try
            {
                //Location location = await getCurrentLocation();
                //locationList.Add(String.Format("Longitude: {0} Latitude: {1}", location.Longitude, location.Latitude));
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to get location");
            }
        }

        async private Task<Position> getCurrentLocation()
        {
            Position position = null;
            try
            {
                position = await _locator.GetPositionAsync(timeoutMilliseconds: 5000);
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Unable to get location: "+ex.Message);
            }
            
            return position;
        }

        private void setupTimer()
        {
            IAdvancedTimer timer = DependencyService.Get<IAdvancedTimer>();
            timer.initTimer(10000, timerElapsed, true);
            timer.startTimer();
        }

        async private void addCurrentLocationToList()
        {
            var locator = CrossGeolocator.Current;
            var position = await locator.GetPositionAsync(timeoutMilliseconds: 10000);
            if (position != null)
            {
                locationList.Add(String.Format("Longitude: {0} Latitude: {1}", position.Longitude, position.Latitude));
            }
        }

        private void timerElapsed(object sender, EventArgs e)
        {
            Debug.WriteLine("Boom! Timer went off");
            addCurrentLocationToList();
        }
    }
}
