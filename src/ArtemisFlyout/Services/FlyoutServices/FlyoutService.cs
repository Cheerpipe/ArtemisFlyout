using System;
using System.Threading.Tasks;
using ArtemisFlyout.Screens;
using ArtemisFlyout.ViewModels;
using Ninject;

namespace ArtemisFlyout.Services
{
    public class FlyoutService : IFlyoutService
    {
        public static FlyoutContainer FlyoutWindowInstance { get; private set; }
        private readonly IKernel _kernel;
        private Func<ViewModelBase> _populateViewModelFunc;
        private bool _opening;
        private bool _closing;

        public FlyoutService(IKernel kernel)
        {
            _kernel = kernel;
        }

        public async void Show(bool animate = true)
        {
            if (_opening)
                return;
            _opening = true;

            if (FlyoutWindowInstance != null) return;
            FlyoutWindowInstance = CreateInstance();

            FlyoutWindowInstance.Deactivated += (_, _) =>
            {
                _ = CloseAndRelease();
            };

            if (animate)
                await FlyoutWindowInstance.ShowAnimated();
            else
                FlyoutWindowInstance.Show();

            FlyoutWindowInstance?.Activate();

            _opening = false;
        }

        public void SetHeight(double newHeight)
        {
            FlyoutWindowInstance?.SetHeight(newHeight + 1);
        }

        public void SetWidth(double newWidth)
        {
            FlyoutWindowInstance?.SetWidth(newWidth + 1);
        }

        //TODO: Move ViewModel creation outside the Service
        private FlyoutContainer CreateInstance()
        {
            if (_populateViewModelFunc == null)
                throw new Exception("PopulateViewModelFunc delegate must be seted using SetPopulateViewModelFunc() before using a Flyout");

            FlyoutContainer flyoutInstance = _kernel.Get<FlyoutContainer>();
            flyoutInstance.DataContext = _populateViewModelFunc();
            return flyoutInstance;
        }

        public async Task PreLoad()
        {
            if (FlyoutWindowInstance != null) return;
            FlyoutWindowInstance = CreateInstance();
            await FlyoutWindowInstance.ShowAnimated(true);
            await Task.Delay(500);
            await CloseAndRelease(false);
        }

        public async void Toggle()
        {
            if (FlyoutWindowInstance is null)
            {
                Show();
            }
            else
            {
                await CloseAndRelease();
            }
        }

        public void SetPopulateViewModelFunc(Func<ViewModelBase> populateViewModelFunc)
        {
            _populateViewModelFunc = populateViewModelFunc;
        }

        public async Task CloseAndRelease(bool animate = true)
        {
            if (_closing)
                return;

            _closing = true;

            if (animate)
                await FlyoutWindowInstance.CloseAnimated();
            else
                FlyoutWindowInstance.Close();

            FlyoutWindowInstance = null;

            _closing = false;

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
    }
}
