using System.Reactive.Disposables;
using ArtemisFlyout.Services;
using ArtemisFlyout.ViewModels;

namespace ArtemisFlyout.Pages
{
    public class ArtemisPluginPrerequisitesViewModel : ViewModelBase
    {
        public double FlyoutHeight => 500;
        public double FlyoutWidth => 500;
        public int ActivePageindex { get; } = 4;

    }
}
