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
        public string Post(string api, string content = "")
        {
            var client = new RestClient($"{_configurationService.GetConfiguration().RestSettings.Host}:{_configurationService.GetConfiguration().RestSettings.Port}");
            var request = new RestRequest(api, Method.POST);
            request.AddParameter("text/plain", content, ParameterType.RequestBody);
            return  client.Post(request).Content;
        }

        public string Get(string api, string content = "")
        {
            var client = new RestClient($"{_configurationService.GetConfiguration().RestSettings.Host}:{_configurationService.GetConfiguration().RestSettings.Port}");
            var request = new RestRequest(api, DataFormat.None);
            if (!string.IsNullOrEmpty(content))
                request.AddParameter("text/plain", content, ParameterType.RequestBody);
            return client.Get(request).Content;
        }
    }
}
