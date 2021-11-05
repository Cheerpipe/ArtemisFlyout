using EmbedIO.WebApi;

namespace ArtemisFlyout.Services
{
    public interface IWebServerService
    {
        public void Start();
        public void Stop();
        void AddController<T>() where T : WebApiController;
    }
}
