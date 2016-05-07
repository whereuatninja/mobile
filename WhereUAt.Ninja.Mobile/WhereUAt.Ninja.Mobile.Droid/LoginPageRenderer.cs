using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms.Platform.Android;
using Auth0.SDK;
using Xamarin.Forms;
using WhereUAt.Ninja.Mobile;
using WhereUAt.Ninja.Mobile.Droid;

[assembly: ExportRenderer(typeof(LoginPage), typeof(LoginPageRenderer))]
namespace WhereUAt.Ninja.Mobile.Droid
{
    public class LoginPageRenderer : PageRenderer
    {
        protected override async void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);

            // this is a ViewGroup - so should be able to load an AXML file and FindView<>
            var activity = this.Context as Activity;

            var auth0 = new Auth0Client(
                "whereuat.auth0.com",
                "F2wSS3rEHorHyW3C9ezB2NnEAClryjcI");

            try
            {
                var user = await auth0.LoginAsync(this.Context);
                App.Instance.SaveToken(user.Profile["identities"][0]["access_token"].ToString());
                App.Instance.SuccessfulLoginAction.Invoke();
            }
            catch (Exception ex)
            {
                int x = 0;
            }

            
            /*
            - get user email => user.Profile["email"].ToString()
            - get facebook/google/twitter/etc access token => user.Profile["identities"][0]["access_token"]
            - get Windows Azure AD groups => user.Profile["groups"]
            - etc.
            */
        }
    }
}