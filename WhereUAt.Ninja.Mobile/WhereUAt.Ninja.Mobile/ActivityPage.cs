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
        public ActivityPage()
        {
            this.messageEditor = new Editor{};

            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "Post an Activity!" },
                    this.messageEditor,
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
            LocationService.getInstance().sendCurrentLocation(messageEditor.Text);
            await this.DisplayAlert("", "Clicked!", "OK");
        }
    }
}
