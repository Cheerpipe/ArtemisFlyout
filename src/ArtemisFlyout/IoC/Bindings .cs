using ArtemisFlyout.Services;
using Ninject.Modules;
using Serilog;
using Serilog.Core;

namespace ArtemisFlyout.IoC
{
    public class Bindings : NinjectModule
    {
        public override void Load()
        {
            Bind<ITrayIconService>().To<WindowsTrayIconService>().InSingletonScope();
            Bind<IFlyoutService>().To<FlyoutService>().InSingletonScope();
            Bind<IArtemisService>().To<ArtemisService>().InSingletonScope();
            Bind<IConfigurationService>().To<ConfigurationService>().InSingletonScope();
            Bind<IRestService>().To<RestService>().InSingletonScope();
            Bind<IWebServerService>().To<WebServerService>().InSingletonScope();
            Bind<IInstanceService>().To<InstanceService>().InSingletonScope();
            Bind<ILogger>().To<Logger>().InSingletonScope();
        }
    }
}