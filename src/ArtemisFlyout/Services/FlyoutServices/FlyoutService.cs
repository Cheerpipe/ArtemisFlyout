using ArtemisFlyout.ViewModels;
using ArtemisFlyout.Views;
using Ninject;

namespace ArtemisFlyout.Services.FlyoutServices
{
    public class FlyoutService : IFlyoutService
    {
        public FlyoutContainer FlyoutContainerInstance { get; set; }
        private readonly IKernel _kernel;

        public FlyoutService(IKernel kernel)
        {
            _kernel = kernel;
        }

        public void Show()
        {
            lock (this)
            {
                if (FlyoutContainerInstance is {IsVisible: true}) return;

                FlyoutContainerInstance = new FlyoutContainer();
                FlyoutContainerInstance.ViewModel = _kernel.Get<FlyoutContainerViewModel>();
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

        public void Close()
        {
            lock (this)
            {
                FlyoutContainerInstance.CloseAnimated();
                FlyoutContainerInstance = null;
            }
        }
    }
}
