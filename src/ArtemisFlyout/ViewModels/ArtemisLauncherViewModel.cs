using System.Reactive.Disposables;
using ArtemisFlyout.Services.LauncherServices;
using ReactiveUI;

namespace ArtemisFlyout.ViewModels
{
    public class ArtemisLauncherViewModel : ViewModelBase
    {
        private readonly ILauncherService _launcherService;
        public ArtemisLauncherViewModel(ILauncherService launcherService)
        {
            _launcherService = launcherService;
        }
        public ArtemisLauncherViewModel()
        {
            this.WhenActivated(disposables =>
            {
                Disposable.Create(() => { }).DisposeWith(disposables);
            });
        }

        public int ActivePageindex => 2;

        public void LaunchArtemis() => _launcherService.Launch();
    }
}
