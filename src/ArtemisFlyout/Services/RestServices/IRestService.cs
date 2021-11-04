using RestSharp;

namespace ArtemisFlyout.Services.RestServices
{
    public interface IRestService
    {
        IRestResponse Post(string api, string content = "");
        IRestResponse Get(string api, string content = "");
    }
}
