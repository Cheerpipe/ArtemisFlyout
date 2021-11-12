using System.Reactive.Disposables;
using ArtemisFlyout.Services;
using ArtemisFlyout.ViewModels;
using ReactiveUI;

namespace ArtemisFlyout.Pages
{
    public class ArtemisLauncherViewModel : ViewModelBase
    {
        private readonly IArtemisService _artemisService;
        private readonly IFlyoutService _flyoutService;
        public ArtemisLauncherViewModel(IArtemisService artemisService, IFlyoutService flyoutService)
        {
            _artemisService = artemisService;
            _flyoutService = flyoutService;
        }
        public ArtemisLauncherViewModel()
        {
            this.WhenActivated(disposables =>
            {
                Disposable.Create(() => { }).DisposeWith(disposables);
            });
        }

        public int ActivePageindex { get; } = 3;

        public void LaunchArtemis()
        {
            _artemisService.Launch();
            _flyoutService.CloseAndRelease();
        }

        public double FlyoutHeight => 300;
        public double FlyoutWidth => 300;

        public bool IsArtemisRunning => false;
    }
}
