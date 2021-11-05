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

            AnimationDelay = 250;
            Width = 320;
            Height = 510;
            HorizontalSpacing = 12;
        }
    }
}