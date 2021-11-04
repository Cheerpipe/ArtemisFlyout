using System;
using RestSharp;

namespace ArtemisFlyout.JsonDatamodel
{
    public class Command
    {
        private const string EndpointBase = "/json-datamodel/";
        private RestClient _client = new RestClient("http://127.0.0.1:9696");


        protected RestRequest CreateRequest(string api, string content, Method method)
        {
            RestRequest restRequest = new RestRequest(EndpointBase + api, method);
            restRequest.Timeout = 100;
            if (restRequest.Method != Method.GET)
                restRequest.AddParameter("application/json", content, ParameterType.RequestBody);
            return restRequest;
        }

        protected string ExecuteRequest(RestRequest restRequest)
        {
            return _client.Execute(restRequest, restRequest.Method).Content.Trim();
        }
    }
}
