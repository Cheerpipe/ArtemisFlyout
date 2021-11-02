
using ArtemisFlyout.ViewModels;

namespace ArtemisFlyout.DI.VMFactory
{
    public interface IVMFActory
    {
        public FlyoutContainerViewModel CreateMainWindowViewModel();
        public ArtemisDeviceTogglesViewModel CreateArtemisDeviceTogglesViewModel();
        public ArtemisMainControlViewModel CreateArtemisMainControlViewModel();
        public ArtemisLauncherViewModel CreateArtemisLauncherViewModel();
    }
}
