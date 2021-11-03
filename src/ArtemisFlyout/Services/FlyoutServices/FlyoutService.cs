using System;
using ArtemisFlyout.ViewModels;
using ArtemisFlyout.Views;
using Ninject;

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

        public void Show()
        {
            lock (this)
            {
                if (FlyoutContainerInstance is { IsVisible: true }) return;
                FlyoutContainerInstance = _kernel.Get<FlyoutContainer>();
                FlyoutContainerInstance.ViewModel = _kernel.Get<FlyoutContainerViewModel>();
                FlyoutContainerInstance.Closed += (_, _) =>
                {
                    FlyoutContainerInstance = null;
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                };
                FlyoutContainerInstance.ShowAnimated();
            }
        }

        public void SetHeight(double newHeight)
        {
            FlyoutContainerInstance.SetHeight(newHeight);
        }

        public void SetWidth(double newWidth)
        {
            FlyoutContainerInstance.SetWidth(newWidth);
        }

        public void Preload()
        {
            lock (this)
            {
                FlyoutContainer container = new FlyoutContainer();
                container.ViewModel = _kernel.Get<FlyoutContainerViewModel>();
                container.Closed += (_, _) =>
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                };

                container.Opacity = 0;
                container.ShowAnimated();
                container.CloseAnimated();
            }
        }

        public void Close()
        {
            lock (this)
            {
                FlyoutContainerInstance?.Close();
                FlyoutContainerInstance = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
        }
    }
}
