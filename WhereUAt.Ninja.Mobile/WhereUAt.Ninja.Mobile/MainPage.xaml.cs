using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace WhereUAt.Ninja.Mobile
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
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

            this.Content = new StackLayout
            {
                Children =
                {
                    headerLabel,
                    button
                }
            };
            //InitializeComponent();
        }

        async void OnGetLocation(object sender, EventArgs e)
        {
            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 50;
                var position = await locator.GetPositionAsync(timeoutMilliseconds: 10000);
                await this.DisplayAlert("Location:", String.Format("Longitude: {0}, Latitude: {1}", position.Longitude, position.Latitude), "OK");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to get location");
            }
        }
    }
}
