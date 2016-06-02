using Auth0.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace WhereUAt.Ninja.Mobile
{
    public class App : Application
    {
        public static NavigationPage _NavPage; 

        public App()
        {
            var mainPage = new MainPage();
            _NavPage = new NavigationPage(mainPage);
            MainPage = _NavPage;
        }

        static volatile App _Instance;
        static object _SyncRoot = new Object();
        public static App Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (_SyncRoot)
                    {
                        if (_Instance == null)
                        {
                            _Instance = new App();
                        }
                    }
                }
                return _Instance;
            }
        }

        public bool IsAuthenticated
        {
            get {
                return !string.IsNullOrWhiteSpace(_Token);
            }
        }

        Auth0Client _Auth0;
        public Auth0Client Auth0
        {
            get { return _Auth0; }
        }

        public void SaveAuth0(Auth0Client auth0)
        {
            _Auth0 = auth0;
        }

        string _UserName;
        public string UserName
        {
            get { return _UserName; }
        }

        public void SaveUserName(string username)
        {
            _UserName = username;
        }

        string _Token;
        public string Token
        {
            get { return _Token; }
        }

        public async void SaveToken(string token)
        {
            _Token = token;
            if (!string.IsNullOrEmpty(token))
            {
                // broadcast a message that authentication was successful
                MessagingCenter.Send<App>(this, "Authenticated");
            }
            else
            {
                MessagingCenter.Send<App>(this, "UnAuthenticated");
            }
        }

        public Action SuccessfulLoginAction
        {
            get
            {
                WhereUAtNinjaAPI.getInstance().updateHttpClientToken();
                return new Action(async () => await MainNav.Navigation.PopAsync());
            }
        }

        public BaseContentPage MainNav { get; set; }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
