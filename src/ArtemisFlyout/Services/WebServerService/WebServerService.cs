using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using EmbedIO;
using EmbedIO.WebApi;
using Newtonsoft.Json;

namespace ArtemisFlyout.Services
{
    public class WebServerService : IWebServerService
    {
        private readonly IConfigurationService _configurationService;
        private readonly List<WebApiControllerRegistration> _controllers = new();
        public WebServer Server { get; private set; }

        public WebServerService(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        public void Start()
        {
            Server = CreateWebServer();
            Server.Start();
        }

        public void Stop()
        {
            Server?.Dispose();
            Server = null;
        }

        public void AddController<T>() where T : WebApiController
        {
            _controllers.Add(new WebApiControllerRegistration<T>());
            Start();
        }

        private WebServer CreateWebServer()
        {
            Stop();

            WebApiModule webApiModule = new("/", JsonNetSerializer);
            WebServer server = new WebServer(o =>
                    o.WithUrlPrefix(
                            $"http://{_configurationService.GetConfiguration().RestApiSettings.ListeningAt}:{_configurationService.GetConfiguration().RestApiSettings.Port}/")
                        .WithMode(HttpListenerMode.EmbedIO))
                .WithLocalSessionManager()
                .WithModule(webApiModule);

            foreach (WebApiControllerRegistration controller in _controllers)
                webApiModule.RegisterController(controller.ControllerType, (Func<WebApiController>)controller.UntypedFactory);
            return server;
        }

        private async Task JsonNetSerializer(IHttpContext context, object data)
        {
            context.Response.ContentType = MimeType.Json;
            await using TextWriter writer = context.OpenResponseText();
            string json = JsonConvert.SerializeObject(data, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
            await writer.WriteAsync(json);
        }
    }
}
