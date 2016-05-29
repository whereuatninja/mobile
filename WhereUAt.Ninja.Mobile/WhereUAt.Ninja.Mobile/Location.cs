using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace WhereUAt.Ninja.Mobile
{
    public class Location : IHttpRequest
    {
        public double Longitude { get; private set; }
        public double Latitude { get; private set; }
        public long Time { get; private set; }
        public String Message { get; private set; }

        public Location(double longitude, double latitude, long time, String message)
        {
            this.Longitude = longitude;
            this.Latitude = latitude;
            this.Time = time;
            this.Message = message;
        }

        public HttpRequestMessage buildRequest()
        {
            String body = getJson();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "locations");
            StringContent content = new StringContent(body);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            request.Content = content;
            return request;
        }

        private String getJson()
        {
            JObject json = new JObject();
            json.Add("long", this.Longitude);
            json.Add("lat", this.Latitude);
            json.Add("time", this.Time);
            json.Add("message", this.Message);
            return JsonConvert.SerializeObject(json);
        }
    }
}
