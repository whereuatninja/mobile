using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Diagnostics;

namespace WhereUAt.Ninja.Mobile
{
    class WhereUAtNinjaAPI
    {
        public static WhereUAtNinjaAPI instance;
        private HttpClient httpClient;

        public static WhereUAtNinjaAPI getInstance()
        {
            if (instance == null)
            {
                instance = new WhereUAtNinjaAPI();
            }
            return instance;
        }

        private WhereUAtNinjaAPI()
        {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<bool> sendLocation(double longitude, double latitude, long time)
        {
            JObject json = new JObject();
            json.Add("long", longitude);
            json.Add("lat", latitude);
            String body = JsonConvert.SerializeObject(json);
            Debug.WriteLine("WhereUAtNinjaAPI.sendLocation before post: " + body);
            HttpResponseMessage response = await httpClient.PostAsync("http://192.168.1.7:3000/locations", new StringContent(body, Encoding.UTF8, "application/json"));
            Debug.WriteLine("WhereUAtNinjaAPI.sendLocation after post");
            String responseBody = await response.Content.ReadAsStringAsync();
            Debug.WriteLine("WhereUAtNinjaAPI.sendLocation responseBody:" + responseBody);

            bool status = Boolean.Parse(responseBody);
            return status;
        }
    }
}
