
using System;
using System.ComponentModel;
using System.Diagnostics;
using Xamarin.Forms;

namespace WhereUAt.Ninja.Mobile
{
    class StatusPageViewModel : INotifyPropertyChanged
    {
        public int storedRequestsCount;
        public DateTime lastSentDateTime;
        public event PropertyChangedEventHandler PropertyChanged;
        

        public StatusPageViewModel()
        {
            MessagingCenter.Subscribe<ApiStatus>(this, "ApiStatus", (status) => {
                updateFromStatus(status);
                Debug.WriteLine("Status Page: Stored Requests Count {0}, Last time sent: {1}", status.StoredRequestsCount, status.LastSentDateTime);
            });
        }

        private void updateFromStatus(ApiStatus status)
        {
            StoredRequestsCount = status.StoredRequestsCount;
            LastSentDateTime = status.LastSentDateTime;
        }

        public int StoredRequestsCount
        {
            set
            {
                if (storedRequestsCount != value)
                {
                    storedRequestsCount = value;
                    OnPropertyChanged("StoredRequestsCount");
                }
            }
            get
            {
                return storedRequestsCount;
            }
        }

        public DateTime LastSentDateTime
        {
            set
            {
                if (lastSentDateTime != value)
                {
                    lastSentDateTime = value;
                    OnPropertyChanged("LastSentDateTime");
                }
            }
            get
            {
                return lastSentDateTime;
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
