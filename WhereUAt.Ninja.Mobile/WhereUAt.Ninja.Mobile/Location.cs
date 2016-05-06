using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhereUAt.Ninja.Mobile
{
    public class Location
    {
        public double Longitude { get; private set; }
        public double Latitude { get; private set; }

        

        public Location(double longitude, double latitude)
        {
            this.Longitude = longitude;
            this.Latitude = latitude;
        }

    }
}
