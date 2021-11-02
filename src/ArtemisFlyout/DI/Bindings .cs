using ArtemisFlyout.Services;
using ArtemisFlyout.Services.ArtemisServices;
using ArtemisFlyout.Services.FlyoutServices;
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
        }
    }
}