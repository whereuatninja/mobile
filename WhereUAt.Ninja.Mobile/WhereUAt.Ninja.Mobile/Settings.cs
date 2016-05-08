using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhereUAt.Ninja.Mobile
{
    class Settings
    {
        private static Settings instance;
        public bool IsLocationTrackerOn { get; set; }
        public int LocationTimeIntervalInMilliseconds { get; set; }
        public int LocationTimeOutInMilliseconds { get; set; }

        public static Settings getInstance(
            bool isLocationTrackerOn = true,
            int locationTimeIntervalInMilliseconds = 20000,
            int locationTimeOutInMilliseconds = 10000)
        {
            if(instance == null)
            {
                instance = new Settings(isLocationTrackerOn, locationTimeIntervalInMilliseconds, locationTimeOutInMilliseconds);
            }
            return instance;
        }

        private Settings(
            bool isLocationTrackerOn = true, 
            int locationTimeIntervalInMilliseconds = 20000, 
            int locationTimeOutInMilliseconds = 10000)
        {
            IsLocationTrackerOn = isLocationTrackerOn;
            LocationTimeIntervalInMilliseconds = locationTimeIntervalInMilliseconds;
            LocationTimeOutInMilliseconds = locationTimeOutInMilliseconds;
        }

    }
}
