using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Diagnostics;
using ModernHttpClient;
using Xamarin.Forms;

namespace WhereUAt.Ninja.Mobile
{
    class WhereUAtNinjaAPI
    {
        public static WhereUAtNinjaAPI instance;
        private HttpClient httpClient;
        private Queue<IHttpRequest> storedRequestsQueue;
        private DateTime lastRequestSentDateTime;

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
            storedRequestsQueue = new Queue<IHttpRequest>();
            lastRequestSentDateTime = DateTime.MinValue;
        }

        private void setupHttpClient()
        {
            httpClient = new HttpClient(new NativeMessageHandler());
            httpClient.BaseAddress = new Uri("http://192.168.99.100/api/");
            //httpClient.BaseAddress = new Uri("http://dev.whereuat.ninja/api/");
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.Instance.Token);
        }

        public void updateHttpClientToken()
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.Instance.Token);
        }

        public async void sendLocation(Location location)
        {
            bool isSuccessful = false;
            Debug.WriteLine("API: sendLocation");
            bool areStoredRequestsSent = await sendStoredRequests();
            if (areStoredRequestsSent)
            {
                isSuccessful = await sendRequest(location);
            }

            if (!isSuccessful) { 
                this.storedRequestsQueue.Enqueue(location);
            }
            sendApiStatus();
        }

        public void sendApiStatus()
        {
            ApiStatus status = new ApiStatus(this.storedRequestsQueue.Count, this.lastRequestSentDateTime);
            MessagingCenter.Send(status, "ApiStatus");
        }

        private async Task<bool> sendRequest(IHttpRequest httpRequest)
        {
            Debug.WriteLine("API: sendRequest");
            bool isSuccessful = false;
            try
            {
                if (App.Instance.IsAuthenticated)
                {
                    Debug.WriteLine("API: IsAuthenticated");
                    HttpResponseMessage response = await httpClient.SendAsync(httpRequest.buildRequest());
                    Debug.WriteLine("API: before call to isRequestSuccessful");
                    isSuccessful = isRequestSuccessful(response).Result;
                    if (isSuccessful)
                    {
                        this.lastRequestSentDateTime = DateTime.Now;
                    }
                }
                
            }
            catch(Exception e)
            {
                Debug.WriteLine("Failed to send request:"+e.Message);
                Debug.WriteLine("Stack trace: " + e.StackTrace);
                isSuccessful = false;
            }
            
            return isSuccessful;
        }

        private async Task<bool> sendStoredRequests()
        {
            Debug.WriteLine("Failed Requests Queue Size: " + storedRequestsQueue.Count);
            while (storedRequestsQueue.Count > 0)
            {
                Debug.WriteLine("about to send a failed request");
                IHttpRequest storedRequest = storedRequestsQueue.Peek();
                bool isSuccessful = await sendRequest(storedRequest);
                if (!isSuccessful)
                {
                    return false;
                }
                else
                {
                    Debug.WriteLine("successfully sent a previously failed request");
                    storedRequestsQueue.Dequeue();
                }
            }
            return true;
        }

        private async Task<bool> isRequestSuccessful(HttpResponseMessage response)
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
