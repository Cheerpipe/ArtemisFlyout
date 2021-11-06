using System.Reactive.Disposables;
using System.Reflection.Metadata.Ecma335;
using ArtemisFlyout.Services;
using ArtemisFlyout.UserControls;
using ArtemisFlyout.ViewModels;
using ReactiveUI;
using Tmds.DBus;

namespace ArtemisFlyout.Screens
{
    public class FlyoutContainerViewModel : ViewModelBase
    {
        private readonly IArtemisService _artemisService;
        private readonly ArtemisDeviceTogglesViewModel _artemisDeviceTogglesViewModel;
        private readonly ArtemisLightControlViewModel _artemisLightControlViewModel;
        private readonly IFlyoutService _flyoutService;
        private int _activePageIndex;

        public FlyoutContainerViewModel(
            IArtemisService artemisService,
            ArtemisDeviceTogglesViewModel artemisDeviceTogglesViewModel,
            ArtemisLightControlViewModel artemisMainControlViewModel,
            IFlyoutService flyoutService)
        {
            _artemisService = artemisService;
            _artemisLightControlViewModel = artemisMainControlViewModel;
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

            //Test artemis

            if (!_artemisService.TestRestApi())
                throw new ConnectException("Artemis REST API not available.");
        }

        public ArtemisLightControlViewModel ArtemisLightControlViewModel => _artemisLightControlViewModel;
        public ArtemisDeviceTogglesViewModel ArtemisDeviceTogglesViewModel => _artemisDeviceTogglesViewModel;

        public void Reset()
        {
            GoMainPage();
        }

        public int ActivePageindex
        {
            get => _activePageIndex;
            set
            {
                this.RaiseAndSetIfChanged(ref _activePageIndex, value);
            }
        }

        private double _flyoutHeight = 510;
        public double FlyoutHeight
        {
            get
            {
                return _flyoutHeight;
            }
            set
            {
                _flyoutHeight = value;
                _flyoutService.SetHeight(value);
            }

        }

        public void SetActivePageIndex(int newPageIndex)
        {
            ActivePageindex = newPageIndex;
        }

        public void GoMainPage()
        {
            SetActivePageIndex(0);
            FlyoutHeight = 510;
        }

        public void GoDevices()
        {
            SetActivePageIndex(1);
            FlyoutHeight = (46 * ArtemisDeviceTogglesViewModel.Blackouts.Count) + 110;
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
