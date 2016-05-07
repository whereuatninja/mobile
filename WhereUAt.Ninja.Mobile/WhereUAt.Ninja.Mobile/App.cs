using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace WhereUAt.Ninja.Mobile
{
    public class App : Application
    {
        static NavigationPage _NavPage; 
        public App()
        {
            var mainPage = new WhereUAt.Ninja.Mobile.MainPage();
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
            get { return !string.IsNullOrWhiteSpace(_Token); }
        }

        string _Token;
        public string Token
        {
            get { return _Token; }
        }

        public async void SaveToken(string token)
        {
            _Token = token;

            // broadcast a message that authentication was successful
            MessagingCenter.Send<App>(this, "Authenticated");
        }

        public Action SuccessfulLoginAction
        {
            get
            {
                return new Action(() => _NavPage.Navigation.PopModalAsync());
            }
        }

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
