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
using System.ComponentModel;

namespace WhereUAt.Ninja.Mobile
{
    public partial class MainPage : BaseContentPage
    {
        private ListView locationListView;
        private ObservableCollection<string> locationList;
        private IGeolocator _locator;

        //view elements
        private SwitchCell locationTrackingSwitchCell;
        private EntryCell trackingIntervalEntryCell;

        private Settings settings;

        public MainPage()
        {
            Debug.WriteLine("MainPage");
            setupSettings();
            setupGeoLocator();
            setupView();
            //getLocationLoop();
        }


        /*private void setupView()
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
        }*/

        private void setupView()
        {
            Label header = new Label
            {
                Text = "Whereu@Ninja",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center
            };

            Label settingsLabel = new Label
            {
                Text = "Settings:",
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                HorizontalOptions = LayoutOptions.Start
            };

            Button loginButton = new Button
            {
                Text = "Log in",
                Font = Font.SystemFontOfSize(NamedSize.Large),
                BorderWidth = 1,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            loginButton.Clicked += OnLoginButtonClicked;

            locationTrackingSwitchCell = new SwitchCell
            {
                On = false,
                Text = "Location Tracking:"
            };
            locationTrackingSwitchCell.OnChanged += OnLocationTrackingSwitchCellChanged;

            trackingIntervalEntryCell = new EntryCell
            {
                Label = "Tracking Interval:",
                Text = (settings.LocationTimeIntervalInMilliseconds / 1000) + "",
                Placeholder = "Enter time interval in seconds"
            };
            trackingIntervalEntryCell.Completed += OnTrackingIntervalEntryCellCompleted;
            trackingIntervalEntryCell.PropertyChanged += OnTrackingIntervalEntryCellPropertyChanged;

            Button saveSettingsButton = new Button
            {
                Text = "Save Settings",
                Font = Font.SystemFontOfSize(NamedSize.Large),
                BorderWidth = 1,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            saveSettingsButton.Clicked += OnSaveButtonClicked;

            TableView tableView = new TableView
            {
                Intent = TableIntent.Settings,
                Root = new TableRoot
                {
                    new TableSection
                    {
                        locationTrackingSwitchCell,
                        trackingIntervalEntryCell
                    }
                }
            };

            // Accomodate iPhone status bar.
            this.Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5);

            // Build the page.
            this.Content = new StackLayout
            {
                Children =
                {
                    header,
                    loginButton,
                    settingsLabel,
                    tableView,
                    saveSettingsButton
                }
            };
        }

        private void OnSaveButtonClicked(object sender, EventArgs e)
        {
            settings.IsLocationTrackerOn = this.locationTrackingSwitchCell.On;
            settings.LocationTimeIntervalInMilliseconds = int.Parse(this.trackingIntervalEntryCell.Text)*1000;
            if (settings.IsLocationTrackerOn && !App.Instance.IsLocationTrackerStarted)
            {
                startLocationBackgroundService();
            }
            Debug.WriteLine("Saved settings. IsLocationTrackerOn: {0}, LocationTimeIntervalInMilliseconds: {1}", settings.IsLocationTrackerOn, settings.LocationTimeIntervalInMilliseconds);
        }

        private void OnTrackingIntervalEntryCellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Debug.WriteLine("Property Changed");
        }

        private void OnLocationTrackingSwitchCellChanged(object sender, ToggledEventArgs e)
        {
            Debug.WriteLine("I got toggled");
        }

        private void OnTrackingIntervalEntryCellCompleted(object sender, EventArgs e)
        {
            Debug.WriteLine("Im done being filled out");
        }

        async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            Debug.WriteLine("I got clicked");
            if (!App.Instance.IsAuthenticated)
            {
                App.Instance.MainNav = this;
                await this.Navigation.PushModalAsync(new LoginPage());
            }
        }

        private void StartServiceButton_Clicked(object sender, EventArgs e)
        {
            //var locationTask = new Task(() => { getLocationLoop(); });
            //locationTask.Start();
            
        }

        private void startLocationBackgroundService()
        {
            var message = new StartLocationBackgroundService();
            MessagingCenter.Send(message, "StartLocationBackgroundService");
            App.Instance.IsLocationTrackerStarted = true;
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

