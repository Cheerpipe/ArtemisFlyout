using System.Reactive.Disposables;
using ArtemisFlyout.Pages;
using ArtemisFlyout.Services;
using ArtemisFlyout.ViewModels;
using ReactiveUI;
using Tmds.DBus;

namespace ArtemisFlyout.Screens
{
    public class FlyoutContainerViewModel : ViewModelBase
    {
        private readonly IArtemisService _artemisService;
        private readonly IFlyoutService _flyoutService;
        private int _activePageIndex;
        private const int MainPageHeight = 530;
        private const int MainPageWidth = 290;

        public FlyoutContainerViewModel(
            IArtemisService artemisService,
            ArtemisDeviceTogglesViewModel artemisDeviceTogglesViewModel,
            ArtemisLightControlViewModel artemisMainControlViewModel,
            ArtemisCustomProfileViewModel artemisCustomProfileViewModel,
            IFlyoutService flyoutService)
        {
            _artemisService = artemisService;
            ArtemisLightControlViewModel = artemisMainControlViewModel;
            ArtemisDeviceTogglesViewModel = artemisDeviceTogglesViewModel;
            ArtemisCustomProfileViewModel = artemisCustomProfileViewModel;
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


        public ArtemisLightControlViewModel ArtemisLightControlViewModel { get; }

        public ArtemisDeviceTogglesViewModel ArtemisDeviceTogglesViewModel { get; }

        public ArtemisCustomProfileViewModel ArtemisCustomProfileViewModel { get; }

        public int ActivePageindex
        {
            get => _activePageIndex;
            set => this.RaiseAndSetIfChanged(ref _activePageIndex, value);
        }

        private double _flyoutHeight= MainPageHeight;
        public double FlyoutHeight
        {
            get => _flyoutHeight;
            set
            {
                _flyoutHeight = value;
                _flyoutService.SetHeight(value);
            }
        }

        private double _flyoutWidth = MainPageWidth;
        public double FlyoutWidth
        {
            get => _flyoutWidth;
            set
            {
                _flyoutWidth = value;
                _flyoutService.SetWidth(value);
            }
        }

        public void SetActivePageIndex(int newPageIndex)
        {
            ActivePageindex = newPageIndex;
        }

        public void GoMainPage()
        {
            SetActivePageIndex(0);
            FlyoutHeight = MainPageHeight;
            FlyoutWidth = 290;
        }

        public void GoCustomProfile()
        {
            SetActivePageIndex(2);
            FlyoutHeight = (65 * ArtemisCustomProfileViewModel.Colors.Count) + 130;
            FlyoutWidth = 290;
        }

        public void GoDevices()
        {
            SetActivePageIndex(1);
            FlyoutHeight = (46 * ArtemisDeviceTogglesViewModel.DeviceStates.Count) + 110;
            FlyoutWidth = 330;
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
