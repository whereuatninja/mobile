﻿using Plugin.Geolocator.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhereUAt.Ninja.Mobile
{
    class LocationService
    {
        public static bool sendLocation(Position position)
        {
            Debug.WriteLine("LocationService.sendLocation");
            WhereUAtNinjaAPI api = WhereUAtNinjaAPI.getInstance();
            DateTime locationDate = DateTime.Now.ToUniversalTime();
            Task<bool> taskStatus = api.sendLocation(position.Longitude, position.Latitude, locationDate.Ticks);
            Debug.WriteLine("LocationService.sendLocation status:" + taskStatus.Result);
            return taskStatus.Result;
        }
    }
}
