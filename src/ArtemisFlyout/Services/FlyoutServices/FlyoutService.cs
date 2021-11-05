using System;
using System.Threading.Tasks;
using ArtemisFlyout.IoC;
using ArtemisFlyout.Screens;
using ArtemisFlyout.UserControls;
using ArtemisFlyout.ViewModels;
using Ninject;
using Tmds.DBus;

namespace ArtemisFlyout.Services.FlyoutServices
{
    public class FlyoutService : IFlyoutService
    {
        public static FlyoutContainer FlyoutContainerInstance { get; private set; }
        private readonly IKernel _kernel;

        public FlyoutService(IKernel kernel)
        {
            _kernel = kernel;
        }

        public async void Show()
        {
            if (FlyoutContainerInstance != null) return;

            FlyoutContainerInstance = _kernel.Get<FlyoutContainer>();

            try
            {
                FlyoutContainerInstance.DataContext = Kernel.Get<FlyoutContainerViewModel>();
            }
            catch (ConnectException)
            {
                FlyoutContainerInstance.DataContext = Kernel.Get<ArtemisLauncherViewModel>();
            }

            await FlyoutContainerInstance.ShowAnimated();
        }

        public void SetHeight(double newHeight)
        {
            FlyoutContainerInstance.SetHeight(newHeight);
        }

        public void SetWidth(double newWidth)
        {
            FlyoutContainerInstance.SetWidth(newWidth);
        }

        public async void Preload()
        {
            Show();
            FlyoutContainerInstance.Opacity = 0;
            await Close();
        }

        public async Task Close()
        {
            await FlyoutContainerInstance.CloseAnimated();
            FlyoutContainerInstance = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

        }
    }
}
