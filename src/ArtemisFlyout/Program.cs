using System.Reflection;
using Avalonia;
using System.Threading;
using ArtemisFlyout.Services.TrayIcon;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using Ninject;

namespace ArtemisFlyout
{
    public class Program
    {
        public static CancellationTokenSource runCancellationTokenSource = new CancellationTokenSource();

        static CancellationToken runCancellationToken = runCancellationTokenSource.Token;

        // This method is needed for IDE previewer infrastructure
        public static AppBuilder BuildAvaloniaApp()
        {
            var builder = AppBuilder.Configure<App>().UsePlatformDetect().UseReactiveUI().UseSkia().With(new Win32PlatformOptions()
            {
                UseWindowsUIComposition = true
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

            var kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());
            var trayIconService = kernel.Get<ITrayIconService>();
            trayIconService.Show();

            // Start the main loop
            app.Run(runCancellationToken);
        }
    }
}
