using RestSharp;

namespace ArtemisFlyout.Services.RestServices
{
    public interface IRestService
    {
        IRestResponse Post(string api, string content = "", string contentType = "");
        IRestResponse Get(string api, string content = "", string contentType = "");
        IRestResponse Put(string api, string content = "", string contentType = "");
    }
}
