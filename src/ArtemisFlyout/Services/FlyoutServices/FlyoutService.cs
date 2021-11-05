using System;
using System.Threading.Tasks;
using ArtemisFlyout.IoC;
using ArtemisFlyout.Screens;
using ArtemisFlyout.UserControls;
using Avalonia.Controls;
using Ninject;
using Tmds.DBus;

namespace ArtemisFlyout.Services
{
    public class FlyoutService : IFlyoutService
    {
        public static FlyoutContainer FlyoutWindowInstance { get; private set; }
        private readonly IKernel _kernel;

        public FlyoutService(IKernel kernel)
        {
            _kernel = kernel;
        }

        public async void Show(bool animate = true)
        {
            if (FlyoutWindowInstance != null) return;
            FlyoutWindowInstance = GetInstance();

            FlyoutWindowInstance.Deactivated += (_, _) =>
            {
                _ = CloseAndRelease();
            };

            if (animate)
                await FlyoutWindowInstance.ShowAnimated();
            else
                FlyoutWindowInstance.Show();
        }

        public void SetHeight(double newHeight)
        {
            FlyoutWindowInstance.SetHeight(newHeight);
        }

        public void SetWidth(double newWidth)
        {
            FlyoutWindowInstance.SetWidth(newWidth);
        }

        private FlyoutContainer GetInstance()
        {
            FlyoutContainer flyoutInstance = _kernel.Get<FlyoutContainer>();
            try
            {
                flyoutInstance.DataContext = Kernel.Get<FlyoutContainerViewModel>();
            }
            catch (ConnectException)
            {
                flyoutInstance.DataContext = Kernel.Get<ArtemisLauncherViewModel>();
            }

            return flyoutInstance;
        }

        public async void Preload()
        {

            if (FlyoutWindowInstance != null) return;
            FlyoutWindowInstance = GetInstance();
            FlyoutWindowInstance.WindowState = WindowState.Minimized;
            await FlyoutWindowInstance.ShowAnimated();
            await CloseAndRelease(false);
        }

        public async Task CloseAndRelease(bool animate = true)
        {
            if (animate)
                await FlyoutWindowInstance.CloseAnimated();
            else
                FlyoutWindowInstance.Close();

            FlyoutWindowInstance = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
    }
}
