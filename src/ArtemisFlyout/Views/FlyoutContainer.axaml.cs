using ArtemisFlyout.ViewModels;
using Avalonia;
using Avalonia.Markup.Xaml;
using ReactiveUI;
// ReSharper disable UnusedParameter.Local

namespace ArtemisFlyout.Views
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
            HorizontalSpacing = 12;
            AnimationDelay = 250;
            Width = 320;
            Height = 510;
            Deactivated += (_, _) =>
            {
                CloseAnimated();
            };
        }
    }
}