using System.Threading.Tasks;

namespace ArtemisFlyout.Services.RestServices
{
    public interface IRestService
    {
        string Post(string api, string content = "");
        string Get(string api, string content = "");
    }
}
