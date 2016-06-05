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
            this.twitterUrlEntry = new Entry {
                Placeholder = "Twitter URL",
                Text = twitterUrl,
                Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeSentence)
            };

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

        private async void OnPostActivityClicked(object sender, EventArgs e)
        {
            LocationService.getInstance().sendCurrentLocation(messageEditor.Text, twitterUrlEntry.Text);
            //await this.DisplayAlert("", "Clicked!", "OK");
            //await App.Instance.MainNav.Navigation.PopAsync();
            await this.Navigation.PopAsync();
        }
    }
}
