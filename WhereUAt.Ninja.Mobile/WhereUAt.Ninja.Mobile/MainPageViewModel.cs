using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace WhereUAt.Ninja.Mobile
{
    class MainPageViewModel : INotifyPropertyChanged
    {
        public String loggedInUserStatusLabelText;
        public event PropertyChangedEventHandler PropertyChanged;

        public MainPageViewModel()
        {
            setupSubscribers();
        }

        private void setupSubscribers()
        {
            MessagingCenter.Subscribe<App>(this, "Authenticated", message => {
                updateFromMessage(true);
            });

            MessagingCenter.Subscribe<App>(this, "UnAuthenticated", message => {
                updateFromMessage(false);
            });
        }

        private void updateFromMessage(bool isAuthenticated)
        {
            if (isAuthenticated)
            {
                String name = App.Instance.UserName;
                LoggedInUserStatusLabelText = "Hello " + name + "!";
            }
            else
            {
                LoggedInUserStatusLabelText = "Expired Token!! Log in again!";
            }
        }


        public String LoggedInUserStatusLabelText
        {
            set
            {
                if (loggedInUserStatusLabelText != value)
                {
                    loggedInUserStatusLabelText = value;
                    OnPropertyChanged("LoggedInUserStatusLabelText");
                }
            }
            get
            {
                return loggedInUserStatusLabelText;
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this,
                    new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
