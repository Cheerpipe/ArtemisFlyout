using System.Threading.Tasks;

namespace ArtemisFlyout.Services.FlyoutServices
{
    public interface IFlyoutService
    {
        void Show(bool animate = true);
        Task CloseAndRelease(bool animate = true);
        void SetHeight(double newHeight);
        void SetWidth(double newWidth);
        void Preload();
    }
}
