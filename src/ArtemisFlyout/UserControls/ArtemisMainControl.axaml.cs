using ArtemisFlyout.ViewModels;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
// ReSharper disable UnusedParameter.Local

namespace ArtemisFlyout.UserControls
{
    public class ArtemisControl : ReactiveUserControl<ArtemisMainControlViewModel>
    {
        public ArtemisControl()
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
