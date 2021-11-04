using System;
using System.Threading.Tasks;
using ArtemisFlyout.Services.LauncherServices;
using ArtemisFlyout.ViewModels;
using ArtemisFlyout.Views;
using Ninject;

namespace ArtemisFlyout.Services.FlyoutServices
{
    public class FlyoutService : IFlyoutService
    {
        public static FlyoutContainer FlyoutContainerInstance { get; private set; }
        public readonly ILauncherService _launcherService;
        private readonly IKernel _kernel;
        private bool _animating;

        public FlyoutService(IKernel kernel, ILauncherService launcherService)
        {
            _launcherService = launcherService;
            _kernel = kernel;
        }

        public void Show()
        {
            if (FlyoutContainerInstance != null) return;

            FlyoutContainerInstance = _kernel.Get<FlyoutContainer>();
            if (_launcherService.IsArtemisRunning())
                FlyoutContainerInstance.ViewModel = _kernel.Get<FlyoutContainerViewModel>();
            else
                FlyoutContainerInstance.DataContext = _kernel.Get<ArtemisLauncherViewModel>();
            FlyoutContainerInstance.ShowAnimated();
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
