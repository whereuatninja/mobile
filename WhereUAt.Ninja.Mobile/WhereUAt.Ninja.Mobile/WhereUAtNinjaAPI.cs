using System;
using System.Collections.Generic;using System.Linq;using System.Text;
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
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.Instance.Token);
        }

        public async Task<bool> sendLocation(double longitude, double latitude, long time)
        {
            JObject json = new JObject();
            json.Add("long", longitude);
            json.Add("lat", latitude);
            String body = JsonConvert.SerializeObject(json);
            Debug.WriteLine("WhereUAtNinjaAPI.sendLocation before post: " + body);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "http://192.168.99.100/api/locations");
            StringContent content = new StringContent(body);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            request.Content = content;
            HttpResponseMessage response = await httpClient.SendAsync(request);
            Debug.WriteLine("WhereUAtNinjaAPI.sendLocation after post");
            String responseBody = await response.Content.ReadAsStringAsync();
            Debug.WriteLine("WhereUAtNinjaAPI.sendLocation responseBody:" + responseBody);

            bool status = Boolean.Parse(responseBody);
            return status;
        }
    }
}
