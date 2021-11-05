using ArtemisFlyout.ViewModels;

namespace ArtemisFlyout.IoC
{
    public class ViewModelLocator
    {
        public ArtemisDeviceTogglesViewModel ArtemisDeviceTogglesViewModel => Kernel.Get<ArtemisDeviceTogglesViewModel>();
        public ArtemisLauncherViewModel ArtemisLauncherViewModel => Kernel.Get<ArtemisLauncherViewModel>();
        public ArtemisMainControlViewModel ArtemisMainControlViewModel => Kernel.Get<ArtemisMainControlViewModel>();
        public FlyoutContainerViewModel FlyoutContainerViewModel => Kernel.Get<FlyoutContainerViewModel>();
    }
}
