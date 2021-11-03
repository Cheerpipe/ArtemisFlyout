namespace ArtemisFlyout.Services.FlyoutServices
{
    public interface IFlyoutService
    {
        void Show();
        void Close();
        void SetHeight(double newHeight);
        void SetWidth(double newWidth);

    }
}
