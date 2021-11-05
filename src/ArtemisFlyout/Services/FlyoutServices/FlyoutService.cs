using System;
using System.Threading.Tasks;
using ArtemisFlyout.IoC;
using ArtemisFlyout.Screens;
using Avalonia.Controls;
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
                // Don't populate any viewmodel because it can be a dummy form for pre loading
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
            FlyoutContainerInstance = _kernel.Get<FlyoutContainer>();
            try
            {
                FlyoutContainerInstance.DataContext = Kernel.Get<FlyoutContainerViewModel>();
            }
            catch (ConnectException)
            {
                // Don't populate any viewmodel because it can be a dummy form for pre loading
            }

            FlyoutContainerInstance.WindowState = WindowState.Minimized;
            await FlyoutContainerInstance.ShowAnimated();
            FlyoutContainerInstance.Close();
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
