using ArtemisFlyout.Services.ArtemisServices;
using ArtemisFlyout.Services.Configuration;
using ArtemisFlyout.Services.FlyoutServices;
using ArtemisFlyout.Services.LauncherServices;
using ArtemisFlyout.Services.RestServices;
using ArtemisFlyout.Services.TrayIcon;
using Ninject.Modules;

namespace ArtemisFlyout.DI
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
            Bind<ILauncherService>().To<LauncherService>().InSingletonScope();
        }
    }
}