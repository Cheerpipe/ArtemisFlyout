using System.Reactive.Disposables;
using ArtemisFlyout.Services;
using ArtemisFlyout.ViewModels;
using ReactiveUI;

namespace ArtemisFlyout.UserControls
{
    public class ArtemisLauncherViewModel : ViewModelBase
    {
        private readonly IArtemisService _artemisService;
        public ArtemisLauncherViewModel(IArtemisService artemisService)
        {
            _artemisService = artemisService;
        }
        public ArtemisLauncherViewModel()
        {
            this.WhenActivated(disposables =>
            {
                Disposable.Create(() => { }).DisposeWith(disposables);
            });
        }

        public int ActivePageindex { get; } = 2;

        public void LaunchArtemis() => _artemisService.Launch();
    }
}
