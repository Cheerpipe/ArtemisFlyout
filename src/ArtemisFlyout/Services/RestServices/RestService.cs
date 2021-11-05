using ArtemisFlyout.Services.Configuration;
using RestSharp;

namespace ArtemisFlyout.Services.RestServices
{
    public class RestService : IRestService
    {
        private readonly IConfigurationService _configurationService;
        public RestService(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        private RestClient GetClient()
        {
            return new(
                $"{_configurationService.GetConfiguration().RestSettings.Host}:{_configurationService.GetConfiguration().RestSettings.Port}")
            {
                Timeout = _configurationService.GetConfiguration().RestSettings.Timeout
            };
        }

        private RestRequest GetRequest(string api, string content = "", string contentType = "")
        {
            RestRequest request = new(api);
            request.AddParameter(string.IsNullOrEmpty(contentType) ? "text/plain" : contentType, content, ParameterType.RequestBody);
            return request;
        }

        public IRestResponse Post(string api, string content = "", string contentType = "")
        {
            RestClient client = GetClient();
            RestRequest request = GetRequest(api, content, contentType);
            return client.Post(request);
        }

        public IRestResponse Get(string api, string content = "", string contentType = "")
        {
            RestClient client = GetClient();
            RestRequest request = GetRequest(api, content, contentType);
            return client.Get(request);
        }

        public IRestResponse Put(string api, string content = "", string contentType = "")
        {
            RestClient client = GetClient();
            RestRequest request = GetRequest(api, content, contentType);
            return client.Put(request);
        }
    }
}
