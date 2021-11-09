using ArtemisFlyout.Services;
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
            Bind<IWebServerService>().To<WebServerService>().InSingletonScope();
            Bind<IInstanceService>().To<InstanceService>().InSingletonScope();
        }
    }
}