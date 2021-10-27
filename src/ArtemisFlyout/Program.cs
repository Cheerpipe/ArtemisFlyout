using System;
using System.Runtime.InteropServices;
using Avalonia;
using System.Threading;
using ArtemisFlyout.ViewModels;
using ArtemisFlyout.Views;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using Avalonia.Rendering;

namespace ArtemisFlyout
{
    public class Program
    {
        [DllImport("Dwmapi.dll")]
        private static extern int DwmIsCompositionEnabled(out bool enabled);

        public static CancellationTokenSource runCancellationTokenSource = new CancellationTokenSource();
        public static MainWindow MainWindowInstance;

        static CancellationToken runCancellationToken = runCancellationTokenSource.Token;

        // This method is needed for IDE previewer infrastructure
        public static AppBuilder BuildAvaloniaApp()
        {
            var builder = AppBuilder.Configure<App>().UsePlatformDetect().UseReactiveUI().UseSkia().With(new Win32PlatformOptions()
            {
                UseWindowsUIComposition = true
            }); ;
            return builder;
        }


        // The entry point. Things aren't ready yet, so at this point
        // you shouldn't use any Avalonia types or anything that expects
        // a SynchronizationContext to be ready
        public static void Main(string[] args)
            => BuildAvaloniaApp().Start(AppMain, args);

        // Application entry point. Avalonia is completely initialized.
        static void AppMain(Application app, string[] args)
        {
            // A cancellation token source that will be used to stop the main loop
            var cts = new CancellationTokenSource();

            // Do you startup code here

            ViewModels.TrayIcon trayIconViewModel = new ViewModels.TrayIcon();

            // Start the main loop
            app.Run(runCancellationToken);
        }
    }
}
