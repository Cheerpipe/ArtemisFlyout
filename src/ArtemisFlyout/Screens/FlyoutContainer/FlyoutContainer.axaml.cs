using Avalonia;
using Avalonia.Markup.Xaml;
using ReactiveUI;

// ReSharper disable UnusedParameter.Local

namespace ArtemisFlyout.Screens
{
    public class FlyoutContainer : FlyoutWindow<FlyoutContainerViewModel>
    {
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

            RevealAnimationDelay = 250;
            ResizeAnimationDelay = 50;
            Width = 320;
            Height = 510;
        }
    }
}