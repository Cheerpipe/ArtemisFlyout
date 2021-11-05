using ArtemisFlyout.Screens;
using ArtemisFlyout.Services.ArtemisServices;
using ArtemisFlyout.Services.Configuration;
using ArtemisFlyout.Services.FlyoutServices;
using ArtemisFlyout.Services.RestServices;
using ArtemisFlyout.Services.TrayIcon;
using ArtemisFlyout.UserControls;
using Ninject.Modules;

namespace ArtemisFlyout.IoC
{
    public class Bindings : NinjectModule
    {
        public override void Load()
        {
            Bind<ITrayIconService>().To<TrayIconService>().InSingletonScope();
            Bind<IFlyoutService>().To<FlyoutService>().InSingletonScope();
            Bind<IArtemisService>().To<ArtemisService>().InSingletonScope();
            Bind<IConfigurationService>().To<ConfigurationService>().InSingletonScope();
            Bind<IRestService>().To<RestService>().InSingletonScope();
            
            Bind<ArtemisDeviceTogglesViewModel>().To<ArtemisDeviceTogglesViewModel>().InSingletonScope();
            Bind<ArtemisLauncherViewModel>().To<ArtemisLauncherViewModel>().InSingletonScope();
            Bind<ArtemisLightControlViewModel>().To<ArtemisLightControlViewModel>().InSingletonScope();
            Bind<FlyoutContainerViewModel>().To<FlyoutContainerViewModel>().InSingletonScope();
           
        }
    }
}