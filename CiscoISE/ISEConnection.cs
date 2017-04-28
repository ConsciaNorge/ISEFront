using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CiscoISE
{
    public class ISEConnection
    {
        public Uri BaseURL { get; set; }
        public string SDKPath { get; set; } = "/ers";
        public string Username { get; set; }
        public string Password { get; set; }

        private Uri baseAddress
        {
            get
            {
                //return new Uri(BaseURL, SDKPath);
                return BaseURL;
            }
        }

        private string BasicAuthenticationString
        {
            get
            {
                return Convert.ToBase64String(Encoding.UTF8.GetBytes(Username + ":" + Password));
            }
        }

        public ISEConnection(Uri baseUrl, string username, string password)
        {
            BaseURL = baseUrl;
            Username = username;
            Password = password;
        }

        private HttpClient GetHttpClientObject()
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

            var clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

            var client = new HttpClient(clientHandler);
            client.BaseAddress = baseAddress;
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Basic",
                    BasicAuthenticationString
                    );

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json")
                );

            return client;
        }

        public async Task<HttpResponseMessage> RestGet(string relativeUrl)
        {
            var response = await GetHttpClientObject().GetAsync("/ers/" + relativeUrl);  

            return response;
        }

        public async Task<HttpResponseMessage> RestPut(string relativeUrl, object data=null)
        {
            var client = GetHttpClientObject();
            var response = await client.PutAsJsonAsync(
                "/ers/" + relativeUrl, 
                data == null ?
                new StringContent("", Encoding.UTF8, "application/json") :
                data);

            return response;
        }

        public async Task<bool> RestPut(string relativeUrl, HttpStatusCode successCode, object data=null)
        {
            var putResult = await RestPut(relativeUrl, data)
                .ContinueWith(task =>
                {
                    var result = task.Result;

                    if (result.StatusCode == successCode)
                    {
                        return true;
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Fail to put : " + result.ReasonPhrase);
                        return false;
                    }
                });

            return putResult;
        }

        public async Task<HttpResponseMessage> RestPost(string relativeUrl, object data = null)
        {
            string json = "";
            if (data != null)
            {
                json = JsonConvert.SerializeObject(
                    data,
                    new IsoDateTimeConverter { DateTimeFormat = "MM/dd/yyyy hh:mm" }
                    );
            }

            var client = GetHttpClientObject();
            var response = await client.PostAsync(
                "/ers/" + relativeUrl,
                new StringContent(json, Encoding.UTF8, "application/json"));

            return response;
        }

        public async Task<bool> RestPost(string relativeUrl, HttpStatusCode successCode, object data = null)
        {
            var postResult = await RestPost(relativeUrl, data)
                .ContinueWith(task =>
                {
                    var response = task.Result;
                    if (response.StatusCode == successCode)
                    {
                        return true;
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Fail to post : " + response.ReasonPhrase);
                        return false;
                    }
                });

            return postResult;
        }

        public async Task<HttpResponseMessage> RestDelete(string relativeUrl)
        {
            var client = GetHttpClientObject();
            var response = await client.DeleteAsync(
                "/ers/" + relativeUrl);

            return response;
        }

        public async Task<bool> RestDelete(string relativeUrl, HttpStatusCode successCode)
        {
            var postResult = await RestDelete(relativeUrl)
                .ContinueWith(task =>
                {
                    var response = task.Result;
                    if (response.StatusCode == successCode)
                    {
                        return true;
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Fail to delete : " + response.ReasonPhrase);
                        return false;
                    }
                });

            return postResult;
        }
    }
}