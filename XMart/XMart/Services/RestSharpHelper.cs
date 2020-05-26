using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace XMart.Services
{
    public sealed class RestSharpHelper<T>
    {
        //private static readonly object padlock = new object();

        private static readonly Lazy<RestSharpHelper<T>> lazy = new Lazy<RestSharpHelper<T>>(() => new RestSharpHelper<T>());

        public static RestSharpHelper<T> Instance
        {
            get { return lazy.Value; }
        }

        private RestSharpHelper()
        {

        }

        static RestClient _restClient = new RestClient("http://47.114.101.147:7777");
        //static RestClient _restClient = new RestClient("http://120.26.3.153:7777");

        public static async Task<T> PostAsync(string url, string json)
        {
            var requestPost = new RestRequest(url, Method.POST);
            requestPost.AddParameter("application/json", json, ParameterType.RequestBody);
            IRestResponse<T> responsePost = await _restClient.ExecuteAsync<T>(requestPost);
            /*
            if (responsePost.ErrorException != null)
            {
                const string message = "Error retrieving response.  Check inner details for more info.";
                var twilioException = new ApplicationException(message, responsePost.ErrorException);
                throw twilioException;
            }
            */
            T t = JsonConvert.DeserializeObject<T>(responsePost.Content);
            return t;
        }

        public static async Task<T> GetAsync(string url)
        {
            var requestGet = new RestRequest(url, Method.GET);
            IRestResponse<T> responseGet = await _restClient.ExecuteAsync<T>(requestGet);
            /*
            if (responseGet.ErrorException != null)
            {
                const string message = "Error retrieving response.  Check inner details for more info.";
                var twilioException = new ApplicationException(message, responseGet.ErrorException);
                throw twilioException;
            }
            */
            T t = JsonConvert.DeserializeObject<T>(responseGet.Content);
            return t;
        }

        public static T Get(string url)
        {
            var requestGet = new RestRequest(url, Method.GET);
            IRestResponse responseGet = _restClient.Execute(requestGet);
            /*
            if (responseGet.ErrorException != null)
            {
                const string message = "Error retrieving response.  Check inner details for more info.";
                var twilioException = new ApplicationException(message, responseGet.ErrorException);
                throw twilioException;
            }
            */
            T t = JsonConvert.DeserializeObject<T>(responseGet.Content);
            return t;
        }

        public static T Post(string url, string json)
        {
            var requestPost = new RestRequest(url, Method.POST);
            requestPost.AddParameter("application/json", json, ParameterType.RequestBody);
            IRestResponse<T> responsePost = _restClient.Execute<T>(requestPost);
            /*
            if (responsePost.ErrorException != null)
            {
                const string message = "Error retrieving response.  Check inner details for more info.";
                var twilioException = new ApplicationException(message, responsePost.ErrorException);
                throw twilioException;
            }
            */
            T t = JsonConvert.DeserializeObject<T>(responsePost.Content);
            return t;
        }

        public static async Task<string> GetAsyncWithoutDeserialization(string url)
        {
            var requestGet = new RestRequest(url, Method.GET);
            IRestResponse responseGet = await _restClient.ExecuteAsync(requestGet);
            /*
            if (responseGet.ErrorException != null)
            {
                const string message = "Error retrieving response.  Check inner details for more info.";
                var twilioException = new ApplicationException(message, responseGet.ErrorException);
                throw twilioException;
            }
            */
            return responseGet.Content; 
        }

        public static async Task<string> PostAsyncWithoutDeserialization(string url, string json)
        {
            var requestPost = new RestRequest(url, Method.POST);
            requestPost.AddParameter("application/json", json, ParameterType.RequestBody);
            IRestResponse responsePost = await _restClient.ExecuteAsync(requestPost);
            /*
            if (responsePost.ErrorException != null)
            {
                const string message = "Error retrieving response.  Check inner details for more info.";
                var twilioException = new ApplicationException(message, responsePost.ErrorException);
                throw twilioException;
            }
            */
            return responsePost.Content;
        }

        public static string GetWithoutDeserialization(string url)
        {
            var requestGet = new RestRequest(url, Method.GET);
            IRestResponse responseGet = _restClient.Execute(requestGet);
            /*
            if (responseGet.ErrorException != null)
            {
                const string message = "Error retrieving response.  Check inner details for more info.";
                var twilioException = new ApplicationException(message, responseGet.ErrorException);
                throw twilioException;
            }
            */
            return responseGet.Content;
        }

        public static string PostWithoutDeserialization(string url, string json)
        {
            var requestPost = new RestRequest(url, Method.POST);
            requestPost.AddParameter("application/json", json, ParameterType.RequestBody);
            IRestResponse responsePost = _restClient.Execute(requestPost);
            /*
            if (responsePost.ErrorException != null)
            {
                const string message = "Error retrieving response.  Check inner details for more info.";
                var twilioException = new ApplicationException(message, responsePost.ErrorException);
                throw twilioException;
            }
            */
            return responsePost.Content;
        }

    }
}
