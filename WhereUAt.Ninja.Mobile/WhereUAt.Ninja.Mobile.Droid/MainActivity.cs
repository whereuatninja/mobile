﻿using System;

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
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());

            MessagingCenter.Subscribe<StartLocationBackgroundService>(this, "StartLocationBackgroundService", message => {
                var intent = new Intent(this, typeof(LocationService));
                StartService(intent);
            });
        }
    }
}

