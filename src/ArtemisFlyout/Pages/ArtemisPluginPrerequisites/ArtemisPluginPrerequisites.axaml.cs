using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;

// ReSharper disable UnusedParameter.Local

namespace ArtemisFlyout.Pages
{
    public class ArtemisPluginPrerequisites : ReactiveUserControl<ArtemisPluginPrerequisitesViewModel>
    {
        public ArtemisPluginPrerequisites()
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
