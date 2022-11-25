using RestSharp;

namespace ArtemisFlyout.Services
{
    public class RestService : IRestService
    {
        private readonly IConfigurationService _configurationService;
        public RestService(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        private RestClient GetArtemisClient()
        {
            return new(
                $"{_configurationService.Get().RestClientSettings.Host}:{_configurationService.Get().RestClientSettings.Port}")
            {
                Timeout = _configurationService.Get().RestClientSettings.Timeout
            };
        }

        private static RestRequest GetRequest(string api, string content = "", string contentType = "")
        {
            RestRequest request = new(api);
            request.AddParameter(string.IsNullOrEmpty(contentType) ? "text/plain" : contentType, content, ParameterType.RequestBody);
            return request;
        }

        public IRestResponse Post(string api, string content = "", string contentType = "")
        {
            RestClient client = GetArtemisClient();
            RestRequest request = GetRequest(api, content, contentType);
            return client.Post(request);
        }

        public IRestResponse Get(string api, string content = "", string contentType = "")
        {
            RestClient client = GetArtemisClient();
            RestRequest request = GetRequest(api, content, contentType);
            return client.Get(request);
        }

        public IRestResponse Get(string endPoint, int port, string api, string content = "", string contentType = "")
        {
            RestClient client = new($"{endPoint}:{port}")
            {
                Timeout = _configurationService.Get().RestClientSettings.Timeout
            };

            RestRequest request = GetRequest(api, content, contentType);
            return client.Get(request);
        }

        public IRestResponse Put(string api, string content = "", string contentType = "")
        {
            RestClient client = GetArtemisClient();
            RestRequest request = GetRequest(api, content, contentType);
            return client.Put(request);
        }
    }
}
