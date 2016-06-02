using System;
using System.Diagnostics;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Plugin.Geolocator.Abstractions;
using WhereUAt.Ninja.Mobile.Messages;
using System.ComponentModel;

namespace WhereUAt.Ninja.Mobile
{
    public partial class MainPage : BaseContentPage
    {
        private StatusPage statusPage;

        //view elements
        private SwitchCell locationTrackingSwitchCell;
        private EntryCell trackingIntervalEntryCell;
        private Button loginButton;
        private Label loggedInUserStatusLabel;
        private Label authorizationStatus;
        private MainPageViewModel viewModel;

        private Settings settings;

        public MainPage()
        {
            Debug.WriteLine("MainPage");
            setupSettings();
            setupView();
            statusPage = new StatusPage();
        }
        

        private void setupView()
        {
            viewModel = new MainPageViewModel();
            Label header = new Label
            {
                Text = "Whereu@Ninja",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center
            };

            loggedInUserStatusLabel = new Label
            {
                Text = "Please log in",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.Start
            };

            loggedInUserStatusLabel.SetBinding(Label.TextProperty, "LoggedInUserStatusLabelText");
            loggedInUserStatusLabel.BindingContext = viewModel;

            Label settingsLabel = new Label
            {
                Text = "Settings:",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.Start
            };

            loginButton = new Button
            {
                Text = "Log in",
                Font = Font.SystemFontOfSize(NamedSize.Large),
                BorderWidth = 1,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            loginButton.Clicked += OnLoginButtonClicked;

            Button activityButton = new Button
            {
                Text = "Post an Activity",
                Font = Font.SystemFontOfSize(NamedSize.Large),
                BorderWidth = 1,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            activityButton.Clicked += OnActivityButtonClicked;

            Button statusButton = new Button
            {
                Text = "Status",
                Font = Font.SystemFontOfSize(NamedSize.Large),
                BorderWidth = 1,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            statusButton.Clicked += OnStatusButtonClicked;

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
                    loggedInUserStatusLabel,
                    loginButton,
                    activityButton,
                    statusButton,
                    settingsLabel,
                    tableView,
                    saveSettingsButton
                }
            };
        }

        private void OnStatusButtonClicked(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(statusPage);
        }

        private void OnActivityButtonClicked(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(new ActivityPage(""));
        }

        private void OnLoginButtonClicked(object sender, EventArgs e)
        {
            if (!App.Instance.IsAuthenticated)
            {
                //App.Instance.MainNav = this;
                //await this.Navigation.PushModalAsync(new LoginPage());
                App.Instance.MainNav = this;
                this.Navigation.PushAsync(new LoginPage());
            }
        }

        private void OnSaveButtonClicked(object sender, EventArgs e)
        {
            saveSettings();
            startOrStopLocationBackgroundService();
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

        private void saveSettings()
        {
            settings.IsLocationTrackerOn = this.locationTrackingSwitchCell.On;
            settings.LocationTimeIntervalInMilliseconds = int.Parse(this.trackingIntervalEntryCell.Text) * 1000;
        }

        private async void startOrStopLocationBackgroundService()
        {
            if (settings.IsLocationTrackerOn)
            {
                startLocationBackgroundService();
                await this.DisplayAlert("", "Location Tracker is on", "OK");
            }
            else
            {
                stopLocationBackgroundService();
                await this.DisplayAlert("", "Location Tracker is off", "OK");
            }
        }

        private void startLocationBackgroundService()
        {
            var message = new StartLocationBackgroundService();
            MessagingCenter.Send(message, "StartLocationBackgroundService");
        }

        private void stopLocationBackgroundService()
        {
            var message = new StopLocationBackgroundService();
            MessagingCenter.Send(message, "StopLocationBackgroundService");
        }

        private void setupSettings()
        {
            this.settings = Settings.getInstance();
        }

        private void update()
        {

        }

    }
}

