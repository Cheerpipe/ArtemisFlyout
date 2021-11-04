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
        public IRestResponse Post(string api, string content = "")
        {
            var client = new RestClient($"{_configurationService.GetConfiguration().RestSettings.Host}:{_configurationService.GetConfiguration().RestSettings.Port}");
            client.Timeout = 100;
            client.ReadWriteTimeout = 100;
            var request = new RestRequest(api, Method.POST);
            request.AddParameter("text/plain", content, ParameterType.RequestBody);
            return  client.Post(request);
        }

        public IRestResponse Get(string api, string content = "")
        {
            var client = new RestClient($"{_configurationService.GetConfiguration().RestSettings.Host}:{_configurationService.GetConfiguration().RestSettings.Port}");
            client.Timeout = 100;
            client.ReadWriteTimeout = 100;
            var request = new RestRequest(api, DataFormat.None);
            if (!string.IsNullOrEmpty(content))
                request.AddParameter("text/plain", content, ParameterType.RequestBody);
            return client.Get(request);
        }
    }
}
