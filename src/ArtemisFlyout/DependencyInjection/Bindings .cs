using ArtemisFlyout.Services;
using Ninject.Modules;

public class Bindings : NinjectModule
{
    public override void Load()
    {
        Bind<IDeviceService>().To<DeviceService>().InSingletonScope();
        Bind<ITrayIconService>().To<TrayIconService>().InSingletonScope();
    }
}