using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
// ReSharper disable UnusedParameter.Local

namespace ArtemisFlyout.UserControls
{
    public class ArtemisLauncher : ReactiveUserControl<ArtemisLauncher>
    {
        public ArtemisLauncher()
        {
            // If you put a WhenActivated block here, your activatable view model 
            // will also support activation, otherwise it won't.
            this.WhenActivated(disposables =>
            {
            });
            AvaloniaXamlLoader.Load(this);
        }
    }
}
