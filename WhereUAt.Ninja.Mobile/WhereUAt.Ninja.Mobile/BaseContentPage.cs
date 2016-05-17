using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace WhereUAt.Ninja.Mobile
{
    public class BaseContentPage: ContentPage
    {
        protected override async void OnAppearing()
        {
            base.OnAppearing();
        
            /*if (!App.Instance.IsAuthenticated)
            {
                App.Instance.MainNav = this;
                await Navigation.PushModalAsync(new LoginPage());
            }*/
        }
    }
}
