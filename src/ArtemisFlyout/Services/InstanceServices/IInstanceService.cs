namespace ArtemisFlyout.Services
{
    public interface IInstanceService
    {
        bool IsAlreadyRunning();
        void ShowInstanceFlyout();
    }
}
