using ArtemisFlyout.ViewModels;
using ArtemisFlyout.Views;
using Ninject;

namespace ArtemisFlyout.Services.FlyoutServices
{
    public class FlyoutService : IFlyoutService
    {
        public MainWindow MainWindowInstance { get; set; }
        private readonly IKernel _kernel;

        public FlyoutService(IKernel kernel)
        {
            _kernel = kernel;
        }

        public void Show()
        {
            lock (this)
            {
                if (MainWindowInstance is {IsVisible: true}) return;

                MainWindowInstance = new MainWindow();
                MainWindowInstance.ViewModel = _kernel.Get<MainWindowViewModel>();
                MainWindowInstance.ShowAnimated();
            }
        }

        public void Close()
        {
            lock (this)
            {
                MainWindowInstance.CloseAnimated();
                MainWindowInstance = null;
            }
        }
    }
}
