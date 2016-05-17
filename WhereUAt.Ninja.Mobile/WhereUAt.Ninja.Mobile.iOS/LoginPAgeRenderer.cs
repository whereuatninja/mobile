using Auth0.SDK;
using System;
using System.Collections.Generic;
using System.Text;
using WhereUAt.Ninja.Mobile;
using WhereUAt.Ninja.Mobile.iOS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(LoginPage), typeof(LoginPageRenderer))]
namespace WhereUAt.Ninja.Mobile.iOS
{
    public class LoginPageRenderer: PageRenderer
    {
        bool IsShown;

        public override async void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);


            if (!IsShown)
            {
                IsShown = true;

                var auth0 = new Auth0Client(
                    "whereuat.auth0.com",
                    "F2wSS3rEHorHyW3C9ezB2NnEAClryjcI");

                try
                {
                    var user = await auth0.LoginAsync(this);
                    App.Instance.SaveToken(user.Profile["identities"][0]["access_token"].ToString());
                    App.Instance.SuccessfulLoginAction.Invoke();
                }
                catch(Exception ex)
                {
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
}

