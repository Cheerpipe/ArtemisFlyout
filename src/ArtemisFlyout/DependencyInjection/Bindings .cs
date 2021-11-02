using ArtemisFlyout.Services;
using ArtemisFlyout.Services.ArtemisServices;
using ArtemisFlyout.Services.DevicesServices;
using ArtemisFlyout.Services.FlyoutServices;
using ArtemisFlyout.Services.TrayIcon;
using Ninject.Modules;

public class Bindings : NinjectModule
{
    public override void Load()
    {
        Bind<IDeviceService>().To<DeviceService>().InSingletonScope();
        Bind<ITrayIconService>().To<TrayIconService>().InSingletonScope();
        Bind<IFlyoutService>().To<FlyoutService>().InSingletonScope();
        Bind<IArtemisService>().To<ArtemisService>().InSingletonScope();
    }
}