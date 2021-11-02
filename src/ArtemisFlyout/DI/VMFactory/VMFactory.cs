using System;
using ArtemisFlyout.ViewModels;

namespace ArtemisFlyout.DI.VMFactory
{
    public class VMFactory : IVMFActory
    {
        public FlyoutContainerViewModel CreateMainWindowViewModel()
        {
            throw new NotImplementedException();
        }

        public ArtemisDeviceTogglesViewModel CreateArtemisDeviceTogglesViewModel()
        {
            throw new NotImplementedException();
        }

        public ArtemisMainControlViewModel CreateArtemisMainControlViewModel()
        {
            throw new NotImplementedException();
        }

        public ArtemisLauncherViewModel CreateArtemisLauncherViewModel()
        {
            throw new NotImplementedException();
        }
    }
}
