using System.Threading.Tasks;

namespace ArtemisFlyout.Services
{
    public interface IFlyoutService
    {
        void Show(bool animate = true);
        Task CloseAndRelease(bool animate = true);
        void SetHeight(double newHeight);
        void SetWidth(double newWidth);
        Task Preload();
        void Toggle();
    }
}
