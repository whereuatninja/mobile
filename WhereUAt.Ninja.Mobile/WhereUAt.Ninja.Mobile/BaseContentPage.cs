using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace WhereUAt.Ninja.Mobile
{
    public class BaseContentPage: ContentPage
    {
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (!App.Instance.IsAuthenticated)
            {
                Navigation.PushModalAsync(new LoginPage());
            }
        }
    }
}
