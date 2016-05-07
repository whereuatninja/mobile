using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhereUAt.Ninja.Mobile
{
    class Settings
    {
        public bool IsLocationTrackerOn { get; set; }
        public int LocationTimeIntervalInMilliseconds { get; set; }
        public int LocationTimeOutInMilliseconds { get; set; }

        public Settings(
            bool isLocationTrackerOn = true, 
            int locationTimeIntervalInMilliseconds = 60000, 
            int locationTimeOutInMilliseconds = 10000)
        {
            IsLocationTrackerOn = isLocationTrackerOn;
            LocationTimeIntervalInMilliseconds = locationTimeIntervalInMilliseconds;
            LocationTimeOutInMilliseconds = locationTimeOutInMilliseconds;
        }

    }
}
