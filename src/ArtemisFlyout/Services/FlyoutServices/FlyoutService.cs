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
        private bool _animating;

        public FlyoutService(IKernel kernel)
        {
            _kernel = kernel;
        }

        public void Show()
        {
            if (FlyoutContainerInstance != null) return;

            FlyoutContainerInstance = _kernel.Get<FlyoutContainer>();
            FlyoutContainerInstance.ViewModel = _kernel.Get<FlyoutContainerViewModel>();
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

        public void Preload()
        {
            Show();
            FlyoutContainerInstance.Opacity = 0;
            Close();
        }

        public async void Close()
        {
   
            await FlyoutContainerInstance.CloseAnimated();
            FlyoutContainerInstance = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

        }
    }
}
