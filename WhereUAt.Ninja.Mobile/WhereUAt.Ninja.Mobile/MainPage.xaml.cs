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
using WhereUAt.Ninja.Mobile.Messages;

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
            Debug.WriteLine("MainPage");
            setupView();
            setupSettings();
            setupGeoLocator();
            //getLocationLoop();
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

            Button startServiceButton = new Button
            {
                Text = "Start Tracking"
            };

            startServiceButton.Clicked += StartServiceButton_Clicked;

            locationListView = new ListView
            {
                ItemsSource = locationList
            };

            this.Content = new StackLayout
            {
                Children =
                {
                    headerLabel,
                    startServiceButton,
                    locationListView
                }
            };
        }

        private void StartServiceButton_Clicked(object sender, EventArgs e)
        {
            //var locationTask = new Task(() => { getLocationLoop(); });
            //locationTask.Start();
            var message = new StartLocationBackgroundService();
            MessagingCenter.Send(message, "StartLocationBackgroundService");
        }

        private void setupSettings()
        {
            this.settings = Settings.getInstance();
        }

        private async void getLocationLoop()
        {
            while (true)
            {
                if (settings.IsLocationTrackerOn/* && App.Instance.IsAuthenticated*/)
                {
                    await Task.Factory.StartNew(async () =>
                    {
                        Position position = await getCurrentLocation();
                        if (position != null)
                        {
                            addCurrentLocationToList(position);
                            sendLocation(position);
                        }
                    });
                }
                else
                {
                    Debug.WriteLine("Not authenticated and/or location tracker off");
                }
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

        private void sendLocation(Position position)
        {
            Debug.WriteLine("sendLocation");
            LocationService.sendLocation(position);
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

