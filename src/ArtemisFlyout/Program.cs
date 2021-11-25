using Avalonia;
using System.Threading;
using ArtemisFlyout.Controllers;
using ArtemisFlyout.IoC;
using ArtemisFlyout.Services;
using Avalonia.Controls;
using Avalonia.ReactiveUI;

namespace ArtemisFlyout
{
    public class Program
    {
        public static CancellationTokenSource RunCancellationTokenSource { get; } = new();

        private static readonly CancellationToken RunCancellationToken = RunCancellationTokenSource.Token;

        // This method is needed for IDE previewer infrastructure
        public static AppBuilder BuildAvaloniaApp()
        {
            var builder = AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .UseReactiveUI()
                .UseSkia()
                .With(new Win32PlatformOptions()
            {
                UseWindowsUIComposition = true,
                CompositionBackdropCornerRadius = 8f,
            });
            return builder;
        }

        // The entry point. Things aren't ready yet, so at this point
        // you shouldn't use any Avalonia types or anything that expects
        // a SynchronizationContext to be ready
        public static void Main(string[] args)
        {
            BuildAvaloniaApp().Start(AppMain, args);
        }

        // Application entry point. Avalonia is completely initialized.
        static void AppMain(Application app, string[] args)
        {

            // Do you startup code here
            Kernel.Initialize(new Bindings());

            IInstanceService instanceService = Kernel.Get<IInstanceService>();
            if (instanceService.IsAlreadyRunning())
            {
                instanceService.ShowInstanceFlyout();
                return;
            }

            var webServerService = Kernel.Get<IWebServerService>();
            webServerService.AddController<FlyoutRestController>();
            webServerService.Start();

            var trayIconService = Kernel.Get<ITrayIconService>();
            trayIconService.Show();

            // Start the main loop
            app.Run(RunCancellationToken);

            // Stop things
            webServerService.Stop();
            trayIconService.Hide();
        }
    }
}
