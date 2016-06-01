using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms;
using WhereUAt.Ninja.Mobile.Messages;
using Android.Content;
using Android.Util;
using WhereUAt.Ninja.Mobile.Droid.Services;

namespace WhereUAt.Ninja.Mobile.Droid
{
    [Activity(Label = "WhereUAt.Ninja.Mobile", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    [IntentFilter(new[] { Intent.ActionSend },
    Categories = new[] { Intent.CategoryBrowsable, Intent.CategoryDefault }, DataMimeType = "text/plain")]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());

            if (!String.IsNullOrEmpty(Intent.GetStringExtra(Intent.ExtraText)))
            {
                Log.Info("MainActivity","ClipData:" + Intent.ClipData);
                string rawTwitterText = Intent.GetStringExtra(Intent.ExtraText);
                string twitterUrl = getTwitterUrlFromShare(rawTwitterText);
                //Toast.MakeText(this, subject, ToastLength.Long).Show();
                App._NavPage.PushAsync(new ActivityPage(twitterUrl));
            }

            MessagingCenter.Subscribe<StartLocationBackgroundService>(this, "StartLocationBackgroundService", message => {
                var intent = new Intent(this, typeof(LocationService));
                StartService(intent);
            });

            MessagingCenter.Subscribe<StopLocationBackgroundService>(this, "StopLocationBackgroundService", message => {
                Log.Debug("MainActivity", "StopLocationBackgroundService message received");
                var intent = new Intent(this, typeof(LocationService));
                StopService(intent);
            });

            IntentFilter twitterFilter = new IntentFilter(Intent.ActionSend);

        }

        private String getTwitterUrlFromShare(String rawTwitterText)
        {
            int start = rawTwitterText.IndexOf("http");
            return rawTwitterText.Substring(start);
        }

        protected void OnNewIntent()
        {

        }
    }
}

