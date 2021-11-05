using System.Text;
using System.Threading.Tasks;
using ArtemisFlyout.Services;
using Avalonia.Threading;
using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;

namespace ArtemisFlyout.Controllers
{
    public class FlyoutRestController : WebApiController
    {
        private readonly IFlyoutService _flyoutService;
        public FlyoutRestController(FlyoutService flyoutService)
        {
            _flyoutService = flyoutService;
        }

        [Route(HttpVerbs.Get, "/flyout/show")]
        public async Task ShowFlyout()
        {
            Dispatcher.UIThread.Post(()=> _flyoutService.Show());
            HttpContext.Response.ContentType = "application/json";
            await using var writer = HttpContext.OpenResponseText(new UTF8Encoding(false));
            await writer.WriteAsync(string.Empty);
        }

        [Route(HttpVerbs.Get, "/flyout/close")]
        public async Task CloseFlyout()
        {
            Dispatcher.UIThread.Post(() => _flyoutService.CloseAndRelease());
            HttpContext.Response.ContentType = "application/json";
            await using var writer = HttpContext.OpenResponseText(new UTF8Encoding(false));
            await writer.WriteAsync(string.Empty);
        }
    }
}
