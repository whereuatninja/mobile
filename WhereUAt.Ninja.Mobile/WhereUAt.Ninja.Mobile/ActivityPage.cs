using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace WhereUAt.Ninja.Mobile
{
    public class ActivityPage : ContentPage
    {
        private Editor messageEditor;
        private Entry twitterUrlEntry;

        public ActivityPage(String twitterUrl)
        {
            this.messageEditor = new Editor{};
            this.twitterUrlEntry = new Entry { Placeholder = "Twitter URL", Text = twitterUrl };

            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "Post an Activity!" },
                    this.messageEditor,
                    twitterUrlEntry,
                    createPostActivityButton()
                }
            };
        }

        private Button createPostActivityButton()
        {
            Button button = new Button
            {
                Text = "Post Activity"
            };
            button.Clicked += OnPostActivityClicked;

            return button;
        }

        private Button createTakePictureButton()
        {
            Button button = new Button
            {
                Text = "Take Picture"
            };
            button.Clicked += OnTakePictureClicked;

            return button;
        }

        private async void OnTakePictureClicked(object sender, EventArgs e)
        {
            /*await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "Sample",
                Name = "test.jpg"
            });

            if (file == null)
                return;

            await DisplayAlert("File Location", file.Path, "OK");
            Image image = new Image();
            image.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                file.Dispose();
                return stream;
            });*/
        }

        private async void OnPostActivityClicked(object sender, EventArgs e)
        {
            LocationService.getInstance().sendCurrentLocation(messageEditor.Text, twitterUrlEntry.Text);
            await this.DisplayAlert("", "Clicked!", "OK");
        }
    }
}
