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
            var builder = AppBuilder.Configure<App>().UsePlatformDetect().UseReactiveUI();

            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                bool dwmEnabled;
                if (DwmIsCompositionEnabled(out dwmEnabled) == 0 && dwmEnabled)
                {
                    var wp = builder.WindowingSubsystemInitializer;
                    return builder.UseWindowingSubsystem(() =>
                    {
                        wp();
                        AvaloniaLocator.CurrentMutable.Bind<IRenderTimer>().ToConstant(new WindowsDWMRenderTimer());
                    });
                }
            }

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

            TrayIconViewModel trayIconViewModel = new TrayIconViewModel();

            // Start the main loop
            app.Run(runCancellationToken);
        }

        // Animation Smoothness workaround from https://github.com/AvaloniaUI/Avalonia/issues/2945#issuecomment-534892298
        class WindowsDWMRenderTimer : IRenderTimer
        {
            public event Action<TimeSpan> Tick;
            private Thread _renderTick;
            public WindowsDWMRenderTimer()
            {
                _renderTick = new Thread(() =>
                {
                    System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                    sw.Start();
                    while (true)
                    {
                        DwmFlush();
                        Tick?.Invoke(sw.Elapsed);
                    }
                });
                _renderTick.IsBackground = true;
                _renderTick.Start();
            }
            [DllImport("Dwmapi.dll")]
            private static extern int DwmFlush();
        }
    }
}
