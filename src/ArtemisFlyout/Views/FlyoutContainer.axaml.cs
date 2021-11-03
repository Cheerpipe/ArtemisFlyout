using ArtemisFlyout.Services.FlyoutServices;
using ArtemisFlyout.ViewModels;
using Avalonia;
using Avalonia.Markup.Xaml;
using Ninject;
using ReactiveUI;
// ReSharper disable UnusedParameter.Local

namespace ArtemisFlyout.Views
{
    public class FlyoutContainer : FlyoutWindow<FlyoutContainerViewModel>
    {
        [Inject]
        public IFlyoutService FlyoutService { get; set; }

        public FlyoutContainer()
        {
            
            this.WhenActivated(disposables =>
            {
                /* Handle interactions etc. */
            });
            AvaloniaXamlLoader.Load(this);

#if DEBUG
            this.AttachDevTools();
#endif

            AnimationDelay = 250;
            Width = 320;
            Height = 510;
            HorizontalSpacing = 12;
            Deactivated += (_, _) =>
            {
                var x = FlyoutService;
                FlyoutService?.Close();

            };
        }
    }
}