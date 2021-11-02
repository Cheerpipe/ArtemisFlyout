using System;
using RestSharp;

namespace ArtemisFlyout.Artemis.Commands
{
    public class Command
    {
        private const string ENDPOINT_BASE = "/json-datamodel/";
        private RestClient _client = new RestClient("http://127.0.0.1:9696");

        public RestClient Client => _client;
        protected RestRequest CreateRequest(string api, string content, Method method)
        {
            RestRequest restRequest = new RestRequest(ENDPOINT_BASE + api, method);
            if (restRequest.Method != Method.GET)
                restRequest.AddParameter("application/json", content, ParameterType.RequestBody);
            return restRequest;
        }

        protected string ExecuteRequest(RestRequest restRequest)
        {
            try
            {
                //return Util.RemoveBom(_client.Execute(restRequest, restRequest.Method).Content.Trim());
                return _client.Execute(restRequest, restRequest.Method).Content.Trim();
            }
            catch (Exception ex)
            {
                //MessageBox.Show($"Error executing request {restRequest.Parameters[0]}.\r\nError: {ex}");
            }
            return string.Empty;
        }
    }
}
