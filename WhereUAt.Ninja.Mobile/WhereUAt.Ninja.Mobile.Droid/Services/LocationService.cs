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
using System.Threading;
using System.Threading.Tasks;
using Android.Util;

namespace WhereUAt.Ninja.Mobile.Droid.Services
{
    [Service]
    public class LocationService : Service
    {
        CancellationTokenSource _cts;

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            Log.Debug("LocationService", "OnStartCommand");
            _cts = new CancellationTokenSource();
            
            Task.Run(() => {
                try
                {
                    LocationTimerService locationService = new LocationTimerService();
                    locationService.startService(_cts.Token).Wait();
                }
                catch (Android.OS.OperationCanceledException)
                {
                }
                finally
                {
                    if (_cts.IsCancellationRequested)
                    {
                        //TODO
                        //var message = new CancelledMessage();
                        //Device.BeginInvokeOnMainThread(
                        //    () => MessagingCenter.Send(message, "CancelledMessage")
                        //);
                    }
                }

            }, _cts.Token);

            return StartCommandResult.Sticky;
        }

        private void timerElapsed()
        {
            Log.Debug("LocationService", "Hello from SimpleService. {0}", DateTime.UtcNow);
        }

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override void OnDestroy()
        {
            if (_cts != null)
            {
                _cts.Token.ThrowIfCancellationRequested();

                _cts.Cancel();
            }
            base.OnDestroy();
        }
    }
}