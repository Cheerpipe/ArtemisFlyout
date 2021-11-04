namespace ArtemisFlyout.Services.LauncherServices
{
    public interface ILauncherService
    {
        public bool IsArtemisRunning();
        public bool IsArtemisListening();
        public void Launch();
    }
}