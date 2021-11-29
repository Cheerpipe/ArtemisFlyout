using System;
using System.Threading.Tasks;
using ArtemisFlyout.IoC;
using ArtemisFlyout.Pages;
using ArtemisFlyout.Screens;
using Ninject;

namespace ArtemisFlyout.Services
{
    public class FlyoutService : IFlyoutService
    {
        public static FlyoutContainer FlyoutWindowInstance { get; private set; }
        private readonly IConfigurationService _configurationService;
        private readonly IArtemisService _artemisService;
        private readonly IKernel _kernel;
        private bool _opening;
        private bool _closing;

        public FlyoutService(IKernel kernel, IConfigurationService configurationService, IArtemisService artemisService)
        {
            _configurationService = configurationService;
            _artemisService = artemisService;
            _kernel = kernel;
        }

        public async void Show(bool animate = true)
        {
            if (_opening)
                return;
            _opening = true;

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
            _opening = false;
        }

        public void SetHeight(double newHeight)
        {
            FlyoutWindowInstance?.SetHeight(newHeight+1);
        }

        public void SetWidth(double newWidth)
        {
            FlyoutWindowInstance?.SetWidth(newWidth+1);
        }

        //TODO: Move ViewModel creation outside the Service
        private FlyoutContainer GetInstance()
        {

            FlyoutContainer flyoutInstance = _kernel.Get<FlyoutContainer>();
            if (!_artemisService.IsRunning())
            {
                flyoutInstance.DataContext = Kernel.Get<ArtemisLauncherViewModel>();
            }
            else if (!_artemisService.CheckJsonDatamodelPluginPlugin().VersionIsOk || !_artemisService.CheckExtendedApiRestPlugin().VersionIsOk)

            {
                flyoutInstance.DataContext = Kernel.Get<ArtemisPluginPrerequisitesViewModel>();
            }
            else
            {
                _configurationService.Load();
                flyoutInstance.DataContext = Kernel.Get<FlyoutContainerViewModel>();
            }

            return flyoutInstance;
        }

        public async Task PreLoad()
        {
            if (FlyoutWindowInstance != null) return;
            FlyoutWindowInstance = GetInstance();
            await FlyoutWindowInstance?.ShowAnimated(true)!;
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

        public async Task CloseAndRelease(bool animate = true)
        {
            if (_closing)
                return;
            _closing = true;

            if (animate)
                await FlyoutWindowInstance?.CloseAnimated()!;
            else
                FlyoutWindowInstance?.Close();

            FlyoutWindowInstance?.ViewModel?.GoMainPage();

            FlyoutWindowInstance = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            _closing = false;
        }
    }
}
