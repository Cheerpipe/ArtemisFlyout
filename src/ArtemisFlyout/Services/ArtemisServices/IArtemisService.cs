namespace ArtemisFlyout.Services
{
    public interface IArtemisService
    {
        void GoHome();
        void GoWorkshop();
        void GoSurfaceEditor();
        void ShowDebugger();
        void GoSettings();
        void Run();
        void Restart();
    }
}