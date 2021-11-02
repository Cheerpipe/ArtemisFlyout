using System.Reactive.Disposables;
using ArtemisFlyout.Services.ArtemisServices;
using ArtemisFlyout.Services.FlyoutServices;
using ReactiveUI;

namespace ArtemisFlyout.ViewModels
{
    public class FlyoutContainerViewModel : ViewModelBase
    {
        private readonly IArtemisService _artemisService;
        private readonly ArtemisDeviceTogglesViewModel _artemisDeviceTogglesViewModel;
        private readonly ArtemisMainControlViewModel _artemisMainControlViewModel;
        private readonly IFlyoutService _flyoutService;
        private int _activePageIndex;

        public FlyoutContainerViewModel(
            IArtemisService artemisService,
            ArtemisDeviceTogglesViewModel artemisDeviceTogglesViewModel,
            ArtemisMainControlViewModel artemisMainControlViewModel,
            IFlyoutService flyoutService)
        {
            _artemisService = artemisService;
            _artemisMainControlViewModel = artemisMainControlViewModel;
            _artemisDeviceTogglesViewModel = artemisDeviceTogglesViewModel;
            _flyoutService = flyoutService;


            this.WhenActivated(disposables =>
            {
                /* Handle activation */
                Disposable
                    .Create(() =>
                    {
                        /* Handle deactivation */
                    })
                    .DisposeWith(disposables);
            });
        }

        public ArtemisMainControlViewModel ArtemisMainControlViewModel => _artemisMainControlViewModel;
        public ArtemisDeviceTogglesViewModel ArtemisDeviceTogglesViewModel => _artemisDeviceTogglesViewModel;

        public int ActivePageindex
        {
            get => _activePageIndex;
            set
            {
                this.RaiseAndSetIfChanged(ref _activePageIndex, value);
            }
        }


        public async void SetActivePageIndex(int newPageIndex)
        {
            ActivePageindex = newPageIndex;
        }

        public async void GoBack()
        {
            ActivePageindex = 0;
        }

        private async void Restart()
        {
            _artemisService.RestartArtemis();
        }

        public async void GoHome()
        {
            _artemisService.GoHome();
        }

        public async void GoWorkshop()
        {
            _artemisService.GoWorkshop();
        }

        public async void GoSurfaceEditor()
        {
            _artemisService.GoSurfaceEditor();
        }

        public async void ShowDebugger()
        {
            _artemisService.ShowDebugger();
        }

        public async void GoSettings()
        {
            _artemisService.GoSettings();
        }
    }
}
