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
        
        public void SetActivePageIndex(int newPageIndex)
        {
            ActivePageindex = newPageIndex;
            //TODO: Better handling o this

            if (newPageIndex==1)
                _flyoutService.SetHeight(580);
            else
            {
                _flyoutService.SetHeight(510);
            }
        }

        public void GoBack()
        {
            SetActivePageIndex(0);
        }

        public void Restart()
        {
            _artemisService.RestartArtemis();
        }

        public void GoHome()
        {
            _artemisService.GoHome();
        }

        public void GoWorkshop()
        {
            _artemisService.GoWorkshop();
        }

        public void GoSurfaceEditor()
        {
            _artemisService.GoSurfaceEditor();
        }

        public void ShowDebugger()
        {
            _artemisService.ShowDebugger();
        }

        public void GoSettings()
        {
            _artemisService.GoSettings();
        }
    }
}
