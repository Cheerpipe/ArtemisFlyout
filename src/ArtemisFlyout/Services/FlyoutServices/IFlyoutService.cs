using System.Threading.Tasks;

namespace ArtemisFlyout.Services.FlyoutServices
{
    public interface IFlyoutService
    {
        void Show();
        Task Close();
        void SetHeight(double newHeight);
        void SetWidth(double newWidth);
        void Preload();
    }
}
