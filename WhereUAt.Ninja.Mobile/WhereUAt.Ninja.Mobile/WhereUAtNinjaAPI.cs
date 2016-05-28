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
using System.Threading;
using ModernHttpClient;

namespace WhereUAt.Ninja.Mobile
{
    class WhereUAtNinjaAPI
    {
        public static WhereUAtNinjaAPI instance;
        private HttpClient httpClient;
        private Queue<Location> failedRequestsQueue;

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
            setupHttpClient();
            failedRequestsQueue = new Queue<Location>();
        }

        private void setupHttpClient()
        {
            httpClient = new HttpClient(new NativeMessageHandler());
            httpClient.BaseAddress = new Uri("http://192.168.99.100/api/");
            //httpClient.BaseAddress = new Uri("http://dev.whereuat.ninja/api/");
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.Instance.Token);
        }

        public async void sendLocation(Location location)
        {
            bool areFailedRequestsSent = sendPreviouslyFailedRequests();
            bool isSuccessful = false;
            if (areFailedRequestsSent)
            {
                HttpRequestMessage request = buildRequestFromLocation(location);
                isSuccessful = await sendRequest(request);
                storeFailedRequest(location, isSuccessful);
            }
            else
            {
                storeFailedRequest(location, false);
            }
        }

        private async Task<bool> sendRequest(HttpRequestMessage request)
        {
            bool isSuccessful = false;
            try
            {
                HttpResponseMessage response = await httpClient.SendAsync(request);
                isSuccessful = isSendLocationSuccessful(response).Result;
            }
            catch(Exception e)
            {
                Debug.WriteLine("Failed to send request:"+e.Message);
                Debug.WriteLine("Stack trace: " + e.StackTrace);
                isSuccessful = false;
            }
            
            return isSuccessful;
        }

        private bool sendPreviouslyFailedRequests()
        {
            Debug.WriteLine("Failed Requests Queue Size: " + failedRequestsQueue.Count);
            while (failedRequestsQueue.Count > 0)
            {
                Debug.WriteLine("about to send a failed request");
                Location failedLocation = failedRequestsQueue.Peek();
                HttpRequestMessage newRequest = buildRequestFromLocation(failedLocation);
                Task<bool> isSuccessful = sendRequest(newRequest);
                if (!isSuccessful.Result)
                {
                    return false;
                }
                else
                {
                    Debug.WriteLine("successfully sent a previously failed request");
                    failedRequestsQueue.Dequeue();
                }
            }
            return true;
        }

        private void storeFailedRequest(Location location, bool isSuccessful)
        {
            if (!isSuccessful)
            {
                failedRequestsQueue.Enqueue(location);
            }
        }

        private HttpRequestMessage buildRequestFromLocation(Location location)
        {
            String body = buildLocationJson(location);
            Debug.WriteLine("Content: " + body);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "locations");
            Debug.WriteLine("request uri: {0}", request.RequestUri);
            //HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "http://dev.whereuat.ninja/api/locations");
            StringContent content = new StringContent(body);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            request.Content = content;
            return request;
        }

        private String buildLocationJson(Location location)
        {
            JObject json = new JObject();
            json.Add("long", location.Longitude);
            json.Add("lat", location.Latitude);
            json.Add("time", location.Time);
            json.Add("message", location.Message);
            String serializedJson = JsonConvert.SerializeObject(json);
            return serializedJson;
        }

        private async Task<bool> isSendLocationSuccessful(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine("WhereUAtNinjaAPI.isSendLocationSuccessful statuscode is successful.");
                String responseBody = await response.Content.ReadAsStringAsync();
                Debug.WriteLine("WhereUAtNinjaAPI.isSendLocationSuccessful responseBody:" + responseBody);
                bool status = Boolean.Parse(responseBody);
                return status;
            }
            else
            {
                String responseContent = await response.Content.ReadAsStringAsync();
                Debug.WriteLine("WhereUAtNinjaAPI.isSendLocationSuccessful statuscode is FAILED!!! Status Code: {0}, ReasonPhrase: {1}, Content: {2}",response.StatusCode, response.ReasonPhrase, responseContent);
                
                return false;
            }
        }
    }
}
